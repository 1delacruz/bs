namespace Battleship.Core.DomainObjects.Ships
{
    public class Battleship : Ship
    {
        public Battleship()
        {
            Length = 4;
        }

        public override string ToString()
        {
            return "B";
        }
    }
}
