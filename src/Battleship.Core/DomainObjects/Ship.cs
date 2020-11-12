using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Battleship.Core.DomainObjects
{
    /// <summary>
    /// Represents a ship.
    /// </summary>
    [DebuggerDisplay("Length = {Length}, HasSunk = {HasSunk}, Coordinates = [{CoordinatesDisplay}]")]
    public abstract class Ship
    {
        /// <summary>
        /// Gets the length of the ship.
        /// </summary>
        public int Length { get; protected set; }

        /// <summary>
        /// Gets the coordinates of the ship.
        /// </summary>
        public List<Coordinate> Coordinates { get; private set; } = new List<Coordinate>();

        /// <summary>
        /// Gets the coordinates for debugger display purposes.
        /// </summary>
        [JsonIgnore]
        public string CoordinatesDisplay => string.Join(", ", Coordinates.Select(x => $"{x.X}:{x.Y}"));

        /// <summary>
        /// Gets the value whether the ship has sunk.
        /// </summary>
        public bool HasSunk => Coordinates.All(x => x.IsHit);
    }
}
