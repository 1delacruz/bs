using System;
using System.Collections.Generic;

namespace Battleship.Core.DomainObjects
{
    public interface IGame
    {
        IReadOnlyList<Type> AllowedShips { get; }
        int BoardSize { get; }
        Guid Id { get; }
        IReadOnlyList<Player> Players { get; }
        GameStatus Status { get; }

        AttackResult ProcessAttack(int xCoordinate, int yCoordinate);
        void Start(IReadOnlyList<Type> allowedShips = null, int boardSize = 10);
    }
}