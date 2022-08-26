using System;
using System.Collections.Generic;
using Grid.Data;
using UnityEngine;

namespace Grid.Controllers
{
    public class Pathfinding
    {
        public Pathfinding(int width, int height, float cellSize, Vector3 origin)
        {
            _grid = new GridData<PathNode>(width, height, cellSize, origin);
        }
        
        private GridData<PathNode> _grid;

        private List<PathNode> _openList;
        private HashSet<PathNode> _closedList;

        private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = null;
            _grid.GetValue(startX, startY, ref startNode);
            if (startNode == null)
            {
                throw new Exception("Can't find start node");
            }
            
            _openList = new List<PathNode>() { startNode };
            _closedList = new HashSet<PathNode>();

            throw new NotImplementedException();
        }
    }
}