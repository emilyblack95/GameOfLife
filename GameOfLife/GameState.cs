namespace GameOfLife
{
    /// <summary>
    ///  Class for any given state in Conway's Game of Life
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// KVP of each Alive Coordinate <--> List of Dead Neighbors on the board
        /// </summary>
        public Dictionary<Point, HashSet<Point>> Coordinates { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GameState()
        {
            Coordinates = new();
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        public GameState(Dictionary<Point, HashSet<Point>> coordinates)
        {
            Coordinates = coordinates;
        }
    }
}
