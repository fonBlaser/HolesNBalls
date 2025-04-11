namespace HolesNBalls.Extensions;

public static class DirectionExtensions
{
    public static bool IsOpposite(this Direction direction, Direction? other)
        => other != null && other == direction.GetOpposite();

    public static Direction GetOpposite(this Direction direction)
        => direction switch
        {
            Direction.Top => Direction.Bottom,
            Direction.Bottom => Direction.Top,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
}