namespace Battleship.Core.DomainObjects
{
    /// <summary>
    /// A result of an attack.
    /// </summary>
    public enum AttackResult
    {
        InvalidCoordinate,
        Hit,
        Miss,
        Sunk,
        Win,
        CoordinateAlreadyAttacked
    }
}
