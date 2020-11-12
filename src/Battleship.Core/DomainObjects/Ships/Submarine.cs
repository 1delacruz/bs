namespace Battleship.Core.DomainObjects.Ships
{
    public class Submarine : Ship
    {
        public Submarine()
        {
            Length = 3;
        }

        public override string ToString()
        {
            return "S";
        }
    }
}
