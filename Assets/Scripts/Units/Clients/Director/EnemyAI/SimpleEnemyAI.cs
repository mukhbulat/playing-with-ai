using System;
using System.Collections;
using System.Collections.Generic;
using Grid.Controllers;
using Units.Clients.Director.EnemyAI.States;
using UnityEngine;
using IUnit = Units.Controllers.IUnit;

namespace Units.Clients.Director.EnemyAI
{
    public class SimpleEnemyAI : MonoBehaviour
    {
        private IUnit _unit;
        private IUnit _player;

        // Current Action:
        public event Action CurrentActionEnded;
        
        private Coroutine _currentActionCoroutine;
        
        // Pathing and movement:
        private Pathfinding _pathfinding;
        [SerializeField] private List<Transform> _waypoints;
        private int _currentWaypoint;
        private Vector3 _currentGlobalDestination;
        private Queue<Vector3> _pathNodes;
        private Vector3 _currentPathNodeDestination;

        // Do not turn on until PathfindingMovement() wont work
        // better in precision mode. (Never? Don't need this anyway)
        private bool _precisionMovement = false;
        
        // Seeking player and attacking him:
        public CheckPlayerState CheckPlayerState { get; private set; }
        [SerializeField] private float _playerCloseRange = 10f;
        [SerializeField] private float _playerOuterRange = 12f;
        private float _attackRange;
        private int _objectAndPlayerLayerMask = (1 << 7) + (1 << 6);
        private Vector3 _lastSeenPosition;

        // State machine:
        public State Attack => _attack;
        public State Chase => _chase;
        public State Hide => _hide;
        public State Patrol => _patrol;
        
        private EnemyStateMachine _stateMachine;
        private State _attack, _chase, _hide, _patrol;

        private IEnumerator Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<IUnit>();
            yield return null;
            
            _pathfinding = FindObjectOfType<Grid.Behaviours.GeneralPathfindingGrid>().Pathfinding;
            
            _unit = GetComponent<IUnit>();
            _attackRange = _unit.Attacking.AttackRange;
            

            ResetStateMachine();
        }

        private void ResetStateMachine()
        {
            _stateMachine = new EnemyStateMachine();
            _attack = new AttackState(_stateMachine, this);
            _chase = new ChaseState(_stateMachine, this);
            _hide = new HideState(_stateMachine, this);
            _patrol = new PatrolState(_stateMachine, this);
            
            _stateMachine.Initialize(_patrol);
        }

        private void Update()
        {
            CheckPlayerState = CheckPlayer();
            _stateMachine?.CurrentState.LogicUpdate();
        }

        public void StartChasing()
        {
            if (_currentActionCoroutine != null)
            {
                StopCoroutine(_currentActionCoroutine);
            }

            _currentActionCoroutine = StartCoroutine(ChaseCoroutine());
        }
        
        public void StartPatrol()
        {
            if (_currentWaypoint >= _waypoints.Count - 1)
            {
                _currentWaypoint = 0;
            }
            else
            {
                _currentWaypoint += 1;
            }

            MoveToDestination(_waypoints[_currentWaypoint].position);
        }

        public void StartAttack()
        {
            if (_currentActionCoroutine != null)
            {
                StopCoroutine(_currentActionCoroutine);
            }

            _currentActionCoroutine = StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            while (CheckPlayerState == CheckPlayerState.InAttackRange)
            {
                _unit.Attacking.StartAttack(_player.Transform.position);
                yield return null;
            }
        }

        private IEnumerator ChaseCoroutine()
        {
            while (CheckPlayerState != CheckPlayerState.NotVisible)
            {
                while ((transform.position - _player.Transform.position).magnitude > (_attackRange * 0.8f))
                {
                    // Find a path, if it's not null, run to the player and check if his position changed too
                    // much every couple of seconds.
                    _currentGlobalDestination = _player.Transform.position;
                    
                    PathfinderResetNodeList();

                    if (_pathNodes == null || _pathNodes.Count == 0)
                    {
                        CurrentActionEnded?.Invoke();
                        yield break;
                        
                    }
                    
                    while (_pathNodes.Count > 0)
                    {
                        while ((_pathNodes.Peek() - transform.position).magnitude >= 0.05f)
                        {
                            _unit.Movable.Move((_pathNodes.Peek() - transform.position).normalized);
                            yield return null;
                        }

                        _pathNodes.Dequeue();
                    }
                }

                if (CheckPlayerState == CheckPlayerState.InAttackRange)
                {
                    CurrentActionEnded?.Invoke();
                    yield break;
                }
            }
        }

        private IEnumerator PathfindingMovementCoroutine()
        {
            PathfinderResetNodeList();

            if (_pathNodes == null || _pathNodes.Count == 0)
            {
                CurrentActionEnded?.Invoke();
                yield break;
            }

            while (_pathNodes.Count > 0)
            {
                while ((_pathNodes.Peek() - transform.position).magnitude >= 0.05f)
                {
                    _unit.Movable.Move((_pathNodes.Peek() - transform.position).normalized);
                    yield return null;
                }

                _pathNodes.Dequeue();
            }
            
            // Don't want to spoil smooth movement, so precision (I guess its not necessary after all.
            // If turned on, unit will move after reaching destination cell to destination location. 

            if (_precisionMovement)
            {
                while ((_currentGlobalDestination - transform.position).magnitude > 0.05f)
                {
                    _unit.Movable.Move((_currentGlobalDestination - transform.position).normalized);
                    yield return null;
                }
            }

            CurrentActionEnded?.Invoke();
        }

        private void PathfinderResetNodeList()
        {
            var startNode = _pathfinding.Grid.GetValue(transform.position);
            var endNode = _pathfinding.Grid.GetValue(_currentGlobalDestination);
            var nodes = _pathfinding.FindPath(startNode, endNode);

            if (nodes == null || nodes.Count == 0)
            {
                _pathNodes = null;
                return;
            }

            _pathNodes = new Queue<Vector3>();
            foreach (var node in nodes)
            {
                var nodePosition = new Vector3(node.HorizontalPosition * _pathfinding.Grid.CellSize + _pathfinding.Grid.CellSize * 0.5f,
                    node.VerticalPosition * _pathfinding.Grid.CellSize + _pathfinding.Grid.CellSize * 0.5f, 0);
                _pathNodes.Enqueue(nodePosition);
            }
            
            _pathNodes.Dequeue();
        }
        
        private void MoveToDestination(Vector3 destinationPoint)
        {
            _currentGlobalDestination = destinationPoint;

            if (_currentActionCoroutine != null)
            {
                StopCoroutine(_currentActionCoroutine);
            }

            _currentActionCoroutine = StartCoroutine(PathfindingMovementCoroutine());
        }

        private CheckPlayerState CheckPlayer()
        {
            var position = transform.position;
            if (_player == null)
            {
                return CheckPlayerState.NotVisible;
            }
            var hit = Physics2D.Raycast(position, _player.Transform.position - position, 
                Mathf.Infinity, _objectAndPlayerLayerMask);
            
            if (hit.transform == _player.Transform)
            {
                var hitDistance = hit.distance;

                if (hitDistance < _attackRange)
                {
                    _lastSeenPosition = _player.Transform.position;
                    return CheckPlayerState.InAttackRange;
                }
                
                if (hitDistance < _playerCloseRange)
                {
                    _lastSeenPosition = _player.Transform.position;
                    return CheckPlayerState.InCloseRange;
                }
                
                if (hitDistance < _playerOuterRange)
                {
                    _lastSeenPosition = _player.Transform.position;
                    return CheckPlayerState.InOuterRange;
                }
            }

            return CheckPlayerState.NotVisible;
        }
    }
}