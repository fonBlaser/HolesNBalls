using HolesNBalls.Exceptions.Validation;

namespace HolesNBalls.Validation;

public class BoardValidator
{
    public void ValidateBoard(Board board)
    {
        ValidateDimensions(board.Width, board.Height);
        ValidateHolesPresence(board.Holes);
        ValidateHoleNumberUniqueness(board.Holes);
        ValidateHolesBounds(board);
        ValidateBallsPresence(board.Balls);
        ValidateBallNumberUniqueness(board.Balls);
        ValidateBallsBounds(board);
        ValidateEachBallHasHoleByNumber(board.Balls, board.Holes);
        ValidateHolesAndBallsPositionUniqueness(board.Holes, board.Balls);
    }

    public void ValidateDimensions(int width, int height)
    {
        if(width <= 0 || height <= 0)
            throw new BoardValidationException($"Board Width and Height should be positive. " +
                                               $"Currents are W:{width} and H:{height})");

        int cellsCount = width * height;
        if (cellsCount < 2)
            throw new BoardValidationException($"Board should have at least 2 cells (now {cellsCount}).");
    }

    public void ValidateHolesPresence(IEnumerable<Hole> holes)
    {
        if(!holes.Any())
            throw new BoardValidationException("Board should have at least one hole.");
    }

    public void ValidateHoleNumberUniqueness(IEnumerable<Hole> holes)
        => ValidateBoardObjectNumberUniqueness(holes, nameof(holes));

    public void ValidateHolesBounds(Board board)
        => ValidateBoardObjectBoundsInCollection(board.Width, board.Height, board.Holes, nameof(board.Holes));

    public void ValidateBallsPresence(IEnumerable<Ball> balls)
    {
        if (!balls.Any())
            throw new BoardValidationException("Board should have at least one ball.");
    }

    public void ValidateBallNumberUniqueness(IEnumerable<Ball> balls)
        => ValidateBoardObjectNumberUniqueness(balls, nameof(balls));

    public void ValidateBallsBounds(Board board)
        => ValidateBoardObjectBoundsInCollection(board.Width, board.Height, board.Balls, nameof(board.Balls));

    public void ValidateEachBallHasHoleByNumber(IEnumerable<Ball> balls, IEnumerable<Hole> holes)
    {
        List<Ball> ballsWithoutHoles = balls.Where(b => holes.All(h => h.Number != b.Number))
                                            .ToList();
       
        if (ballsWithoutHoles.Any())
        {
            string errorMessage = $"Each ball should have a hole. " +
                                  $"Balls without holes: {string.Join(',', ballsWithoutHoles)}";
            throw new BoardValidationException(errorMessage);
        }
    }

    public void ValidateHolesAndBallsPositionUniqueness(IEnumerable<Hole> holes, IEnumerable<Ball> balls)
    {
        List<BoardObject> nonUniqueObjects = holes.Concat<BoardObject>(balls)
                                                  .GroupBy(h => (h.X, h.Y))
                                                  .Where(g => g.Count() > 1)
                                                  .SelectMany(g => g)
                                                  .ToList();
        
        if (nonUniqueObjects.Any())
        {
            string errorMessage = $"Holes and Balls should have unique positions. " +
                                  $"Objects with non-unique positions: {string.Join(',', nonUniqueObjects)}";
            throw new BoardValidationException(errorMessage);
        }
    }

    private void ValidateBoardObjectNumberUniqueness(IEnumerable<BoardObject> collection, string collectionName)
    {
        List<int> nonUniqueNumbers = collection.GroupBy(h => h.Number)
                                               .Where(g => g.Count() > 1)
                                               .Select(g => g.Key)
                                               .ToList();

        if (nonUniqueNumbers.Any())
        {
            string errorMessage = $"Objects in '{collectionName}' collection should have unique numbers. " +
                                  $"Non-unique numbers: {string.Join(',', nonUniqueNumbers)}";
            throw new BoardValidationException(errorMessage);
        }
    }

    private void ValidateBoardObjectBoundsInCollection(int width, int height, IEnumerable<Coordinates> collection, string collectionName)
    {
        IReadOnlyCollection<Coordinates> outerCoords = collection.Where(coords => AreCoordsOutsideOfBoard(coords, width, height))
                                                                 .ToList();
            
        if (outerCoords.Any())
        {
            string errorMessage = $"{collectionName} must be within the board " +
                                  $"(X:[0,{width - 1}], Y:[0,{height - 1}]). " +
                                  $"Wrong ones: {string.Join(',', outerCoords)}";
            throw new BoardValidationException(errorMessage);
        }
    }

    public void ValidateCoordinatesBounds(Coordinates coordinates, int width, int height, string objectTypeName)
    {
        if (AreCoordsOutsideOfBoard(coordinates, width, height))
        {
            string errorMessage = $"{objectTypeName} coordinates must be within the board " +
                                  $"(X:[0,{width - 1}], Y:[0,{height - 1}]). " +
                                  $"Current: {coordinates}";
            throw new BoardValidationException(errorMessage);
        }
    }

    private bool AreCoordsOutsideOfBoard(Coordinates coordinates, int boardWidth, int boardHeight)
        => coordinates.X < 0
        || coordinates.X >= boardWidth
        || coordinates.Y < 0
        || coordinates.Y >= boardHeight;
}