namespace HolesNBalls;

public abstract record BoardObject
{
    public required int Number { get; init; }
    public required int X { get; init; }
    public required int Y { get; init; }
}