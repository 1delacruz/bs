using System;
using System.Collections.Generic;
using Battleship.Core.DomainObjects;
using Microsoft.Extensions.Logging;

namespace Battleship.Core.Services
{
    /// <summary>
    /// Represent the a class that generates the coordinates
    /// </summary>
    public class CoordinateGenerator : ICoordinateGenerator
    {
        private readonly ILogger<CoordinateGenerator> _logger;

        public CoordinateGenerator(ILogger<CoordinateGenerator> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Randomly generates the coordinates. 
        /// </summary>
        /// <param name="boardSize">The size of the board</param>
        /// <param name="shipLength">The length of the ship.</param>
        /// <param name="orientation">The orientation of the ship.</param>
        /// <returns></returns>
        public IReadOnlyList<Coordinate> GenerateCoordinates(int boardSize, int shipLength, Orientation orientation)
        {
            var random = new Random((int)DateTime.Now.Ticks);

            var coordinates = new List<Coordinate>(shipLength);
            int startX = random.Next(1, boardSize);
            int startY = random.Next(1, boardSize);
            int variableIndex;
            int fixedIndex;

            if (orientation == Orientation.Horizontal)
            {
                variableIndex = startX;
                fixedIndex = startY;
            }
            else
            {
                variableIndex = startY;
                fixedIndex = startX;
            }

            // check if the ship could potentially go out of bounds by going downwards or to the right, depending on the orientation.
            // if it will, then try placing the ship upwards or to the left.
            if ((variableIndex + shipLength) >= boardSize)
            {
                var lowerBound = variableIndex - shipLength;

                if (lowerBound < 0)
                {
                    _logger.LogError("The ship will be out of bounds");
                    return null;
                }

                for (int i = variableIndex; i > lowerBound; i--)
                {
                    Coordinate coordinate;
                    if (orientation == Orientation.Horizontal)
                    {
                        coordinate = new Coordinate(i, fixedIndex);
                    }
                    else
                    {
                        coordinate = new Coordinate(fixedIndex, i);
                    }

                    coordinates.Add(coordinate);
                }
            }
            else
            {
                var upperBound = variableIndex + shipLength;

                if (upperBound > boardSize)
                {
                    _logger.LogError("The ship will be out of bounds");
                    return null;
                }

                for (int i = variableIndex; i < upperBound; i++)
                {
                    Coordinate coordinate;
                    if (orientation == Orientation.Horizontal)
                    {
                        coordinate = new Coordinate(i, fixedIndex);
                    }
                    else
                    {
                        coordinate = new Coordinate(fixedIndex, i);
                    }

                    coordinates.Add(coordinate);
                }
            }

            return coordinates;
        }

    }
}
