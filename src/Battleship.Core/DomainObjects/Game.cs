using System;
using System.Collections.Generic;
using System.Linq;
using Battleship.Core.DomainObjects.Ships;
using Battleship.Core.Services;
using Microsoft.Extensions.Logging;

namespace Battleship.Core.DomainObjects
{
    /// <summary>
    /// Represents the battleship game.
    /// </summary>
    public abstract class Game : IGame
    {
        private readonly List<Player> _players = new List<Player>();
        private readonly ICoordinateGenerator _coordinateGenerator;
        private readonly ILogger<Game> _logger;

        protected Game(ICoordinateGenerator coordinateGenerator, ILogger<Game> logger)
        {
            _coordinateGenerator = coordinateGenerator;
            _logger = logger;

            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets the unique identifier of the game.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the allowed ships.
        /// </summary>
        public IReadOnlyList<Type> AllowedShips { get; private set; }

        /// <summary>
        /// Gets the board size.
        /// </summary>
        public int BoardSize { get; private set; }

        /// <summary>
        /// Gets the game status.
        /// </summary>
        public GameStatus Status { get; private set; }

        /// <summary>
        /// Gets the players;
        /// </summary>
        public IReadOnlyList<Player> Players => _players;

        /// <summary>
        /// Starts the game
        /// </summary>
        /// <param name="allowedShips"></param>
        /// <param name="boardSize"></param>
        public void Start(IReadOnlyList<Type> allowedShips = null, int boardSize = 10)
        {
            AllowedShips = allowedShips ?? new List<Type>
            {
                typeof(AircraftCarrier),
                typeof(Ships.Battleship),
                typeof(Submarine),
                typeof(Cruiser),
                typeof(Destroyer)
            };
            BoardSize = boardSize;

            // add player
            var players = GetPlayers();
            _players.AddRange(players);
            _players.ForEach(p => PlaceShips(p));

            Status = GameStatus.Started;

            BoardPrinter.PrintBoard(this);
        }

        /// <summary>
        /// Processes an attack.
        /// </summary>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        /// <returns></returns>
        public AttackResult ProcessAttack(int xCoordinate, int yCoordinate)
        {
            if (Status == GameStatus.Ended)
            {
                throw new Exception("The game has ended");
            }

            if (xCoordinate < 0 || yCoordinate < 0 || xCoordinate >= BoardSize || yCoordinate >= BoardSize)
            {
                return AttackResult.InvalidCoordinate;
            }

            var playerToGuess = _players.First();
            AttackResult attackResult = playerToGuess.ProcessAttack(xCoordinate, yCoordinate);

            if (playerToGuess.Ships.All(s => s.HasSunk))
            {
                attackResult = AttackResult.Win;
                Status = GameStatus.Ended;
            }

            return attackResult;
        }

        /// <summary>
        /// Places the ships in random locations.
        /// </summary>
        /// <param name="player"></param>
        private void PlaceShips(Player player)
        {
            var random = new Random((int)DateTime.Now.Ticks);

            foreach (var shipType in AllowedShips)
            {
                var ship = Activator.CreateInstance(shipType) as Ship;
                IReadOnlyList<Coordinate> coordinates;
                bool retry;

                do
                {
                    var orientation = (Orientation)random.Next(0, 2);
                    // the coordinates will be regenerated if could go out of bounds or has a collision to an already placed ship
                    coordinates = _coordinateGenerator.GenerateCoordinates(BoardSize, ship.Length, orientation);
                    retry = coordinates == null || HasCollision(player.Ships, coordinates);
                }
                while (retry);

                ship.Coordinates.AddRange(coordinates);
                player.AddShips(ship);
            }
        }

        protected abstract IEnumerable<Player> GetPlayers();

        private bool HasCollision(IEnumerable<Ship> playerShips, IEnumerable<Coordinate> proposedCoordinates)
        {
            var hasCollission = proposedCoordinates.Any(pc => playerShips.Any(s => s.Coordinates.Any(c => c.Equals(pc))));

            if (hasCollission)
            {
                _logger.LogInformation("Collision detected");
            }

            return hasCollission;
        }
    }
}
