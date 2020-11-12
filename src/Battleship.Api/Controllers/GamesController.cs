using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Battleship.Api.Models;
using Battleship.Core.DomainObjects;
using Battleship.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Battleship.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private const int MAX_GAME_COUNT = 10;
        private readonly IGameRegistry _gameRegistry;

        public GamesController(IGameRegistry gameRegistry)
        {
            _gameRegistry = gameRegistry;
        }

        /// <summary>
        /// Gets the list of game id.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IActionResult GetGamesIds()
        {
            var gameIds = _gameRegistry.GetGameIds();

            return Ok(gameIds);
        }

        /// <summary>
        /// Creates and starts a new game.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IActionResult NewGame()
        {
            // FOR HOSTING REASON: delete the game as soon as the player has won
            // to avoid the number of games from increasing
            if (_gameRegistry.GetGameIds().Count() >= MAX_GAME_COUNT)
            {
                return BadRequest("A maximum of 10 simulateneous games is supported. Delete some of the games.");
            }

            var game = _gameRegistry.StartNewGame();

            return Ok(game.Id);
        }

        /// <summary>
        /// Processes an attack.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/attack")]
        public IActionResult Attack([FromRoute, Required] Guid id, [Required] AttackRequest request)
        {
            var game = _gameRegistry.GetGame(id);

            if (game == null)
            {
                return NotFound("Game not found");
            }

            var attackResult = game.ProcessAttack(request.XCoordinate, request.YCoordinate);

            // FOR HOSTING REASON: delete the game as soon as the player has won
            // to avoid the number of games from increasing
            if (attackResult == AttackResult.Win)
            {
                _gameRegistry.DeleteGame(id);
            }

            return Ok(attackResult);
        }

        /// <summary>
        /// Deletes a game.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteGame([FromRoute, Required] Guid id)
        {
            _gameRegistry.DeleteGame(id);

            return NoContent();
        }

        /// <summary>
        /// For debugging purposes only.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/debug")]
        public IActionResult Debug([FromRoute, Required] Guid id)
        {
            var game = _gameRegistry.GetGame(id);

            if (game == null)
            {
                return NotFound("Game not found");
            }

            return Ok(game);
        }
    }
}
