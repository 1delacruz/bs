using System;
using System.Collections.Generic;
using Battleship.Core.DomainObjects;

namespace Battleship.Core.Services
{
    public interface IGameRegistry
    {
        /// <summary>
        /// Deletes a game.
        /// </summary>
        /// <param name="id"></param>
        void DeleteGame(Guid id);

        /// <summary>
        /// Creates and starts a new game.
        /// </summary>
        /// <returns></returns>
        IGame StartNewGame();

        /// <summary>
        /// Gets a game by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IGame GetGame(Guid id);

        /// <summary>
        /// Gets the list of game id.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Guid> GetGameIds();
    }
}