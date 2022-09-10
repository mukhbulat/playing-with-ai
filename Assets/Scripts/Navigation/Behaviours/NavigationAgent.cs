using System.Collections;
using System.Collections.Generic;
using Grid.Controllers;
using Grid.Data;
using UnityEngine;
using Navigation.Controllers;

namespace Navigation.Behaviours
{
    [AddComponentMenu("2D Navigation Agent")]
    public class NavigationAgent : MonoBehaviour, IMovement, ITakesSpace
    {
        public float Speed => _speed;
        public Vector3 Position { get; }
        public float Size { get; }
        
        [SerializeField] private float _speed = 10;
        
        private Coroutine _movementCoroutine;
        private Vector3 _currentGlobalDestination;
        private Pathfinding _pathfinding;
        private Stack<PathNode> _pathNodes;
        
        private float _distanceToNextNode;
        private float _movementDelta;

        private void Awake()
        {
            _pathfinding = Grid.Behaviours.GeneralPathfindingGrid.Pathfinding;
            _movementDelta = (_speed + 1) * Time.deltaTime;
        }

        public void Move(Vector3 point)
        {
            _currentGlobalDestination = point;
        }

        public void Stop()
        {
            if (_movementCoroutine != null)
            {
                StopCoroutine(_movementCoroutine);
            }
        }

        public void ChangeSpeed(float newSpeed)
        {
            _speed = newSpeed;
            _movementDelta = (_speed + 1) * Time.deltaTime;
        }

        private IEnumerator MovementCoroutine()
        {
            if (!ResetPathfinding())
            {
                yield break;
            }

            while (_pathNodes.Count > 0)
            {
                while (_distanceToNextNode > _movementDelta)
                {
                    
                }
            }
        }

        private void CalculateDistanceToNextNode()
        {
            
        }

        private bool ResetPathfinding()
        {
            var startNode = _pathfinding.Grid.GetValue(transform.position);
            var endNode = _pathfinding.Grid.GetValue(_currentGlobalDestination);
            var result = _pathfinding.FindPath(startNode, endNode, ref _pathNodes);
            if (!result) return false;
            _pathNodes.Pop();
            return true;
        }
    }
}