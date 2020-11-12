using System;
using System.Collections.Generic;
using Battleship.Core.DomainObjects;

namespace Battleship.Core.Services
{
    /// <summary>
    /// Represents the registration of games
    /// </summary>
    public class GameRegistry : IGameRegistry
    {
        private readonly IDictionary<Guid, IGame> _games = new Dictionary<Guid, IGame>();
        private readonly IServiceProvider _serviceProvider;

        public GameRegistry(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Creates and starts a new game.
        /// </summary>
        /// <returns></returns>
        public IGame StartNewGame()
        {
            var game = _serviceProvider.GetService(typeof(IGame)) as IGame;
            game.Start();
            _games.Add(game.Id, game);

            return game;
        }

        /// <summary>
        /// Gets a game by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IGame GetGame(Guid id)
        {
            _games.TryGetValue(id, out IGame game);

            return game;
        }

        /// <summary>
        /// Deletes a game.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteGame(Guid id)
        {
            _games.Remove(id);
        }

        /// <summary>
        /// Gets the list of game id.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Guid> GetGameIds()
        {
            return _games.Keys;
        }
    }
}
