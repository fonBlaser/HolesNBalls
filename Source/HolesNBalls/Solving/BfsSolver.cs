using HolesNBalls.Extensions;
using HolesNBalls.Validation;

namespace HolesNBalls.Solving;

/// <summary>
/// Solves the board using the Breadth-first search algorithm.
/// </summary>
public class BfsSolver
{
    public Turn InitialTurn { get; }

    public BfsSolver(Board board)
    {
        new BoardValidator().ValidateBoard(board);
        InitialTurn = CreateTurn(board, direction: null, previousTurn: null);
    }

    /// <summary>
    /// Solves the board using the specified mode.
    /// <see cref="BfsSolutionMode">Mode</see> affects the search interruption and results.
    /// </summary>
    /// <returns>The turns matching the solution mode.</returns>
    public List<Turn> Solve(BfsSolutionMode mode = BfsSolutionMode.FirstWin)
    {
        List<Turn> topTurns = [InitialTurn];

        List<Turn> result = new();
        if (ShouldAddResult(InitialTurn.State, mode))
            result.Add(InitialTurn);

        bool interruptSolution = false;
        while (!interruptSolution)
        {
            Turn[] baseTurns = topTurns.Where(t => t.State == BoardState.Solvable).ToArray();
            topTurns.Clear();

            if (baseTurns.Length == 0)
                interruptSolution = true;

            bool interruptDepth = false;

            foreach (Turn baseTurn in baseTurns)
            {
                Direction[] effectiveDirections = GetEffectiveDirections(baseTurn);

                foreach (Direction direction in effectiveDirections)
                {
                    Board nextBoard = baseTurn.Board.Move(direction);
                    Turn turn = CreateTurn(nextBoard, direction, baseTurn);

                    topTurns.Add(turn);

                    if (ShouldAddResult(turn.State, mode))
                        result.Add(turn);

                    if (ShouldInterruptDepth(turn.State, mode))
                        interruptDepth = true;

                    if (ShouldInterruptSolution(turn.State, mode))
                        interruptSolution = true;

                    if (interruptDepth)
                        break;
                }

                if (interruptDepth)
                    break;
            }
        }

        return result;
    }

    private Turn CreateTurn(Board board, Direction? direction, Turn? previousTurn)
    {
        if (previousTurn != null && direction == null)
            throw new ArgumentException("Direction cannot be null if previousTurn is not null.");

        BoardState state = GetBoardState(board);
        int turnNumber = previousTurn == null ? 0 : previousTurn.Number + 1;

        return new Turn(turnNumber, board, state, direction, previousTurn);
    }

    private BoardState GetBoardState(Board board)
    {
        if (board.Drops.Any(d => d.Hole.Number != d.Ball.Number))
            return BoardState.Lost;

        if (!board.Balls.Any())
            return BoardState.Win;

        bool isBoardSolvable = GetIsBoardSolvable(board);

        return isBoardSolvable
             ? BoardState.Solvable 
             : BoardState.Unsolvable;
    }

    private bool GetIsBoardSolvable(Board board)
    {
        int[] holeXs = board.Holes.Select(h => h.X).Distinct().ToArray();
        int[] holeYs = board.Holes.Select(h => h.Y).Distinct().ToArray();

        bool hasBorderedHole = holeXs.Contains(0) || holeXs.Contains(board.Width - 1)
                            || holeYs.Contains(0) || holeYs.Contains(board.Height - 1);

        if (hasBorderedHole)
            return true;

        int[] ballXs = board.Balls.Select(b => b.X).Distinct().ToArray();
        int[] ballYs = board.Balls.Select(b => b.Y).Distinct().ToArray();

        int stackX = ballYs.Length;
        int stackY = ballXs.Length;

        int gapX = Math.Min(holeXs.Min(), board.Width - holeXs.Max() - 1);
        int gapY = Math.Min(holeYs.Min(), board.Height - holeYs.Max() - 1);

        bool someStackLargerThanGap = stackX > gapX || stackY > gapY;

        if (someStackLargerThanGap)
            return true;

        bool anyBallHasHoleOnSameLine = ballXs.Any(bx => holeXs.Contains(bx)) 
                                     || ballYs.Any(by => holeYs.Contains(by));

        if (anyBallHasHoleOnSameLine)
            return true;

        return false;
    }

    private Direction[] GetEffectiveDirections(Turn baseTurn)
    {
        Direction[] directions =
        [
            Direction.Top,
            Direction.Bottom,
            Direction.Left,
            Direction.Right
        ];

        List<Direction> effectiveDirections = new();

        foreach (Direction direction in directions)
        {
            if (baseTurn.Board.AreBallsAlignedTo(direction))
                continue;

            if (direction == baseTurn.Direction)
                continue;

            if (direction.IsOpposite(baseTurn.Direction))
            {
                if (baseTurn.Previous != null && baseTurn.Previous.Board.AreBallsAlignedTo(direction))
                    continue;
            }

            effectiveDirections.Add(direction);
        }

        return effectiveDirections.ToArray();
    }

    private bool ShouldAddResult(BoardState turnState, BfsSolutionMode mode)
        => turnState != BoardState.Solvable 
        && (turnState == BoardState.Win || mode == BfsSolutionMode.All);

    private bool ShouldInterruptDepth(BoardState turnState, BfsSolutionMode mode)
        => turnState == BoardState.Win
        && mode == BfsSolutionMode.FirstWin;

    private bool ShouldInterruptSolution(BoardState turnState, BfsSolutionMode mode)
        => turnState == BoardState.Win
        && (mode == BfsSolutionMode.FirstWin || mode == BfsSolutionMode.IncludeWinsOnSameDepth);
}