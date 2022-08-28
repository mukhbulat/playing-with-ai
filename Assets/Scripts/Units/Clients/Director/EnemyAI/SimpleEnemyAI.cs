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
        public event Action CurrentActionEnded;
        
        [SerializeField] private List<Transform> _waypoints;
        private int _currentWaypoint;
        private Vector3 _currentGlobalDestination;
        private Queue<Vector3> _pathNodes;
        private Vector3 _currentPathNodeDestination;
        
        private Coroutine _currentActionCoroutine;
        
        private IUnit _unit;
        private Pathfinding _pathfinding;

        
        private EnemyStateMachine _stateMachine;
        private State _attack, _chase, _hide, _patrol;

        private void Awake()
        {
            
        }

        private IEnumerator Start()
        {
            yield return null;
            
            _pathfinding = FindObjectOfType<Grid.Behaviours.GeneralPathfindingGrid>().Pathfinding;
            _unit = GetComponent<IUnit>();

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

        public bool CheckIfDestinationReached()
        {
            return (transform.position - _currentGlobalDestination).magnitude < 0.05f;
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
                var vectorToNextNode = (_pathNodes.Peek() - transform.position);
                
                while (vectorToNextNode.magnitude >= 0.05f)
                {
                    _unit.Movable.Move(vectorToNextNode.normalized);
                    yield return null;
                }

                _pathNodes.Dequeue();
            }

            var vectorToDestination = (_currentGlobalDestination - transform.position);
            while (vectorToDestination.magnitude > 0.05f)
            {
                _unit.Movable.Move(vectorToDestination.normalized);
                yield return null;
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
                var nodePosition = new Vector3(node.HorizontalPosition * _pathfinding.Grid.CellSize,
                    node.VerticalPosition * _pathfinding.Grid.CellSize, 0);
                _pathNodes.Enqueue(nodePosition);
            }
            
            // 
            Debug.Log("Removed first node");
            _pathNodes.Dequeue();
        }
    }
}