namespace HolesNBalls;

public record Board
{
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required IReadOnlyCollection<Hole> Holes { get; init; }
    public required IReadOnlyCollection<Ball> Balls { get; init; }
}