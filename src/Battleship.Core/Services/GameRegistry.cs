using System;
using System.Collections.Generic;
using Battleship.Core.DomainObjects;

namespace Battleship.Core.Services
{
    public class GameRegistry : IGameRegistry
    {
        private readonly IDictionary<Guid, IGame> _games = new Dictionary<Guid, IGame>();
        private readonly IServiceProvider _serviceProvider;

        public GameRegistry(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IGame StartNewGame()
        {
            var game = _serviceProvider.GetService(typeof(IGame)) as IGame;
            game.Start();
            _games.Add(game.Id, game);

            return game;
        }

        public IGame GetGame(Guid id)
        {
            _games.TryGetValue(id, out IGame game);

            return game;
        }

        public void DeleteGame(Guid id)
        {
            _games.Remove(id);
        }

        public IEnumerable<Guid> GetGameIds()
        {
            return _games.Keys;
        }
    }
}
