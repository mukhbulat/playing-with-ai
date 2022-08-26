using UnityEngine;

namespace Grid.Data
{
    public class GridData<TCell>
    {
        public TCell[,] GridArray => _gridArray;
        
        private int _width;
        private int _height;
        private float _cellSize;
        private TCell[,] _gridArray;
        private Vector3 _origin;

        public GridData(int width, int height, float cellSize, Vector3 origin, TCell cell)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _origin = origin;

            _gridArray = new TCell[width, height];
        }

        /*
        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellSize + _origin;
        }
        */

        private void GetArrayElement(Vector3 worldPosition, out int x, out int y)
        {
            x = -1;
            y = -1;
            x = Mathf.FloorToInt((worldPosition - _origin).x / _cellSize);
            y = Mathf.FloorToInt((worldPosition - _origin).y / _cellSize);
        }

        public void SetValue(int x, int y, TCell value)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _gridArray[x, y] = value;
            }
        }

        public void SetValue(Vector3 worldPosition, TCell value)
        {
            int x, y;
            GetArrayElement(worldPosition, out x, out y);
            SetValue(x, y, value);
        }

        public bool GetValue(int x, int y, ref TCell value)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                value = _gridArray[x, y];
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetValue(Vector3 worldPosition, ref TCell value)
        {
            int x, y;
            GetArrayElement(worldPosition, out x, out y);
            return GetValue(x, y, ref value);
        }
    }
}