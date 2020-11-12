using System.Collections.Generic;
using Battleship.Core.DomainObjects;

namespace Battleship.Core.Services
{
    public interface ICoordinateGenerator
    {
        /// <summary>
        /// Randomly generates the coordinates. 
        /// </summary>
        /// <param name="boardSize">The size of the board</param>
        /// <param name="shipLength">The length of the ship.</param>
        /// <param name="orientation">The orientation of the ship.</param>
        /// <returns></returns>
        IReadOnlyList<Coordinate> GenerateCoordinates(int boardSize, int shipLength, Orientation orientation);
    }
}
