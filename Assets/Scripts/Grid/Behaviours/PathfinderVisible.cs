using System;
using System.Collections;
using System.Collections.Generic;
using Grid.Controllers;
using UnityEngine;

namespace Grid.Behaviours
{
    public class PathfinderVisible : MonoBehaviour
    {
        private Pathfinding _pathfinding;
        [SerializeField] private GameObject _walkablePrefab;
        [SerializeField] private GameObject _notWalkablePrefab;

        private List<GameObject> _sprites;

        private IEnumerator Start()
        {
            yield return null;
            yield return null;

            _pathfinding = GeneralPathfindingGrid.Pathfinding;

            _sprites = new List<GameObject>();
            
            Visualize();
        }

        private void Visualize()
        {
            foreach (var node in _pathfinding.Grid.GridArray)
            {
                var position = new Vector3(node.HorizontalPosition * _pathfinding.Grid.CellSize,
                    node.VerticalPosition * _pathfinding.Grid.CellSize, 0);

                _sprites.Add(Instantiate(node.IsWalkable ? _walkablePrefab : _notWalkablePrefab, position,
                    Quaternion.identity));
            }
        }
    }
}