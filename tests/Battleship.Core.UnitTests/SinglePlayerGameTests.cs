using System;
using System.Collections.Generic;
using System.Linq;
using Battleship.Core.DomainObjects;
using Battleship.Core.DomainObjects.Games;
using Battleship.Core.DomainObjects.Ships;
using Battleship.Core.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Battleship.Core.UnitTests
{
    public class SinglePlayerGameTests
    {
        private const int BOARD_SIZE = 10;
        private readonly IReadOnlyList<Coordinate> _mockCoordinates;

        private readonly Mock<ICoordinateGenerator> _mockCoordinateGenerator;
        private readonly SinglePlayerGame _game;

        public SinglePlayerGameTests()
        {
            _mockCoordinates = Enumerable.Range(0, 5).Select(x => new Coordinate(0, x)).ToList();
            _mockCoordinateGenerator = new Mock<ICoordinateGenerator>();
            _mockCoordinateGenerator
                .Setup(x => x.GenerateCoordinates(BOARD_SIZE, It.IsAny<int>(), It.IsAny<Orientation>()))
                .Returns(_mockCoordinates);

            _game = new SinglePlayerGame(_mockCoordinateGenerator.Object, new Mock<ILogger<Game>>().Object);
        }

        [Fact]
        public void GivenAGame_WhenStarted_ThenGameIsInitialized()
        {
            _game.Start(new List<Type> { typeof(AircraftCarrier) }, BOARD_SIZE);

            _game.Status.Should().Be(GameStatus.Started);
            _game.Players.Should().HaveCount(1);
            
            var player = _game.Players.First();
            player.Ships.Should().HaveCount(1);
            player.Ships.First().Coordinates.Should().BeEquivalentTo(_mockCoordinates);
        }

        [Fact]
        public void GivenAStartedGame_WhenAllShipsSunk_ThenPlayerWins()
        {
            _game.Start(new List<Type> { typeof(AircraftCarrier) }, BOARD_SIZE);

            var playerToGuess = _game.Players.First();

            var totalMoves = playerToGuess.Ships.Sum(s => s.Length);
            int movesCounter = 0;
            foreach (var ship in playerToGuess.Ships)
            {
                foreach (var coordinate in ship.Coordinates)
                {
                    var attackResult = _game.ProcessAttack(coordinate.X, coordinate.Y);
                    movesCounter++;

                    if (movesCounter < totalMoves)
                    {
                        (attackResult == AttackResult.Hit || attackResult == AttackResult.Sunk).Should().BeTrue();
                    }
                    else
                    {
                        attackResult.Should().Be(AttackResult.Win);
                    }
                }
            }

            movesCounter.Should().Be(totalMoves);
            _game.Status.Should().Be(GameStatus.Ended);
        }

        [Theory]
        [InlineData(1, 0, AttackResult.Miss)]
        [InlineData(11, 1, AttackResult.InvalidCoordinate)]
        [InlineData(1, 1, AttackResult.CoordinateAlreadyAttacked)]
        public void GivenAPlayer_WhenAttacked_ThenReturnAttackResult(int x, int y, AttackResult expectedAttackResult)
        {
            _game.Start(new List<Type> { typeof(AircraftCarrier) }, BOARD_SIZE);           
            _game.ProcessAttack(1, 1); // to simulate a coordinate that's already been attacked

            var attackResult = _game.ProcessAttack(x, y);

            attackResult.Should().Be(expectedAttackResult);
        }
    }
}
