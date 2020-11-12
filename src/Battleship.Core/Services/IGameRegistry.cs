using System;
using System.Collections.Generic;
using Battleship.Core.DomainObjects;

namespace Battleship.Core.Services
{
    public interface IGameRegistry
    {
        void DeleteGame(Guid id);

        IGame StartNewGame();

        IGame GetGame(Guid id);

        IEnumerable<Guid> GetGameIds();
    }
}