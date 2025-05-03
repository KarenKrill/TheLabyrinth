namespace TheLabyrinth.GameFlow.Abstractions
{
    public enum MazeShape
    {
        Custom,
        Triangle,
        Square,
        Circle
    }
    public struct LevelInfo
    {
        public int Index;
        public string Name;
        public MazeShape MazeShape;
        public int MazeLevelsCount;
        public LevelInfo(int index, string name, MazeShape mazeShape, int mazeLevelsCount)
        {
            Index = index;
            Name = name;
            MazeShape = mazeShape;
            MazeLevelsCount = mazeLevelsCount;
        }
    }
}
