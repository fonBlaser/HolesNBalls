﻿namespace HolesNBalls;

public record Board
{
    public required int Width { get; init; }
    public required int Height { get; init; }
    
    public required IReadOnlyCollection<Hole> Holes { get; init; }
    public required IReadOnlyCollection<Ball> Balls { get; init; }

    public IReadOnlyCollection<Drop> Drops { get; init; } = [];

    /// <summary>
    /// Turns board to top and moves balls to right direction.
    /// If ball meets a hole, the Drop is added to TurnDrops.
    /// </summary>
    /// <returns>New instance of the board with the updated drops and bal positions.</returns>
    public Board Move(MoveDirection direction)
    {
        List<Drop> currentDrops = new();
        List<Ball> missedBalls = new();

        Func<BoardObject, int> moveAxisSelector = GetMoveAxisSelector(direction);
        Func<BoardObject, int> nonMoveAxisSelector = GetNonMoveAxisSelector(direction);

        IGrouping<int, Ball>[] ballLines = GroupByAxis<Ball>(Balls, nonMoveAxisSelector);
        IGrouping<int, Hole>[] holeLines = GroupByAxis<Hole>(Holes, nonMoveAxisSelector);

        Func<Ball, Hole, bool> directionMatches = GetDirectionMatchFilter(direction);
        Func<BoardObject, BoardObject, int> calculateDistance = GetDistanceCalculator(direction);

        foreach (int line in ballLines.Select(l => l.Key))
        {
            List<Ball> lineMissedBalls = new();
            Ball[] ballLine = GetLine<Ball>(ballLines, line, moveAxisSelector);
            Hole[] holeLine = GetLine<Hole>(holeLines, line, moveAxisSelector);

            foreach (Ball ball in ballLine)
            {
                Hole? nearestHole = null;
                int nearestDistance = 0;

                foreach (Hole hole in holeLine)
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
                int moveAxisLength = GetMoveAxisLength(direction);
                int stackLength = lineMissedBalls.Count;
                bool isDirectionToAxisEnd = IsDirectionToAxisEnd(direction);
                int offset = isDirectionToAxisEnd ? moveAxisLength - stackLength : 0;

                Func<Ball, int, Ball> moveAxisUpdater = GetMoveAxisUpdater(direction, offset);

                lineMissedBalls = lineMissedBalls.OrderBy(moveAxisSelector)
                                                 .Cast<Ball>()
                                                 .Select(moveAxisUpdater).ToList();

                missedBalls.AddRange(lineMissedBalls);
            }
        }

        return this with
        {
            Balls = missedBalls,
            Drops = Drops.Union(currentDrops).ToArray(),
        };
    }

    private int GetMoveAxisLength(MoveDirection direction)
        => direction switch
        {
            MoveDirection.Top => Height,
            MoveDirection.Bottom => Height,
            MoveDirection.Left => Width,
            MoveDirection.Right => Width,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private bool IsDirectionToAxisEnd(MoveDirection direction)
        => direction switch
        {
            MoveDirection.Top => false,
            MoveDirection.Bottom => true,
            MoveDirection.Left => false,
            MoveDirection.Right => true,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static IGrouping<int, TObject>[] GroupByAxis<TObject>(IEnumerable<TObject> collection, Func<TObject, int> axisSelector) 
        where TObject : BoardObject
        => collection.GroupBy(axisSelector)
            .OrderBy(g => g.Key)
            .ToArray();

    private static Func<BoardObject, int> GetMoveAxisSelector(MoveDirection direction)
        => direction switch
        {
            MoveDirection.Top => obj => obj.Y,
            MoveDirection.Bottom => obj => obj.Y,
            MoveDirection.Left => obj => obj.X,
            MoveDirection.Right => obj => obj.X,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static Func<BoardObject, int> GetNonMoveAxisSelector(MoveDirection direction)
        => direction switch
        {
            MoveDirection.Top => obj => obj.X,
            MoveDirection.Bottom => obj => obj.X,
            MoveDirection.Left => obj => obj.Y,
            MoveDirection.Right => obj => obj.Y,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static Func<Ball, int, Ball> GetMoveAxisUpdater(MoveDirection direction, int offset)
        => direction switch
        {
            MoveDirection.Top => (ball, index) => ball with { Y = index + offset },
            MoveDirection.Bottom => (ball, index) => ball with { Y = index + offset },
            MoveDirection.Left => (ball, index) => ball with { X = index + offset },
            MoveDirection.Right => (ball, index) => ball with { X = index + offset },
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static Func<BoardObject, BoardObject, bool> GetDirectionMatchFilter(MoveDirection direction)
        => direction switch
        {
            MoveDirection.Top => (from, to) => from.X == to.X && from.Y > to.Y,
            MoveDirection.Bottom => (from, to) => from.X == to.X && from.Y < to.Y,
            MoveDirection.Left => (from, to) => from.Y == to.Y && from.X > to.X,
            MoveDirection.Right => (from, to) => from.Y == to.Y && from.X < to.X,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static Func<BoardObject, BoardObject, int> GetDistanceCalculator(MoveDirection direction)
        => direction switch
        {
            MoveDirection.Top => (from, to) => from.Y - to.Y,
            MoveDirection.Bottom => (from, to) => to.Y - from.Y,
            MoveDirection.Left => (from, to) => from.X - to.X,
            MoveDirection.Right => (from, to) => to.X - from.X,
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