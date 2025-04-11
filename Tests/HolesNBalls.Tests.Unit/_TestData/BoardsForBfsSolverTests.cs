namespace HolesNBalls.Tests.Unit._TestData;

public static class BoardsForBfsSolverTests
{
    public static Board InvalidBoard =>
        new()
        {
            Width = 3,
            Height = 3,
            Holes = new List<Hole>(),
            Balls = new List<Ball>()
        };

    public static Board UnsolvableBoard_WithOneBallOneHole =>
        new()
        {
            Width = 3,
            Height = 3,
            Holes = new List<Hole>
            {
                new() { Number = 0, X = 1, Y = 1 }
            },
            Balls = new List<Ball>
            {
                new() { Number = 0, X = 0, Y = 0 }
            }
        };

    public static Board UnsolvableBoard_WithFourBallsFourHoles_Stacked =>
        new()
        {
            Width = 8,
            Height = 8,
            Holes = new List<Hole>
            {
                new() { Number = 0, X = 2, Y = 2 }, new() { Number = 1, X = 5, Y = 2 },
                new() { Number = 2, X = 2, Y = 5 }, new() { Number = 3, X = 5, Y = 5 }
            },
            Balls = new List<Ball>()
            {
                new() { Number = 0, X = 3, Y = 3 }, new() { Number = 1, X = 4, Y = 3 },
                new() { Number = 2, X = 3, Y = 4 }, new() { Number = 3, X = 4, Y = 4 }
            }
        };

    public static Board UnsolvableBoard_WithFourBallsFourHoles_Spread =>
        new()
        {
            Width = 14,
            Height = 14,
            Holes = new List<Hole>
            {
                new() { Number = 0, X = 4, Y = 4 }, new() { Number = 1, X = 9, Y = 4 },
                new() { Number = 2, X = 4, Y = 9 }, new() { Number = 3, X = 9, Y = 9 }
            },
            Balls = new List<Ball>()
            {
                new() { Number = 0, X = 5, Y = 5 },
                new() { Number = 1, X = 6, Y = 6 },
                new() { Number = 2, X = 7, Y = 7 },
                new() { Number = 3, X = 8, Y = 8 }
            }
        };

    public static Board UnsolvableBoard_WithNineBallsNineHoles_Shuffled =>
        new()
        {
            Width = 12,
            Height = 12,
            Holes = new List<Hole>()
            {
                new() { Number = 0, X = 3, Y = 3 }, new() { Number = 1, X = 5, Y = 3 }, new() {Number = 3, X = 7, Y = 3},
                new() { Number = 4, X = 3, Y = 5 }, new() { Number = 5, X = 5, Y = 5 }, new() {Number = 6, X = 7, Y = 5},
                new() { Number = 7, X = 3, Y = 7 }, new() { Number = 8, X = 5, Y = 7 }, new() {Number = 9, X = 7, Y = 7}
            },
            Balls = new List<Ball>()
            {
                new() { Number = 0, X = 4, Y = 4 }, new() { Number = 1, X = 6, Y = 4 }, new() {Number = 3, X = 8, Y = 4},
                new() { Number = 4, X = 4, Y = 6 }, new() { Number = 5, X = 6, Y = 6 }, new() {Number = 6, X = 8, Y = 6},
                new() { Number = 7, X = 4, Y = 8 }, new() { Number = 8, X = 6, Y = 8 }, new() {Number = 9, X = 8, Y = 8}
            }
        };

    public static Board SolvableBoard_WithOneBallOneHole =>
        new()
        {
            Width = 3,
            Height = 3,
            Holes = new List<Hole>
            {
                new() { Number = 0, X = 0, Y = 0 }
            },
            Balls = new List<Ball>
            {
                new() { Number = 0, X = 1, Y = 1 }
            }
        };

    public static Board SolvableBoard_WithFourBallsFourHoles_Stacked =>
        new()
        {
            Width = 8,
            Height = 8,
            Holes = new List<Hole>
            {
                new() { Number = 0, X = 2, Y = 2 }, new() { Number = 1, X = 3, Y = 2 },
                new() { Number = 2, X = 2, Y = 3 }, new() { Number = 3, X = 3, Y = 3 }
            },
            Balls = new List<Ball>()
            {
                new() { Number = 0, X = 4, Y = 2 }, new() { Number = 1, X = 5, Y = 2 },
                new() { Number = 2, X = 4, Y = 3 }, new() { Number = 3, X = 5, Y = 3 }
            }
        };

    public static Board SolvableBoard_WithFourBallsFourHoles_Spread =>
        new()
        {
            Width = 5,
            Height = 4,
            Holes = new List<Hole>
            {
                new() { Number = 0, X = 0, Y = 0 }, 
                new() { Number = 1, X = 1, Y = 1 },
                new() { Number = 2, X = 2, Y = 2 }, 
                new() { Number = 3, X = 3, Y = 3 }
            },
            Balls = new List<Ball>()
            {
                new() { Number = 0, X = 1, Y = 0 },
                new() { Number = 1, X = 2, Y = 1 },
                new() { Number = 2, X = 3, Y = 2 },
                new() { Number = 3, X = 4, Y = 3 }
            }
        };

    public static Board SolvableBoard_WithNineBallsNineHoles_Shuffled =>
        new()
        {
            Width = 3,
            Height = 6,
            Holes = new List<Hole>()
            {
                new() { Number = 0, X = 0, Y = 0 }, new() { Number = 1, X = 0, Y = 2 }, new() { Number = 3, X = 0, Y = 4 },
                new() { Number = 4, X = 1, Y = 0 }, new() { Number = 5, X = 1, Y = 2 }, new() { Number = 6, X = 1, Y = 4 },
                new() { Number = 7, X = 2, Y = 0 }, new() { Number = 8, X = 2, Y = 2 }, new() { Number = 9, X = 2, Y = 4 }
            },
            Balls = new List<Ball>()
            {
                new() { Number = 0, X = 0, Y = 1 }, new() { Number = 1, X = 0, Y = 3 }, new() { Number = 3, X = 0, Y = 5 },
                new() { Number = 4, X = 1, Y = 1 }, new() { Number = 5, X = 1, Y = 3 }, new() { Number = 6, X = 1, Y = 5 },
                new() { Number = 7, X = 2, Y = 1 }, new() { Number = 8, X = 2, Y = 3 }, new() { Number = 9, X = 2, Y = 5 }
            }
        };


    public static Board WinnableAndLosableBoard_WithOneBallTwoHoles =>
        new()
        {
            Width = 3,
            Height = 3,
            Holes = new List<Hole>
            {
                new() { Number = 0, X = 0, Y = 0 },
                new() { Number = 1, X = 2, Y = 2 }
            },
            Balls = new List<Ball>
            {
                new() { Number = 0, X = 1, Y = 1 }
            }
        };
}