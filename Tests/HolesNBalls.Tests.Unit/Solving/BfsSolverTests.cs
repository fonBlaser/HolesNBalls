using HolesNBalls.Exceptions.Validation;
using HolesNBalls.Solving;
using HolesNBalls.Tests.Unit._TestData;

namespace HolesNBalls.Tests.Unit.Solving;

[Trait("Category", "Unit")]
public class BfsSolverTests
{
    [Fact]
    public void BfsSolver_ForInvalidBoard_ThrowsException()
    {
        Assert.Throws<BoardValidationException>(() => new BfsSolver(BoardsForBfsSolverTests.InvalidBoard));
    }

    [Fact]
    public void BfsSolver_ForValidUnsolvableBoard_CreatesInitialTurnWithUnsolvableState()
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
            Assert.Equal(0, solver.InitialTurn.Number);
            Assert.Equal(BoardState.Solvable, solver.InitialTurn.State);
        });
    }

    [Fact]
    public void BfsSolver_ForValidSolvableBoard_CreatesInitialTurnWithSolvableState()
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
            Assert.Equal(0, solver.InitialTurn.Number);
            Assert.Equal(BoardState.Unsolvable, solver.InitialTurn.State);
        });
    }
}