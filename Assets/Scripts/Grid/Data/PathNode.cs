namespace Grid.Data
{
    public class PathNode
    {
        public PathNode PreviousNode { get; set; }

        private GridData<PathNode> _grid;
        public int HorizontalPosition { get; private set; } 
        public int VerticalPosition { get; private set; }

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get; set; }

        public PathNode(GridData<PathNode> grid, int x, int y)
        {
            _grid = grid;
            HorizontalPosition = x;
            VerticalPosition = y;
        }

        public void CalculateFCost()
        {
            FCost = GCost + HCost;
        }
    }
}