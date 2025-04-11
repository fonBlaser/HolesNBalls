namespace HolesNBalls.Tests.Unit;

[Trait("Category", "Unit")]
public class BoardMoveTests
{
    [Fact]
    public void BoardDropsAndTurnDrops_ByDefault_AreEmpty()
    {
        Board board = _TestData.BoardsForMoveTests.SimpleBoard_3x3_HoleLeftTop_BallCenter;

        Assert.Empty(board.Drops);
    }

    [Theory]
    [InlineData(MoveDirection.Top, 1, 0)]
    [InlineData(MoveDirection.Bottom, 1, 2)]
    [InlineData(MoveDirection.Left, 0, 1)]
    [InlineData(MoveDirection.Right, 2, 1)]
    public void BoardWithOneBallCenter_Move_UpdatesBallPosition(MoveDirection direction, int updatedX, int updatedY)
    {
        Board board = _TestData.BoardsForMoveTests.SimpleBoard_3x3_HoleLeftTop_BallCenter;
        Ball sourceBall = board.Balls.Single();

        Board updatedBoard = board.Move(direction);
        Assert.NotEqual(updatedBoard, board);

        Ball updatedBall = updatedBoard.Balls.Single();
        Assert.NotEqual(sourceBall, updatedBall);
        Assert.Equal(sourceBall.Number, updatedBall.Number);
        Assert.Equal(updatedX, updatedBall.X);
        Assert.Equal(updatedY, updatedBall.Y);
    }

    [Fact]
    public void BoardWithOneBallCenterOneHoleTopLeft_MoveTopThenLeft_HasDropAndNoBalls()
    {
        Board board = _TestData.BoardsForMoveTests.SimpleBoard_3x3_HoleLeftTop_BallCenter;
        Ball sourceBall = board.Balls.Single();

        Board updatedBoard = board.Move(MoveDirection.Top).Move(MoveDirection.Left);

        Assert.NotEqual(updatedBoard, board);
        Assert.Empty(updatedBoard.Balls);

        Drop drop = Assert.Single(updatedBoard.Drops);
        Assert.Equal(sourceBall.Number, drop.Ball.Number);
    }

    [Theory]
    [InlineData(MoveDirection.Top, MoveDirection.Left, 0, 2, 0, 2)] // Top left corner
    [InlineData(MoveDirection.Left, MoveDirection.Top, 0, 2, 0, 2)] // Top left corner
    [InlineData(MoveDirection.Bottom, MoveDirection.Left, 0, 2, 4, 6)] // Bottom left corner
    [InlineData(MoveDirection.Left, MoveDirection.Bottom, 0, 2, 4, 6)] // Bottom left corner
    [InlineData(MoveDirection.Top, MoveDirection.Right, 4, 6, 0, 2)] // Top right corner
    [InlineData(MoveDirection.Right, MoveDirection.Top, 4, 6, 0, 2)] // Top right corner
    [InlineData(MoveDirection.Bottom, MoveDirection.Right, 4, 6, 4, 6)] // Bottom right corner
    [InlineData(MoveDirection.Right, MoveDirection.Bottom, 4, 6, 4, 6)] // Bottom right corner
    public void Board7x7With9BallsWithSpaces_WithinTwoMoves_HasBallsStack3x3InCorner(MoveDirection first,
        MoveDirection second, int minX, int maxX, int minY, int maxY)
    {
        Board board = _TestData.BoardsForMoveTests.Board_7x7_With9BallsWithSpaces_WithoutHoles;
        int ballsCount = board.Balls.Count;

        Board updatedBoard = board.Move(first).Move(second);

        Assert.NotEqual(updatedBoard, board);
        Assert.Empty(updatedBoard.Drops);
        Assert.Equal(ballsCount, updatedBoard.Balls.Count);

        Assert.All(updatedBoard.Balls, ball => Assert.True(ball.X >= minX));
        Assert.All(updatedBoard.Balls, ball => Assert.True(ball.X <= maxX));
        Assert.All(updatedBoard.Balls, ball => Assert.True(ball.Y >= minY));
        Assert.All(updatedBoard.Balls, ball => Assert.True(ball.Y <= maxY));

    }

    [Fact]
    public void Board3x5With4BallsWithHolesAnd1FreeBall_WithinTwoMoves_HasSubsequentDrops()
    {
        Board board = _TestData.BoardsForMoveTests.Board_3x5_With5Balls_WithHolesBetweenBorderBalls_AndFreeMiddleBall;

        Ball leftTopBall = board.Balls.Single(b => b is { X: 0, Y: 1 });
        Ball rightTopBall = board.Balls.Single(b => b is { X: 2, Y: 1 });
        Ball leftBottomBall = board.Balls.Single(b => b is { X: 0, Y: 3 });
        Ball rightBottomBall = board.Balls.Single(b => b is { X: 2, Y: 3 });
        Ball freeBall = board.Balls.Single(b => b is { X : 1 });

        Hole leftHole = board.Holes.Single(h => h is { X: 0, Y: 2 });
        Hole rightHole = board.Holes.Single(h => h is { X: 2, Y: 2 });


        //First move
        Board boardAfterMoveTop = board.Move(MoveDirection.Top);

        //Two drops there and three balls left
        Assert.Equal(2, boardAfterMoveTop.Drops.Count);
        Assert.Equal(3, boardAfterMoveTop.Balls.Count);

        //Left bottom ball dropped to left hole
        Assert.Contains(boardAfterMoveTop.Drops, drop => drop.Ball.Number == leftBottomBall.Number
                                                      && drop.Hole == leftHole);
        Assert.DoesNotContain(boardAfterMoveTop.Balls, ball => ball.Number == leftBottomBall.Number);

        //Right bottom ball dropped to right hole
        Assert.Contains(boardAfterMoveTop.Drops, drop => drop.Ball.Number == rightBottomBall.Number 
                                                      && drop.Hole == rightHole);
        Assert.DoesNotContain(boardAfterMoveTop.Balls, ball => ball.Number == rightBottomBall.Number);

        //Middle ball left
        Assert.Contains(boardAfterMoveTop.Balls, ball => ball.Number == freeBall.Number);


        //Second move
        Board boardAfterMoveBottom = boardAfterMoveTop.Move(MoveDirection.Bottom);

        //Four drops there and one ball left
        Assert.Equal(4, boardAfterMoveBottom.Drops.Count);
        Assert.Single(boardAfterMoveBottom.Balls);

        //Left top ball dropped to left hole
        Assert.Contains(boardAfterMoveBottom.Drops, drop => drop.Ball.Number == leftTopBall.Number 
                                                         && drop.Hole == leftHole);
        Assert.DoesNotContain(boardAfterMoveBottom.Balls, ball => ball.Number == leftTopBall.Number);

        //Right top ball dropped to right hole
        Assert.Contains(boardAfterMoveBottom.Drops, drop => drop.Ball.Number == rightTopBall.Number 
                                                         && drop.Hole == rightHole);
        Assert.DoesNotContain(boardAfterMoveBottom.Balls, ball => ball.Number == rightTopBall.Number);

        //Middle ball left
        Assert.Contains(boardAfterMoveBottom.Balls, ball => ball.Number == freeBall.Number);
    }

    [Fact]
    public void Board5x3With4BallsWithHolesAnd1FreeBall_WithinTwoMoves_HasSubsequentDrops()
    {
        Board board = _TestData.BoardsForMoveTests.Board_5x3_With5Balls_WithHolesBetweenBorderBalls_AndFreeMiddleBall;
        
        Ball leftTopBall = board.Balls.Single(b => b is { X: 1, Y: 0 });
        Ball rightTopBall = board.Balls.Single(b => b is { X: 3, Y: 0 });
        Ball leftBottomBall = board.Balls.Single(b => b is { X: 1, Y: 2 });
        Ball rightBottomBall = board.Balls.Single(b => b is { X: 3, Y: 2 });
        Ball freeBall = board.Balls.Single(b => b is { X: 2, Y: 1 });

        Hole topHole = board.Holes.Single(h => h is { X: 2, Y: 0 });
        Hole bottomHole = board.Holes.Single(h => h is { X: 2, Y: 2 });


        //First move
        Board boardAfterMoveLeft = board.Move(MoveDirection.Left);

        //Two drops there and three balls left
        Assert.Equal(2, boardAfterMoveLeft.Drops.Count);
        Assert.Equal(3, boardAfterMoveLeft.Balls.Count);

        //Right top ball dropped to top hole
        Assert.Contains(boardAfterMoveLeft.Drops, drop => drop.Ball.Number == rightTopBall.Number
                                                       && drop.Hole == topHole);
        Assert.DoesNotContain(boardAfterMoveLeft.Balls, ball => ball.Number == rightTopBall.Number);

        //Right bottom ball dropped to bottom hole
        Assert.Contains(boardAfterMoveLeft.Drops, drop => drop.Ball.Number == rightBottomBall.Number
                                                       && drop.Hole == bottomHole);
        Assert.DoesNotContain(boardAfterMoveLeft.Balls, ball => ball.Number == rightBottomBall.Number);

        //Middle ball left
        Assert.Contains(boardAfterMoveLeft.Balls, ball => ball.Number == freeBall.Number);


        //Second move
        Board boardAfterMoveRight = boardAfterMoveLeft.Move(MoveDirection.Right);

        //Four drops there and one ball left
        Assert.Equal(4, boardAfterMoveRight.Drops.Count);
        Assert.Single(boardAfterMoveRight.Balls);

        //Left top ball dropped to top hole
        Assert.Contains(boardAfterMoveRight.Drops, drop => drop.Ball.Number == leftTopBall.Number
                                                        && drop.Hole == topHole);
        Assert.DoesNotContain(boardAfterMoveRight.Balls, ball => ball.Number == leftTopBall.Number);

        //Left bottom ball dropped to bottom hole
        Assert.Contains(boardAfterMoveRight.Drops, drop => drop.Ball.Number == leftBottomBall.Number
                                                        && drop.Hole == bottomHole);
        Assert.DoesNotContain(boardAfterMoveRight.Balls, ball => ball.Number == leftBottomBall.Number);

        //Middle ball left
        Assert.Contains(boardAfterMoveRight.Balls, ball => ball.Number == freeBall.Number);
    }

    [Fact]
    public void BoardWithVerticalOrientationAndFreeBallInMiddle_WithingFourMoves_HasAllDropsAndNoBalls()
    {
        Board board = _TestData.BoardsForMoveTests.Board_3x5_With5Balls_WithHolesBetweenBorderBalls_AndFreeMiddleBall;

        Board resultBoard = board.Move(MoveDirection.Top)
                                 .Move(MoveDirection.Bottom)
                                 .Move(MoveDirection.Left)
                                 .Move(MoveDirection.Top);

        Assert.NotEqual(resultBoard, board);

        Assert.Empty(resultBoard.Balls);
        Assert.Equal(5, resultBoard.Drops.Count);
    }

    [Fact]
    public void BoardWithHorizontalOrientationAndFreeBallInMiddle_WithingFourMoves_HasAllDropsAndNoBalls()
    {
        Board board = _TestData.BoardsForMoveTests.Board_5x3_With5Balls_WithHolesBetweenBorderBalls_AndFreeMiddleBall;

        Board resultBoard = board.Move(MoveDirection.Left)
                                 .Move(MoveDirection.Right)
                                 .Move(MoveDirection.Top)
                                 .Move(MoveDirection.Left);

        Assert.NotEqual(resultBoard, board);

        Assert.Empty(resultBoard.Balls);
        Assert.Equal(5, resultBoard.Drops.Count);
    }
}
