namespace GameOfLife
{
    /// <summary>
    /// Represents a point on the Game of Life board.
    /// </summary>
    public class Point
    {
        /// <summary>
        /// X coord.
        /// </summary>
        public long X { get; set; }

        /// <summary>
        /// Y coord.
        /// </summary>
        public long Y { get; set; }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="x">x coord</param>
        /// <param name="y">y coord</param>
        public Point(long x, long y)        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj != null ? (obj as Point)!.X == this.X && (obj as Point)!.Y == this.Y : false;
        }
    }
}
