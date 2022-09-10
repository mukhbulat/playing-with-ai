using System;
using Grid.Controllers;
using UnityEngine;

namespace Grid.Behaviours
{
    public class GeneralPathfindingGrid : MonoBehaviour
    {
        public static Pathfinding Pathfinding { get; private set; }

        [SerializeField] private int _cellsHorizontal = 50;
        [SerializeField] private int _cellsVertical = 50;
        [SerializeField] private float _cellSize = 1f;
        [SerializeField] private Vector3 _originPoint = Vector3.zero;
        
        private void Awake()
        {
            Pathfinding = new Pathfinding(_cellsHorizontal, _cellsVertical, _cellSize, _originPoint);
        }
    }
}