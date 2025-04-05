namespace HolesNBalls.Tests.Unit;

[Trait("Category", "Unit")]
public class BoardMoveTests
{
    [Fact]
    public void BoardDropsAndTurnDrops_ByDefault_AreEmpty()
    {
        Board board = _TestData.Boards.SimpleBoard_2x2_1Hole_1Ball;
        
        Assert.Empty(board.TurnDrops);
        Assert.Empty(board.Drops);
    }
    
    [Fact]
    public void BoardWithOneBallAtBottom_MoveTop_MovesBallToTop()
    {
        Board board = _TestData.Boards.SimpleBoard_2x2_1Hole_1Ball;
        Ball sourceBall = board.Balls.Single();
        
        Board updatedBoard = board.MoveTop();
        
        Assert.NotEqual(updatedBoard, board);
        Ball updatedBall = updatedBoard.Balls.Single();
        Assert.NotEqual(sourceBall, updatedBall);
        Assert.Equal(sourceBall.Number, updatedBall.Number);
        Assert.Equal(sourceBall.X, updatedBall.X);
        Assert.Equal(sourceBall.Y - 1, updatedBall.Y);
    }
    
    [Fact]
    public void BoardWithOneBallMoveTop_WhenNoHoleMatches_DoesNotAddDrop()
    {
        Board board = _TestData.Boards.SimpleBoard_2x2_1Hole_1Ball;
        
        board.MoveTop();
        
        Assert.Empty(board.TurnDrops);
        Assert.Empty(board.Drops);
    }
}