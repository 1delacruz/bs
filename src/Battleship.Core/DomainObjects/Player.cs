using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;

namespace Battleship.Core.DomainObjects
{
    /// <summary>
    /// Represents a player of the game.
    /// </summary>
    public class Player
    {
        private readonly List<Ship> _ships = new List<Ship>();
        private readonly List<Coordinate> _attackedCoordinates = new List<Coordinate>();

        public Player(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        /// <summary>
        /// Gets the unique identifier of the player.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the name of the player;
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the ships.
        /// </summary>
        public IReadOnlyList<Ship> Ships => _ships;

        /// <summary>
        /// Gets the coordinates attacked by the opponent.
        /// </summary>
        public IReadOnlyList<Coordinate> AttackedCoordinates => _attackedCoordinates;

        /// <summary>
        /// Adds a ship.
        /// </summary>
        /// <param name="ships"></param>
        public void AddShips(params Ship[] ships)
        {
            Guard.Against.NullOrEmpty(ships, nameof(ships));

            _ships.AddRange(ships);
        }

        /// <summary>
        /// Processes an attack.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public AttackResult ProcessAttack(int x, int y)
        {
            if (_attackedCoordinates.Any(c => c.X == x && c.Y == y))
            {
                return AttackResult.CoordinateAlreadyAttacked;
            }

            AttackResult attackResult = AttackResult.Miss;
            foreach (var ship in Ships)
            {
                var coordinate = ship.Coordinates.SingleOrDefault(c => c.X == x && c.Y == y);

                if (coordinate != null)
                {
                    coordinate.IsHit = true;
                    attackResult = ship.HasSunk ? AttackResult.Sunk : AttackResult.Hit;
                    break;
                }
            }

            _attackedCoordinates.Add(new Coordinate(x, y));

            return attackResult;
        }
    }
}
