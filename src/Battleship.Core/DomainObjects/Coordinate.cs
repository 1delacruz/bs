using System;
using System.Diagnostics;

namespace Battleship.Core
{
    /// <summary>
    /// Represents a point in the game board.
    /// </summary>
    [DebuggerDisplay("Y = {Y}, X = {X}, IsHit = {IsHit}")]
    public class Coordinate : IEquatable<Coordinate>
    {
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the x-axis of the coordinate.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the y-axis of the coordinate.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Gets or sets the values whether the coordinate has been hit.
        /// </summary>
        public bool IsHit { get; set; }

        public bool Equals(Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}
