namespace Battleship.Core.DomainObjects.Ships
{
    public class AircraftCarrier : Ship
    {
        public AircraftCarrier()
        {
            Length = 5;
        }

        public override string ToString()
        {
            return "A";
        }
    }
}
