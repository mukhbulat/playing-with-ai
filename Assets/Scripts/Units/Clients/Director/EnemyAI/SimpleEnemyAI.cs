using System;
using System.Collections;
using System.Collections.Generic;
using Grid.Controllers;
using Grid.Data;
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
        
        // Seeking player:
        [SerializeField] private float _playerVisibilityRange = 10f;
        private int _ObjectPlayerLayerMask = (1 << 7) + (1 << 6);

        // State machine:
        private EnemyStateMachine _stateMachine;
        private State _attack, _chase, _hide, _patrol;

        private IEnumerator Start()
        {
            yield return null;
            
            _pathfinding = FindObjectOfType<Grid.Behaviours.GeneralPathfindingGrid>().Pathfinding;
            _unit = GetComponent<IUnit>();
            _player = GameObject.FindWithTag("Player").GetComponent<IUnit>();
            
            if (_player == null)
            {
                Debug.Log("Player is not found");
            }
            
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
            _stateMachine?.CurrentState.LogicUpdate();
        }

        public void MoveToNextWaypoint()
        {
            if (_currentWaypoint >= _waypoints.Count - 1)
            {
                _currentWaypoint = 0;
            }
            else
            {
                _currentWaypoint += 1;
            }

            _currentGlobalDestination = _waypoints[_currentWaypoint].position;

            if (_currentActionCoroutine != null)
            {
                StopCoroutine(_currentActionCoroutine);
            }

            _currentActionCoroutine = StartCoroutine(PathfindingMovement());
        }
        
        private IEnumerator PathfindingMovement()
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

        public void CheckPlayer()
        {
            var position = transform.position;
            var hit = Physics2D.Raycast(position, _player.Transform.position - position, 
                Mathf.Infinity, _ObjectPlayerLayerMask);
            
            if (hit.transform == _player.Transform)
            {
                if (hit.distance < _playerVisibilityRange)
                {
                    Debug.Log("Found player in range");
                }
            }
        }
    }
}