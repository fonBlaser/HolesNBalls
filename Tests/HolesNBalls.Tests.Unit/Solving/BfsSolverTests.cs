using HolesNBalls.Exceptions.Validation;
using HolesNBalls.Solving;
using HolesNBalls.Tests.Unit._TestData;

namespace HolesNBalls.Tests.Unit.Solving;

[Trait("Category", "Unit")]
public class BfsSolverTests
{
    [Fact]
    public void BfsSolverCtor_ForInvalidBoard_ThrowsException()
    {
        Assert.Throws<BoardValidationException>(() => new BfsSolver(BoardsForBfsSolverTests.InvalidBoard));
    }

    [Fact]
    public void BfsSolverCtor_ForValidUnsolvableBoard_CreatesInitialTurnWithUnsolvableState()
    {
        List<Board> solvableBoards =
        [
            BoardsForBfsSolverTests.SolvableBoard_WithOneBallOneHole,
            BoardsForBfsSolverTests.SolvableBoard_WithFourBallsFourHoles_Stacked,
            BoardsForBfsSolverTests.SolvableBoard_WithFourBallsFourHoles_Spread,
            BoardsForBfsSolverTests.SolvableBoard_WithNineBallsNineHoles_Shuffled
        ];

        Assert.All(solvableBoards, board =>
        {
            BfsSolver solver = new(board);

            Assert.NotNull(solver.InitialTurn);
            Assert.Null(solver.InitialTurn.Previous);
            Assert.Equal(0, solver.InitialTurn.Number);
            Assert.Equal(BoardState.Solvable, solver.InitialTurn.State);
        });
    }

    [Fact]
    public void BfsSolverCtor_ForValidSolvableBoard_CreatesInitialTurnWithSolvableState()
    {
        List<Board> unsolvableBoards =
        [
            BoardsForBfsSolverTests.UnsolvableBoard_WithOneBallOneHole,
            BoardsForBfsSolverTests.UnsolvableBoard_WithFourBallsFourHoles_Stacked,
            BoardsForBfsSolverTests.UnsolvableBoard_WithFourBallsFourHoles_Spread,
            BoardsForBfsSolverTests.UnsolvableBoard_WithNineBallsNineHoles_Shuffled
        ];

        Assert.All(unsolvableBoards, board =>
        {
            BfsSolver solver = new(board);

            Assert.NotNull(solver.InitialTurn);
            Assert.Null(solver.InitialTurn.Previous);
            Assert.Equal(0, solver.InitialTurn.Number);
            Assert.Equal(BoardState.Unsolvable, solver.InitialTurn.State);
        });
    }

    [Theory]
    [InlineData(BfsSolutionMode.FirstWin)]
    [InlineData(BfsSolutionMode.IncludeWinsOnSameDepth)]
    [InlineData(BfsSolutionMode.IncludeWinsOnAllDepths)]
    public void BfsSolverSolve_ForUnsolvableBoardAndNonAllModes_ReturnsEmptyResult(BfsSolutionMode mode)
    {
        List<Board> unsolvableBoards =
        [
            BoardsForBfsSolverTests.UnsolvableBoard_WithOneBallOneHole,
            BoardsForBfsSolverTests.UnsolvableBoard_WithFourBallsFourHoles_Stacked,
            BoardsForBfsSolverTests.UnsolvableBoard_WithFourBallsFourHoles_Spread,
            BoardsForBfsSolverTests.UnsolvableBoard_WithNineBallsNineHoles_Shuffled
        ];

        Assert.All(unsolvableBoards, board =>
        {
            BfsSolver solver = new(board);
            List<Turn> result = solver.Solve(mode);

            Assert.NotNull(result);
            Assert.Empty(result);
        });
    }

    [Fact]
    public void BfsSolverSolve_ForUnsolvableBoardAndAllMode_ReturnsInitialTurn()
    {
        List<Board> unsolvableBoards =
        [
            BoardsForBfsSolverTests.UnsolvableBoard_WithOneBallOneHole,
            BoardsForBfsSolverTests.UnsolvableBoard_WithFourBallsFourHoles_Stacked,
            BoardsForBfsSolverTests.UnsolvableBoard_WithFourBallsFourHoles_Spread,
            BoardsForBfsSolverTests.UnsolvableBoard_WithNineBallsNineHoles_Shuffled
        ];

        Assert.All(unsolvableBoards, board =>
        {
            BfsSolver solver = new(board);
            List<Turn> result = solver.Solve(BfsSolutionMode.All);
            
            Turn resultTurn = Assert.Single(result);

            Assert.Equal(0, resultTurn.Number);
            Assert.Equal(BoardState.Unsolvable, resultTurn.State);
            Assert.Empty(resultTurn.GetMoveSequence());
        });
    }

    [Fact]
    public void BfsSolverSolveFirstWinMode_ForSimpleBoard_ReturnsSingleSolutionWithTwoTurnsInSequence()
    {
        Board board = BoardsForBfsSolverTests.SolvableBoard_WithOneBallOneHole;

        BfsSolver solver = new(board);

        List<Turn> results = solver.Solve();

        Turn result = Assert.Single(results);
        Assert.Equal(2, result.Number);
        Assert.Equal(BoardState.Win, result.State);

        Direction[] sequence = result.GetMoveSequence();
        Assert.Equal(2, sequence.Length);
    }

    [Fact]
    public void BfsSolverSolveAllMode_ForSimpleBoard_ReturnsLotOfWins()
    {
        Board board = BoardsForBfsSolverTests.SolvableBoard_WithOneBallOneHole;

        BfsSolver solver = new(board);

        List<Turn> results = solver.Solve(BfsSolutionMode.All);

        Assert.NotEmpty(results);
        Assert.All(results, r => Assert.Equal(BoardState.Win, r.State));
    }

    [Fact]
    public void BfsSolverSolveFirstWinMode_ForSimpleBoardWithDifferentHolesAndOneBall_ReturnsOneWinAndOneLose()
    {
        Board board = BoardsForBfsSolverTests.WinnableAndLosableBoard_WithOneBallTwoHoles;

        BfsSolver solver = new(board);

        List<Turn> results = solver.Solve();

        Turn result = Assert.Single(results);
        Assert.Equal(BoardState.Win, result.State);
    }

    [Fact]
    public void BfsSolverSolveIncludeWinsOnSameDepthMode_ForSimpleBoardWithDifferentHolesAndOneBall_ReturnsTwoWins()
    {
        Board board = BoardsForBfsSolverTests.WinnableAndLosableBoard_WithOneBallTwoHoles;

        BfsSolver solver = new(board);
        List<Turn> results = solver.Solve(BfsSolutionMode.IncludeWinsOnSameDepth);
        Assert.Equal(2, results.Count);
        Assert.All(results, r =>
        {
            Assert.Equal(BoardState.Win, r.State);
            Assert.Equal(2, r.Number);
        });
    }

    [Fact]
    public void BfsSolverSolveIncludeWinsOnAllDepthsMode_ForSimpleBoardWithDifferentHolesAndOneBall_ReturnsWinsWithDifferentDepths()
    {
        Board board = BoardsForBfsSolverTests.WinnableAndLosableBoard_WithOneBallTwoHoles;

        BfsSolver solver = new(board);
        List<Turn> results = solver.Solve(BfsSolutionMode.IncludeWinsOnAllDepths);

        Assert.Equal(12, results.Count);
        Assert.All(results, r => Assert.Equal(BoardState.Win, r.State));
        Assert.NotEqual(1, results.Select(r => r.Number).Distinct().Count());
    }

    [Fact]
    public void BfsSolverSolveAllMode_ForSimpleBoardWithDifferentHolesAndOneBall_ReturnsLotsOfWinsAndLosesWithDifferentDepths()
    {
        Board board = BoardsForBfsSolverTests.WinnableAndLosableBoard_WithOneBallTwoHoles;

        BfsSolver solver = new(board);
        List<Turn> results = solver.Solve(BfsSolutionMode.All);

        Turn[] wins = results.Where(r => r.State == BoardState.Win).ToArray();
        Turn[] loses = results.Where(r => r.State == BoardState.Lost).ToArray();

        Assert.Equal(12, wins.Length);
        Assert.Equal(12, loses.Length);

        Assert.NotEqual(1, wins.Length);
        Assert.NotEqual(1, loses.Length);
    }
}