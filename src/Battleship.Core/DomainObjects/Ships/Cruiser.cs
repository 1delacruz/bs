namespace Battleship.Core.DomainObjects.Ships
{
    public class Cruiser : Ship
    {
        public Cruiser()
        {
            Length = 3;
        }

        public override string ToString()
        {
            return "C";
        }
    }
}
