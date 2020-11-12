using System;
using AutoFixture.Xunit2;
using Battleship.Api.Controllers;
using Battleship.Api.Models;
using Battleship.Core.DomainObjects;
using Battleship.Core.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Battleship.Api.UnitTests
{
    public class GameControllerTests
    {
        private readonly GamesController _controller;
        private readonly Mock<IGameRegistry> _mockGameRegistry;
        private readonly Mock<IGame> _mockGame;

        public GameControllerTests()
        {
            _mockGameRegistry = new Mock<IGameRegistry>();
            _mockGame = new Mock<IGame>();
            _controller = new GamesController(_mockGameRegistry.Object);
        }

        [Fact]
        public void GivenANewGameRequest_WhenReceived_ThenNewGameStarted()
        {
            var game = _mockGame.Object;
            _mockGameRegistry
                .Setup(x => x.StartNewGame())
                .Returns(game);

            var result = _controller.NewGame() as OkObjectResult;

            result.Should().NotBeNull();
            _mockGameRegistry.Verify(x => x.StartNewGame(), Times.Once);
        }

        [Fact]
        public void GivenAGame_WhenAttacked_ThenReturnResult()
        {
            var game = _mockGame.Object;
            _mockGameRegistry
                .Setup(x => x.GetGame(game.Id))
                .Returns(game);

            var request = new AttackRequest
            {
                XCoordinate = 1,
                YCoordinate = 0
            };
            var result = _controller.Attack(game.Id, request) as OkObjectResult;

            result.Should().NotBeNull();
            _mockGameRegistry.Verify(x => x.GetGame(game.Id), Times.Once);
        }

        [Theory]
        [AutoData]
        public void GivenANonExistingGame_WhenAttacked_ThenReturnNotFound(Guid gameId)
        {
            var request = new AttackRequest
            {
                XCoordinate = 1,
                YCoordinate = 0
            };
            var result = _controller.Attack(gameId, request) as NotFoundObjectResult;

            result.Should().NotBeNull();
            _mockGameRegistry.Verify(x => x.GetGame(gameId), Times.Once);
        }

        [Fact]
        public void GivenAGame_WhenDeleteRequestIsReceived_ThenTheGameIsDeleted()
        {
            var game = _mockGame.Object;
            _mockGameRegistry
                .Setup(x => x.GetGame(game.Id))
                .Returns(game);

            var result = _controller.DeleteGame(game.Id) as NoContentResult;

            result.Should().NotBeNull();
            _mockGameRegistry.Verify(x => x.DeleteGame(game.Id), Times.Once);
        }
    }
}
