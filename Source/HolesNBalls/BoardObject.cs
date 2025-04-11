namespace HolesNBalls;

public abstract record BoardObject : Coordinates
{
    public required int Number { get; init; }
}