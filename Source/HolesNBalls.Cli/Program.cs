using System.Diagnostics;
using HolesNBalls;
using HolesNBalls.Exceptions.Validation;
using HolesNBalls.Solving;
using HolesNBalls.Validation;

try
{
    PrintInstructions();

    BoardValidator validator = new BoardValidator();

    Coordinates boardDimensions = AskBoardWidthAndHeight(validator);
    int holesAmount = AskHolesAmount();
    int ballsAmount = AskBallsAmount(holesAmount);
    List<Hole> holes = AskHolePositions(holesAmount, boardDimensions, validator);
    List<Ball> balls = AskBallPositions(ballsAmount, holes, boardDimensions, validator);

    Board board = new()
    {
        Width = boardDimensions.X,
        Height = boardDimensions.Y,
        Holes = holes,
        Balls = balls
    };

    validator.ValidateBoard(board);

    BfsSolutionMode mode = AskForSolutionMode();
    BfsSolver solver = new BfsSolver(board);

    Console.WriteLine("Solving...");

    Stopwatch sw = Stopwatch.StartNew();
    List<Turn> finalSolutionTurns = solver.Solve(mode);
    sw.Stop();

    Console.WriteLine($"Solved in {sw.ElapsedMilliseconds} ms.\n\n");
    if (finalSolutionTurns.Count == 0)
    {
        Console.WriteLine("No solutions found.");
    }
    else
    {
        Console.WriteLine($"Found {finalSolutionTurns.Count} solution(s). Traces below:");
        foreach (Turn finalTurn in finalSolutionTurns)
            Console.WriteLine(finalTurn);
    }
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.StackTrace);
}
finally
{
    Console.WriteLine("\n\nPress any key to exit...");
    Console.ReadKey();
}

void PrintInstructions()
{
    Console.WriteLine("Welcome to the Holes and Balls game solver!");
    Console.WriteLine("You will be prompted to enter the board width and height.");
    Console.WriteLine("Then you'll setup Holes and Balls.");
    Console.WriteLine();
    Console.WriteLine("Solver will find first win, same depth wins, all wins, or even loses - whatever you choose.");
    Console.WriteLine();
    Console.WriteLine("Some rules:");
    Console.WriteLine("- Board should have at least two cells (1x2, 2x1)");
    Console.WriteLine("- Board should have at least one Ball and one Hole");
    Console.WriteLine("- Balls amount should be less or equal to Holes amount");
    Console.WriteLine("- Holes and Balls coordinates should be in range [0 : Width - 1, 0 : Height - 1]");
    Console.WriteLine("- Holes and Balls coordinates should be unique");
    Console.WriteLine("- All coordinates are entered with space between them");
    Console.WriteLine();
    Console.WriteLine("Good luck!");
    Console.WriteLine("___________________________________________________________________________________________\n\n");
}

Coordinates AskBoardWidthAndHeight(BoardValidator validator)
{
    do
    {
        try
        {
            Console.Write("Board width and height: ");
            Coordinates coordinates = WaitForCoordinates();
            validator.ValidateDimensions(coordinates.X, coordinates.Y);
            return coordinates;
        }
        catch (BoardValidationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    } while(true);
}

int AskHolesAmount()
{
    do
    {
        try
        {
            Console.Write("Holes amount: ");
            int holesAmount = WaitForInt();
            if (holesAmount < 1)
                throw new BoardValidationException("Holes amount should be greater than 0.");
            return holesAmount;
        }
        catch (BoardValidationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    } while (true);
}

int AskBallsAmount(int holesAmount)
{
    do
    {
        try
        {
            Console.Write("Balls amount: ");
            int ballsAmount = WaitForInt();
            if (ballsAmount < 1)
                throw new BoardValidationException("Balls amount should be greater than 0.");
            if (ballsAmount > holesAmount)
                throw new BoardValidationException("Balls amount should be less or equal to Holes amount.");
            return ballsAmount;
        }
        catch (BoardValidationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    } while (true);
}

List<Hole> AskHolePositions(int holesAmount, Coordinates boardDimensions, BoardValidator validator)
{
    List<Hole> holes = new();
    for (int i = 0; i < holesAmount; i++)
    {
        do
        {
            try
            {
                Console.Write($"Hole {i + 1} coordinates: ");
                Coordinates coordinates = WaitForCoordinates();
                validator.ValidateCoordinatesBounds(coordinates, boardDimensions.X, boardDimensions.Y, nameof(Hole));
                
                Hole? existingHole = holes.FirstOrDefault(h => h.X == coordinates.X && h.Y == coordinates.Y);
                if(existingHole != null)
                    throw new BoardValidationException($"Hole {existingHole.Number} already exists at the same position.");

                holes.Add(new Hole { Number = i + 1, X = coordinates.X, Y = coordinates.Y });
                break;
            }
            catch (BoardValidationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (true);
    }
    return holes;
}

List<Ball> AskBallPositions(int ballsAmount, List<Hole> holes, Coordinates boardDimensions, BoardValidator validator)
{
    List<Ball> balls = new();
    for (int i = 0; i < ballsAmount; i++)
    {
        do
        {
            try
            {
                Console.Write($"Ball {i + 1} coordinates: ");
                Coordinates coordinates = WaitForCoordinates();
                validator.ValidateCoordinatesBounds(coordinates, boardDimensions.X, boardDimensions.Y, nameof(Ball));

                Hole? existingHole = holes.FirstOrDefault(h => h.X == coordinates.X && h.Y == coordinates.Y);
                if (existingHole != null)
                    throw new BoardValidationException($"Hole {existingHole.Number} already exists at the same position.");

                Ball? existingBall = balls.FirstOrDefault(b => b.X == coordinates.X && b.Y == coordinates.Y);
                if (existingBall != null)
                    throw new BoardValidationException($"Ball {existingBall.Number} already exists at the same position.");

                balls.Add(new Ball { Number = i + 1, X = coordinates.X, Y = coordinates.Y });
                break;
            }
            catch (BoardValidationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (true);
    }
    return balls;
}

BfsSolutionMode AskForSolutionMode()
{
    do
    {
        try
        {
            Console.Write("Solution mode (1 - first win, 2 - same depth wins, 3 - all wins, 4 - all wins and loses): ");
            int mode = WaitForInt();
            if (mode < 1 || mode > 4)
                throw new BoardValidationException("Invalid solution mode. Choose from 1 to 4.");
            return (BfsSolutionMode)mode;
        }
        catch (BoardValidationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    } while (true);
}

Coordinates WaitForCoordinates()
{
    string[] parts = ReadInputParts();

    if (parts.Length != 2)
        throw new BoardValidationException("You should enter two numbers separated by space.");

    if (!int.TryParse(parts[0], out int width) || !int.TryParse(parts[1], out int height))
        throw new BoardValidationException("You should enter two numbers separated by space.");

    return new Coordinates { X = width, Y = height };
}

int WaitForInt()
{
    string[] parts = ReadInputParts();

    if (parts.Length != 1)
        throw new BoardValidationException("You should enter one number.");

    if (!int.TryParse(parts[0], out int value))
        throw new BoardValidationException("You should enter a number.");

    return value;
}

string[] ReadInputParts()
    => (Console.ReadLine() ?? string.Empty).Trim().Split();