using System.Collections.Generic;
using Battleship.Core.Services;
using Microsoft.Extensions.Logging;

namespace Battleship.Core.DomainObjects.Games
{
    /// <summary>
    /// Represents a single-player mode game
    /// </summary>
    public class SinglePlayerGame : Game
    {
        public SinglePlayerGame(ICoordinateGenerator coordinateGenerator, ILogger<Game> logger) : base(coordinateGenerator, logger)
        {
        }

        protected override IEnumerable<Player> GetPlayers()
        {
            var player = new Player("Computer");
            yield return player;
        }
    }
}
