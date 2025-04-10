namespace HolesNBalls.Tests.Unit._TestData;

public static class Boards
{
    public static Board SimpleBoard_3x3_HoleLeftTop_BallCenter =>
        new()
        {
            Width = 3,
            Height = 3,
            Holes = new List<Hole>
            {
                new() {Number = 0, X = 0, Y = 0}
            },
            Balls = new List<Ball>
            {
                new() {Number = 0, X = 1, Y = 1}
            }
        };

    public static Board Board_7x7_With9BallsWithSpaces_WithoutHoles =>
        new()
        {
            Width = 7,
            Height = 7,
            Holes = new List<Hole>(),
            Balls = new List<Ball>
            {
                new() {Number = 0, X = 1, Y = 1}, new() {Number = 0, X = 3, Y = 1}, new() {Number = 0, X = 5, Y = 1},
                new() {Number = 0, X = 1, Y = 3}, new() {Number = 0, X = 3, Y = 3}, new() {Number = 0, X = 5, Y = 3},
                new() {Number = 0, X = 1, Y = 5}, new() {Number = 0, X = 3, Y = 5}, new() {Number = 0, X = 5, Y = 5}
            }
        };

    public static Board Board_3x5_With5Balls_WithHolesBetweenBorderBalls_AndFreeMiddleBall =>
        new()
        {
            Width = 3,
            Height = 5,
            Holes = new List<Hole>
            {
                new() {Number = 0, X = 0, Y = 2},
                new() {Number = 1, X = 2, Y = 2},
            },
            Balls = new List<Ball>
            {
                new() {Number = 0, X = 0, Y = 1},
                new() {Number = 1, X = 2, Y = 1},
                new() {Number = 2, X = 1, Y = 2}, //Free ball for vertical line
                new() {Number = 3, X = 0, Y = 3},
                new() {Number = 4, X = 2, Y = 3}
            }
        };

    public static Board Board_5x3_With5Balls_WithHolesBetweenBorderBalls_AndFreeMiddleBall =>
        new()
        {
            Width = 5,
            Height = 3,
            Holes = new List<Hole>
            {
                new() {Number = 0, X = 2, Y = 0},
                new() {Number = 1, X = 2, Y = 2},
            },
            Balls = new List<Ball>
            {
                new() {Number = 0, X = 1, Y = 0},
                new() {Number = 1, X = 3, Y = 0},
                new() {Number = 2, X = 2, Y = 1}, //Free ball for horizontal line
                new() {Number = 3, X = 1, Y = 2},
                new() {Number = 4, X = 3, Y = 2}
            }
        };
}