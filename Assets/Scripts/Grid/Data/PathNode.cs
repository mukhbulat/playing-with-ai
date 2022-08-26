namespace Grid.Data
{
    public class PathNode
    {
        public PathNode PreviousNode { get; set; }

        private GridData<PathNode> _grid;
        private int _horizontalPosition;
        private int _verticalPosition;

        public int GCost;
        public int HCost;
        public int FCost;

        public PathNode(GridData<PathNode> grid, int x, int y)
        {
            _grid = grid;
            _horizontalPosition = x;
            _verticalPosition = y;
        }
        
        
    }
}