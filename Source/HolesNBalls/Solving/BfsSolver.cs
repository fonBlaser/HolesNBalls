using HolesNBalls.Validation;

namespace HolesNBalls.Solving;

public class BfsSolver
{
    private List<Turn> _topTurns = null;

    public Turn InitialTurn { get; }
    public IReadOnlyCollection<Turn> TopDepthTurns => _topTurns;

    public BfsSolver(Board board)
    {
        new BoardValidator().ValidateBoard(board);
        InitialTurn = CreateTurn(board);
        _topTurns = [InitialTurn];
    }

    public List<Turn> Resolve(BfsSolutionMode mode = BfsSolutionMode.FirstWin)
    {
        throw new NotImplementedException();
    }

    private Turn CreateTurn(Board board)
    {
        BoardState state = GetBoardState(board);

        return new Turn(0, board, state);
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
}