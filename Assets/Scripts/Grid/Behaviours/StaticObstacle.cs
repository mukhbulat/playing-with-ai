using System;
using System.Collections;
using Grid.Controllers;
using UnityEngine;

namespace Grid.Behaviours
{
    [RequireComponent(typeof(Collider2D))]
    public class StaticObstacle : MonoBehaviour
    {
        // Right now only box obstacles

        private BoxCollider2D _collider;
        private Pathfinding _pathfinding;

        private IEnumerator Start()
        {
            yield return null;

            _collider = GetComponent<BoxCollider2D>();
            _pathfinding = FindObjectOfType<GeneralPathfindingGrid>().Pathfinding;
            
            ChangeWalkable();
        }

        private void ChangeWalkable()
        {
            var bounds = _collider.bounds;
            _pathfinding.Grid.GetValue(bounds.min, out int x1, out int y1);
            _pathfinding.Grid.GetValue(bounds.max, out int x2, out int y2);

            if (x1 < 0 || x2 < 0 || y1 < 0 || y2 < 0)
            {
                throw new Exception($"Bounds of Static Obstacle {gameObject.name} are out of grid.");
            }
            
            if (x2 < x1)
            {
                (x1, x2) = (x2, x1);
            }

            if (y2 < y1)
            {
                (y1, y2) = (y2, y1);
            }

            for (int i = x1; i < x2; i++)
            {
                for (int j = y1; j < y2; j++)
                {
                    if (_pathfinding == null) Debug.Log($"Pathfinding is null : {i} {j}");
                    if (_pathfinding.Grid == null) Debug.Log($"Grid is null : {i} {j}");
                    if (_pathfinding.Grid.GetValue(i, j) == null) Debug.Log($"PathNode is null : {i} {j}");
                    _pathfinding.Grid.GetValue(i, j).ChangeWalkable(false);
                }
            }
        }

    }
}