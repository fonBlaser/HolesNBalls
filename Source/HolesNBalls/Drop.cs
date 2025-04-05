namespace HolesNBalls;

public record Drop
{
    public required  Hole Hole { get; init; }
    public required Ball Ball { get; init; }
}