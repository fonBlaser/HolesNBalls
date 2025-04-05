namespace HolesNBalls;

public record Board
{
    public required int Width { get; init; }
    public required int Height { get; init; }
    
    public required IReadOnlyCollection<Hole> Holes { get; init; }
    public required IReadOnlyCollection<Ball> Balls { get; init; }
    
    public IReadOnlyDictionary<int, Drop> TurnDrops { get; init; } = new Dictionary<int, Drop>();
    public IReadOnlyCollection<Drop> Drops => TurnDrops.OrderBy(d => d.Key)
                                                       .Select(d => d.Value)
                                                       .ToList();

    /// <summary>
    /// Turns board to top and moves balls to right direction.
    /// If ball meets a hole, the Drop is added to TurnDrops.
    /// </summary>
    /// <returns>New instance of the board with the updated drops and bal positions.</returns>
    public Board MoveTop()
    {
        return this;
    }
}