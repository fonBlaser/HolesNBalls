namespace HolesNBalls.Tests.Unit._TestData;

public static class Boards
{
    public static Board SimpleBoard_2x2_1Hole_1Ball =>
        new()
        {
            Width = 2,
            Height = 2,
            Holes = new List<Hole>
            {
                new() {Number = 0, X = 0, Y = 0}
            },
            Balls = new List<Ball>
            {
                new() {Number = 0, X = 1, Y = 1}
            }
        };
}