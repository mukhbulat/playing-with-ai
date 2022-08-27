using System;
using System.Collections.Generic;
using System.Linq;
using Grid.Data;
using UnityEngine;

namespace Grid.Controllers
{
    public class Pathfinding
    {
        public GridData<PathNode> Grid => _grid;

        private const int MoveDiagonalCost = 14;
        private const int MoveStraightCost = 10;
        
        public Pathfinding(int width, int height, float cellSize, Vector3 origin)
        {
            _grid = new GridData<PathNode>(width, height, cellSize, origin, (GridData<PathNode> grid, int x, int y) => new PathNode(grid, x, y));
        }
        
        private GridData<PathNode> _grid;

        private List<PathNode> _openList;
        private HashSet<PathNode> _closedList;

        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = _grid.GetValue(startX, startY);
            PathNode endNode = _grid.GetValue(endX, endY);
            if (startNode == null)
            {
                throw new Exception("Start node is null");
            }

            if (endNode == null)
            {
                throw new Exception("End node is null");
            }
            
            _openList = new List<PathNode>() { startNode };
            _closedList = new HashSet<PathNode>();

            for (int i = 0; i < _grid.Width; i++)
            {
                for (int j = 0; j < _grid.Height; j++)
                {
                    PathNode pathNode = _grid.GetValue(i, j);
                    pathNode.GCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.PreviousNode = null;
                }
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode, endNode);
            
            startNode.CalculateFCost();

            while (_openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(_openList);
                if (currentNode == endNode)
                {
                    return CalculatePathNode(endNode);
                }

                _openList.Remove(currentNode);
                _closedList.Add(currentNode);

                foreach (var neighbourNode in GetNeighbourNodes(currentNode))
                {
                    if (_closedList.Contains(neighbourNode))
                    {
                        continue;
                    }

                    int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.GCost)
                    {
                        neighbourNode.PreviousNode = currentNode;
                        neighbourNode.GCost = tentativeGCost;
                        neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        if (!_openList.Contains(neighbourNode))
                        {
                            _openList.Add(neighbourNode);
                        }
                    }
                }
            }

            return null;
        }

        private HashSet<PathNode> GetNeighbourNodes(PathNode currentNode)
        {
            HashSet<PathNode> neighbors = new HashSet<PathNode>();
            bool leftFree = currentNode.HorizontalPosition - 1 >= 0;
            bool rightFree = currentNode.HorizontalPosition + 1 < _grid.Width;
            bool downFree = currentNode.VerticalPosition - 1 >= 0;
            bool upFree = currentNode.VerticalPosition + 1 < _grid.Height;

            if (leftFree)
            {
                neighbors.Add(GetNode(currentNode.HorizontalPosition - 1, currentNode.VerticalPosition));
                if (downFree)
                    neighbors.Add(GetNode(currentNode.HorizontalPosition - 1, currentNode.VerticalPosition - 1));
                if (upFree)
                    neighbors.Add(GetNode(currentNode.HorizontalPosition - 1, currentNode.VerticalPosition + 1));
            }

            if (rightFree)
            {
                neighbors.Add(GetNode(currentNode.HorizontalPosition + 1, currentNode.VerticalPosition));
                if (downFree)
                    neighbors.Add(GetNode(currentNode.HorizontalPosition + 1, currentNode.VerticalPosition - 1));
                if (upFree)
                    neighbors.Add(GetNode(currentNode.HorizontalPosition + 1, currentNode.VerticalPosition + 1));
            }

            if (upFree) 
                neighbors.Add(GetNode(currentNode.HorizontalPosition, currentNode.VerticalPosition + 1));
            if (downFree)
                neighbors.Add(GetNode(currentNode.HorizontalPosition, currentNode.VerticalPosition - 1));

            return neighbors;
        }

        private PathNode GetNode(int x, int y)
        {
            return _grid.GridArray[x, y];
        }
        
        private List<PathNode> CalculatePathNode(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);
            PathNode currentNode = endNode;
            while (currentNode.PreviousNode != null)
            {
                path.Add(currentNode.PreviousNode);
                currentNode = currentNode.PreviousNode;
            }

            path.Reverse();
            return path;
        }

        private int CalculateDistanceCost(PathNode startNode, PathNode endNode)
        {
            int horDistance = Mathf.Abs(startNode.HorizontalPosition - endNode.HorizontalPosition);
            int verDistance = Mathf.Abs(startNode.VerticalPosition - endNode.VerticalPosition);
            int left = Mathf.Abs(horDistance - verDistance);
            return MoveDiagonalCost * Mathf.Min(horDistance, verDistance) + MoveStraightCost * left;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodes)
        {
            PathNode lowestCostNode = pathNodes[0];
            foreach (var node in pathNodes)
            {
                if (node.FCost < lowestCostNode.FCost)
                {
                    lowestCostNode = node;
                }
            }

            return lowestCostNode;
        }
    }
}