using System;
using System.Collections.Generic;
using System.Text;
using Battleship.Core.DomainObjects;
using Battleship.Core.DomainObjects.Ships;
using FluentAssertions;
using Xunit;

namespace Battleship.Core.UnitTests
{
    public class PlayerTests
    {
        private static readonly List<Coordinate> _mockCoordinates = new List<Coordinate>
        {
            new Coordinate(0, 0),
            new Coordinate(0, 1),
            new Coordinate(0, 2)
        };
        private readonly Player _player;

        public PlayerTests()
        {
            var ship = new Submarine();
            ship.Coordinates.AddRange(_mockCoordinates);

            _player = new Player("Computer");
            _player.AddShips(ship);
            _player.ProcessAttack(1, 1);
        }

        [Fact]
        public void GivenAPlayer_WhenCoordinatesAreGenerate_ThenShipsArePlacedInTheCoordinates()
        {
            for (int i = 0; i < _mockCoordinates.Count; i++)
            {
                _player.Ships[0].Coordinates[i].Should().Be(_mockCoordinates[i]);
            }
        }

        [Theory]
        [InlineData(0, 1, AttackResult.Hit)]
        [InlineData(1, 0, AttackResult.Miss)]
        [InlineData(1, 1, AttackResult.CoordinateAlreadyAttacked)]
        public void GivenAPlayer_WhenAttacked_ThenReturnAttackResult(int x, int y, AttackResult expectedAttackResult)
        {
            var actualAttackResult = _player.ProcessAttack(x, y);

            actualAttackResult.Should().Be(expectedAttackResult);
        }
    }
}
