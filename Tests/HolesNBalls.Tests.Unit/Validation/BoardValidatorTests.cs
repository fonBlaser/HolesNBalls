using HolesNBalls.Exceptions.Validation;
using HolesNBalls.Validation;

namespace HolesNBalls.Tests.Unit.Validation;

[Trait("Category", "Unit")]
public class BoardValidatorTests
{
    //This test class contains few obvious duplicates, but it's done to show
    //the whole picture of the validation process and keep clean call stack.

    private readonly BoardValidator _validator = new();

    [Theory]
    [InlineData(-1, 2)]
    [InlineData(-0, 2)]
    [InlineData(2, -1)]
    [InlineData(2, 0)]
    public void ValidateDimensions_ForNegativeOrZeroDimensions_ThrowsException(int width, int height)
    {
        Assert.Throws<BoardValidationException>(() => _validator.ValidateDimensions(width, height));
    }

    [Fact]
    public void ValidateDimensions_ForOneCellBoard_ThrowsException()
    {
        Assert.Throws<BoardValidationException>(() => _validator.ValidateDimensions(1, 1));
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    public void ValidateDimensions_ForValidDimensions_DoesNotThrowException(int width, int height)
    {
        _validator.ValidateDimensions(width, height);
    }

    [Fact]
    public void ValidateHolesPresence_ForEmptyCollection_ThrowsException()
    {
        Assert.Throws<BoardValidationException>(() => _validator.ValidateHolesPresence(Array.Empty<Hole>()));
    }

    [Fact]
    public void ValidateHolesPresence_ForNonEmptyCollection_DoesNotThrowException()
    {
        _validator.ValidateHolesPresence([
            new()
            {
                Number = 1,
                X = 0,
                Y = 0
            }
        ]);
    }

    [Fact]
    public void ValidateHoleNumberUniqueness_ForHolesWithSameNumbers_ThrowsException()
    {
        Assert.Throws<BoardValidationException>(() => _validator.ValidateHoleNumberUniqueness([
            new()
            {
                Number = 1,
                X = 0,
                Y = 0
            },
            new()
            {
                Number = 1,
                X = 1,
                Y = 1
            }
        ]));
    }

    [Fact]
    public void ValidateHoleNumberUniqueness_ForHolesWithDifferentNumbers_DoesNotThrowException()
    {
        _validator.ValidateHoleNumberUniqueness([
            new()
            {
                Number = 1,
                X = 0,
                Y = 0
            },
            new()
            {
                Number = 2,
                X = 1,
                Y = 1
            }
        ]);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(2, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 2)]
    public void ValidateHolesBounds_ForHolesOutsideOfBoard_ThrowsException(int holeX, int holeY)
    {
        Board board = new()
        {
            Width = 2,
            Height = 2,
            Holes =
            [
                new()
                {
                    Number = 1,
                    X = holeX,
                    Y = holeY
                }
            ],
            Balls = []
        };

        Assert.Throws<BoardValidationException>(() => _validator.ValidateHolesBounds(board));
    }

    [Fact]
    public void ValidateHolesBounds_ForHolesInsideOfBoard_DoesNotThrowException()
    {
        Board board = new()
        {
            Width = 2,
            Height = 2,
            Holes =
            [
                new()
                {
                    Number = 1,
                    X = 0,
                    Y = 0
                }
            ],
            Balls = []
        };

        _validator.ValidateHolesBounds(board);
    }

    [Fact]
    public void ValidateBallsPresence_ForEmptyCollection_ThrowsException()
    {
        Assert.Throws<BoardValidationException>(() => _validator.ValidateBallsPresence(Array.Empty<Ball>()));
    }

    [Fact]
    public void ValidateBallsPresence_ForNonEmptyCollection_DoesNotThrowException()
    {
        _validator.ValidateBallsPresence([
            new()
            {
                Number = 1,
                X = 0,
                Y = 0
            }
        ]);
    }

    [Fact]
    public void ValidateBallNumberUniqueness_ForBallsWithSameNumbers_ThrowsException()
    {
        Assert.Throws<BoardValidationException>(() => _validator.ValidateBallNumberUniqueness([
            new()
            {
                Number = 1,
                X = 0,
                Y = 0
            },
            new()
            {
                Number = 1,
                X = 1,
                Y = 1
            }
        ]));
    }

    [Fact]
    public void ValidateBallNumberUniqueness_ForBallsWithDifferentNumbers_DoesNotThrowException()
    {
        _validator.ValidateBallNumberUniqueness([
            new()
            {
                Number = 1,
                X = 0,
                Y = 0
            },
            new()
            {
                Number = 2,
                X = 1,
                Y = 1
            }
        ]);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(2, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 2)]
    public void ValidateBallsBounds_ForBallsOutsideOfBoard_ThrowsException(int ballX, int ballY)
    {
        Board board = new()
        {
            Width = 2,
            Height = 2,
            Holes = [],
            Balls =
            [
                new()
                {
                    Number = 1,
                    X = ballX,
                    Y = ballY
                }
            ]
        };
        Assert.Throws<BoardValidationException>(() => _validator.ValidateBallsBounds(board));
    }

    [Fact]
    public void ValidateBallsBounds_ForBallsInsideOfBoard_DoesNotThrowException()
    {
        Board board = new()
        {
            Width = 2,
            Height = 2,
            Holes = [],
            Balls =
            [
                new()
                {
                    Number = 1,
                    X = 0,
                    Y = 0
                }
            ]
        };

        _validator.ValidateBallsBounds(board);
    }

    [Fact]
    public void ValidateHolesAndBallsPositionUniqueness_ForSameHolesPositions_ThrowsException()
    {
        Assert.Throws<BoardValidationException>(() => _validator.ValidateHolesAndBallsPositionUniqueness([
            new()
            {
                Number = 1,
                X = 0,
                Y = 0
            },
            new()
            {
                Number = 2,
                X = 0,
                Y = 0
            }
        ], Array.Empty<Ball>()));
    }

    [Fact]
    public void ValidateHolesAndBallsPositionUniqueness_ForSameBallsPositions_ThrowsException()
    {
        Assert.Throws<BoardValidationException>(() => _validator.ValidateHolesAndBallsPositionUniqueness(Array.Empty<Hole>(), [
            new()
            {
                Number = 1,
                X = 0,
                Y = 0
            },
            new()
            {
                Number = 2,
                X = 0,
                Y = 0
            }
        ]));
    }

    [Fact]
    public void ValidateHolesAndBallsPositionUniqueness_ForSameBallAndHolePositions_ThrowsException()
    {
        Assert.Throws<BoardValidationException>(() => _validator.ValidateHolesAndBallsPositionUniqueness([
            new()
            {
                Number = 1,
                X = 0,
                Y = 0
            }
        ], [
            new()
            {
                Number = 2,
                X = 0,
                Y = 0
            }
        ]));
    }

    [Fact]
    public void ValidateHolesAndBallsPositionUniqueness_ForDifferentBallAndHolePositions_DoesNotThrowException()
    {
        _validator.ValidateHolesAndBallsPositionUniqueness([
            new()
            {
                Number = 1,
                X = 0,
                Y = 0
            }
        ], [
            new()
            {
                Number = 2,
                X = 1,
                Y = 1
            }
        ]);
    }

    [Fact]
    public void ValidateEachBallHasHole_ForBallAndNoHoles_ThrowsException()
    {
        Assert.Throws<BoardValidationException>(() => _validator.ValidateEachBallHasHoleByNumber([
            new()
            {
                Number = 1,
                X = 0,
                Y = 0
            }
        ], Array.Empty<Hole>()));
    }

    [Fact]
    public void ValidateEachBallHasHole_ForBallWithoutHole_ThrowsException()
    {
        Assert.Throws<BoardValidationException>(() => _validator.ValidateEachBallHasHoleByNumber([
            new()
            {
                Number = 1,
                X = 0,
                Y = 0
            }
        ], [
            new()
            {
                Number = 0,
                X = 1,
                Y = 1
            }
        ]));
    }

    [Fact]
    public void ValidateEachBallHasHole_ForBallWithHole_DoesNotThrowException()
    {
        _validator.ValidateEachBallHasHoleByNumber([
            new()
            {
                Number = 1,
                X = 0,
                Y = 0
            }
        ], [
            new()
            {
                Number = 1,
                X = 1,
                Y = 1
            }
        ]);
    }

    // The general test with different invalid boards setup
    [Theory]
    [MemberData(nameof(InvalidBoards))]
    public void ValidateBoard_ForInvalidBoard_ThrowsException(NamedBoard namedBoard)
    {
        Assert.Throws<BoardValidationException>(() => _validator.ValidateBoard(namedBoard.Board));
    }

    //The general test with different valid boards setup
    [Theory]
    [MemberData(nameof(ValidBoards))]
    public void ValidateBoard_ForValidBoard_DoesNotThrowException(NamedBoard namedBoard)
    {
        _validator.ValidateBoard(namedBoard.Board);
    }

    public class NamedBoard
    {
        public string Name { get; init; }
        public Board Board { get; init; }

        public NamedBoard(string name, Board board)
        {
            Name = name;
            Board = board;
        }
    }

    #region Test Data

    public static IEnumerable<object[]> InvalidBoards => new List<object[]>
    {
        new object[]{
            new NamedBoard("Board with negative dimensions", new Board
            {
                Width = -1,
                Height = -1,
                Holes = [],
                Balls = []
            })
        },
        new object[]{
            new NamedBoard("Board with one cell", new Board
            {
                Width = 1,
                Height = 1,
                Holes = [],
                Balls = []
            })
        },
        new object[]{
            new NamedBoard("Board with hole outside of board", new Board
            {
                Width = 2,
                Height = 2,
                Holes = [
                    new Hole
                    {
                        Number = 1,
                        X = 2,
                        Y = 2
                    }
                ],
                Balls = []
            })
        },
        new object[]{
            new NamedBoard("Board with ball outside of board", new Board
            {
                Width = 2,
                Height = 2,
                Holes = [],
                Balls = [
                    new Ball
                    {
                        Number = 1,
                        X = 2,
                        Y = 2
                    }
                ]
            })
        },
        new object[]{
            new NamedBoard("Board with hole and ball in the same position", new Board
            {
                Width = 2,
                Height = 2,
                Holes = [
                    new Hole
                    {
                        Number = 1,
                        X = 0,
                        Y = 0
                    }
                ],
                Balls = [
                    new Ball
                    {
                        Number = 1,
                        X = 0,
                        Y = 0
                    }
                ]
            })
        }
    };

    public static IEnumerable<object[]> ValidBoards => new List<object[]>
    {
        new object[]
        {
            new NamedBoard("Board with two cells", new Board
            {
                Width = 1,
                Height = 2,
                Holes =
                [
                    new Hole
                    {
                        Number = 1,
                        X = 0,
                        Y = 0
                    }
                ],
                Balls =
                [
                    new Ball
                    {
                        Number = 1,
                        X = 0,
                        Y = 1
                    }
                ]
            })
        },
        new object[]
        {
            new NamedBoard("Board with hole and ball in different positions", new Board
            {
                Width = 2,
                Height = 2,
                Holes =
                [
                    new Hole
                    {
                        Number = 1,
                        X = 0,
                        Y = 0
                    }
                ],
                Balls =
                [
                    new Ball
                    {
                        Number = 1,
                        X = 1,
                        Y = 1
                    }
                ]
            })
        }
    };

    #endregion
}