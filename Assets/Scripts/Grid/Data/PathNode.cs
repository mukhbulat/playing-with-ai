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
        public int FCost { get; private set; }
        
        public bool IsWalkable { get; private set; }

        public PathNode(GridData<PathNode> grid, int x, int y)
        {
            _grid = grid;
            HorizontalPosition = x;
            VerticalPosition = y;
            IsWalkable = true;
        }

        public void CalculateFCost()
        {
            FCost = GCost + HCost;
        }

        public void ChangeWalkable(bool isWalkable)
        {
            IsWalkable = isWalkable;
        }
    }
}