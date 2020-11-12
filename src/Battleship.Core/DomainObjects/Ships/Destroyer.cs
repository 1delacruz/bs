namespace Battleship.Core.DomainObjects.Ships
{
    public class Destroyer : Ship
    {
        public Destroyer()
        {
            Length = 2;
        }

        public override string ToString()
        {
            return "D";
        }
    }
}
