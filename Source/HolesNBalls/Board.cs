using HolesNBalls.Extensions;

namespace HolesNBalls;

/// <summary>
/// Represents the game board.
/// </summary>
public record Board
{
    private Dictionary<Direction, bool> _ballsAlignment = [];

    public required int Width { get; init; }
    public required int Height { get; init; }
    
    public required IReadOnlyCollection<Hole> Holes { get; init; }
    public required IReadOnlyCollection<Ball> Balls { get; init; }

    public IReadOnlyCollection<Drop> Drops { get; init; } = [];

    /// <summary>
    /// Turns board and moves balls to right direction.
    /// If ball meets a hole, the Drop is added to Drops.
    /// </summary>
    /// <returns>New instance of the board with the updated drops and bal positions.</returns>
    public Board Move(Direction direction)
    {
        List<Drop> currentDrops = new();
        List<Ball> missedBalls = new();

        Func<BoardObject, int> moveAxisSelector = GetMoveAxisSelector(direction);
        Func<BoardObject, int> nonMoveAxisSelector = GetNonMoveAxisSelector(direction);

        IGrouping<int, Ball>[] ballLines = GroupByAxis<Ball>(Balls, nonMoveAxisSelector);
        IGrouping<int, Hole>[] holeLines = GroupByAxis<Hole>(Holes, nonMoveAxisSelector);

        foreach (int line in ballLines.Select(l => l.Key))
        {
            List<Ball> lineMissedBalls = new();
            Ball[] ballLine = GetLine<Ball>(ballLines, line, moveAxisSelector);
            Hole[] holeLine = GetLine<Hole>(holeLines, line, moveAxisSelector);

            foreach (Ball ball in ballLine)
            {
                Hole? nearestHole = GetNearestHoleForBallByDirection(ball, holeLine, direction);
                
                if (nearestHole == null)
                {
                    lineMissedBalls.Add(ball);
                }
                else
                {
                    currentDrops.Add(new Drop() { Ball = ball, Hole = nearestHole });
                }
            }

            //Update missed balls positions
            if (lineMissedBalls.Any())
            {
                lineMissedBalls = UpdateBallLinePositions(direction, lineMissedBalls, moveAxisSelector);
                missedBalls.AddRange(lineMissedBalls);
            }
        }

        Dictionary<Direction, bool> updatedAlignments = new Dictionary<Direction, bool>(_ballsAlignment)
            {
                [direction] = true,
                [direction.GetOpposite()] = false
            };

        return this with
        {
            Balls = missedBalls,
            Drops = Drops.Union(currentDrops).ToArray(),
            _ballsAlignment = updatedAlignments
        };
    }

    public bool AreBallsAlignedTo(Direction direction)
    {
        if(_ballsAlignment.TryGetValue(direction, out bool aligned))
            return aligned;

        aligned = true;

        Func<BoardObject, int> moveAxisSelector = GetMoveAxisSelector(direction);
        Func<BoardObject, int> nonMoveAxisSelector = GetNonMoveAxisSelector(direction);
        

        IGrouping<int, Ball>[] ballLines = GroupByAxis<Ball>(Balls, nonMoveAxisSelector);

        foreach (int line in ballLines.Select(l => l.Key))
        {
            Ball[] ballLine = GetLine<Ball>(ballLines, line, moveAxisSelector);

            if (ballLine.Length == 0)
                continue;

            int stackLength = ballLine.Length;
            int moveAxisLength = GetMoveAxisLength(direction);

            aligned = IsDirectionToAxisEnd(direction)
                    ? moveAxisSelector(ballLine.First()) == moveAxisLength - stackLength
                    : moveAxisSelector(ballLine.Last()) == stackLength - 1;

            if (!aligned)
                break;
        }

        _ballsAlignment.Add(direction, aligned);

        return aligned;
    }

    private List<Ball> UpdateBallLinePositions(Direction direction, List<Ball> lineMissedBalls, Func<BoardObject, int> moveAxisSelector)
    {
        int moveAxisLength = GetMoveAxisLength(direction);
        int stackLength = lineMissedBalls.Count;
        bool isDirectionToAxisEnd = IsDirectionToAxisEnd(direction);
        int offset = isDirectionToAxisEnd ? moveAxisLength - stackLength : 0;

        Func<Ball, int, Ball> moveAxisUpdater = GetMoveAxisUpdater(direction, offset);

        return lineMissedBalls.OrderBy(moveAxisSelector)
                              .Cast<Ball>()
                              .Select(moveAxisUpdater)
                              .ToList();
    }

    private Hole? GetNearestHoleForBallByDirection(Ball ball, Hole[] holes, Direction direction)
    {
        if(!holes.Any())
            return null;

        Func<Ball, Hole, bool> directionMatches = GetDirectionMatchFilter(direction);
        Func<BoardObject, BoardObject, int> calculateDistance = GetDistanceCalculator(direction);

        Hole? nearestHole = null;
        int nearestDistance = 0;

        foreach (Hole hole in holes)
        {
            if (directionMatches(ball, hole))
            {
                int currentDistance = calculateDistance(ball, hole);
                if (nearestHole == null || currentDistance < nearestDistance)
                {
                    nearestHole = hole;
                    nearestDistance = currentDistance;
                }
            }
        }

        return nearestHole;
    }

    private int GetMoveAxisLength(Direction direction)
        => direction switch
        {
            Direction.Top => Height,
            Direction.Bottom => Height,
            Direction.Left => Width,
            Direction.Right => Width,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private bool IsDirectionToAxisEnd(Direction direction)
        => direction switch
        {
            Direction.Top => false,
            Direction.Bottom => true,
            Direction.Left => false,
            Direction.Right => true,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static IGrouping<int, TObject>[] GroupByAxis<TObject>(IEnumerable<TObject> collection, Func<TObject, int> axisSelector) 
        where TObject : BoardObject
        => collection.GroupBy(axisSelector)
            .OrderBy(g => g.Key)
            .ToArray();

    private static Func<BoardObject, int> GetMoveAxisSelector(Direction direction)
        => direction switch
        {
            Direction.Top => obj => obj.Y,
            Direction.Bottom => obj => obj.Y,
            Direction.Left => obj => obj.X,
            Direction.Right => obj => obj.X,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static Func<BoardObject, int> GetNonMoveAxisSelector(Direction direction)
        => direction switch
        {
            Direction.Top => obj => obj.X,
            Direction.Bottom => obj => obj.X,
            Direction.Left => obj => obj.Y,
            Direction.Right => obj => obj.Y,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static Func<Ball, int, Ball> GetMoveAxisUpdater(Direction direction, int offset)
        => direction switch
        {
            Direction.Top => (ball, index) => ball with { Y = index + offset },
            Direction.Bottom => (ball, index) => ball with { Y = index + offset },
            Direction.Left => (ball, index) => ball with { X = index + offset },
            Direction.Right => (ball, index) => ball with { X = index + offset },
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static Func<BoardObject, BoardObject, bool> GetDirectionMatchFilter(Direction direction)
        => direction switch
        {
            Direction.Top => (from, to) => from.X == to.X && from.Y > to.Y,
            Direction.Bottom => (from, to) => from.X == to.X && from.Y < to.Y,
            Direction.Left => (from, to) => from.Y == to.Y && from.X > to.X,
            Direction.Right => (from, to) => from.Y == to.Y && from.X < to.X,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static Func<BoardObject, BoardObject, int> GetDistanceCalculator(Direction direction)
        => direction switch
        {
            Direction.Top => (from, to) => from.Y - to.Y,
            Direction.Bottom => (from, to) => to.Y - from.Y,
            Direction.Left => (from, to) => from.X - to.X,
            Direction.Right => (from, to) => to.X - from.X,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    

    private static TObject[] GetLine<TObject>(IGrouping<int, TObject>[] grouping, int line, Func<TObject, int> orderSelector)
        where TObject : BoardObject
    {
        IGrouping<int, TObject>? lineObjects = grouping.FirstOrDefault(g => g.Key == line);

        if (lineObjects == null)
            return Array.Empty<TObject>();

        return lineObjects.OrderBy(orderSelector).ToArray();
    }
}