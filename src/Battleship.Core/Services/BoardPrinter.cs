using System.Diagnostics;
using System.Linq;
using Battleship.Core.DomainObjects;

namespace Battleship.Core.Services
{
    public static class BoardPrinter
    {
        [Conditional("DEBUG")]
        public static void PrintBoard(Game game)
        {
            int boardSize = game.BoardSize;

            foreach (var player in game.Players)
            {
                Debug.WriteLine($"Name: {player.Name}");

                for (int y = 0; y < boardSize; y++)
                {
                    string line = "";
                    for (int x = 0; x < boardSize; x++)
                    {
                        var ship = player.Ships.SingleOrDefault(s => s.Coordinates.Any(c => c.Y == y && c.X == x));
                        if (ship != null)
                        {
                            line += $" {ship} ";
                        }
                        else
                        {
                            line += " O ";
                        }
                    }

                    Debug.WriteLine(line);
                }
            }
        }
    }
}
