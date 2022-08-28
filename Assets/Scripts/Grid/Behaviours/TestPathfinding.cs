using System.Collections.Generic;
using Grid.Controllers;
using Grid.Data;
using UnityEngine;

namespace Grid.Behaviours
{
    public class TestPathfinding : MonoBehaviour
    {
        [SerializeField] private GameObject _endGameObject;
        [SerializeField] private int _width = 10;
        [SerializeField] private int _height = 10;
        [SerializeField] private float _cellSize = 1f;
        [SerializeField] private Vector3 _origin = Vector3.zero;
        
        private Pathfinding _pathfinding;

        private void Start()
        {
            _pathfinding = FindObjectOfType<GeneralPathfindingGrid>().Pathfinding;
        }

        private void Update()
        {
            Vector3 gameObjectPosition = _endGameObject.transform.position;
            gameObjectPosition.z = 0;
            _pathfinding.Grid.GetValue(gameObjectPosition, out int x, out int y);
            List<PathNode> path = _pathfinding.FindPath(3, 3, x, y);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].HorizontalPosition, path[i].VerticalPosition), 
                        new Vector3(path[i + 1].HorizontalPosition, path[i + 1].VerticalPosition));
                }
            }
            else
            {
                Debug.Log("Cant find path :(");
            }
        }
    }
}