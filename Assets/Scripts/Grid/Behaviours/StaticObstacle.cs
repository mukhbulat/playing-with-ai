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
            _pathfinding = GeneralPathfindingGrid.Pathfinding;
            
            ChangeWalkable();
        }

        private void ChangeWalkable()
        {
            var bounds = _collider.bounds;
            _pathfinding.Grid.GetValue(bounds.min, out int x1, out int y1);
            _pathfinding.Grid.GetValue(bounds.max, out int x2, out int y2);

            if (x2 < x1)
            {
                (x1, x2) = (x2, x1);
            }

            if (y2 < y1)
            {
                (y1, y2) = (y2, y1);
            }
            
            /*
            if (x1 > 0)
            {
                x1 -= 1;
            }

            if (y1 > 0)
            {
                y1 -= 1;
            }

            if (x2 < _pathfinding.Grid.Width - 1)
            {
                x2 += 1;
            }

            if (y2 < _pathfinding.Grid.Height - 1)
            {
                y2 += 1;
            }
            */

            if (x1 == x2)
            {
                x2 += 1;
            }

            if (y1 == y2)
            {
                y2 += 1;
            }
            for (int i = x1; i < x2; i++)
            {
                for (int j = y1; j < y2; j++)
                {
                    _pathfinding.Grid.GetValue(i, j).ChangeWalkable(false);
                }
            }
        }

    }
}