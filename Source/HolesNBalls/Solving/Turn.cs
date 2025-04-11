namespace HolesNBalls.Solving;

public class Turn
{
    public int Number { get; }
    public Board Board { get; }
    public BoardState State { get; }
    public Direction? Direction { get; }
    public Turn? Previous { get; }

    public Turn(int number, Board board, BoardState state, Direction? direction = null, Turn? previous = null)
    {
        Number = number;
        Board = board;
        State = state;
        Direction = direction;
        Previous = previous;
    }

    public Direction[] GetMoveSequence()
    {
        List<Direction> sequence = new();

        Turn? currentTurn = this;

        while (currentTurn != null)
        {
            if (currentTurn.Direction.HasValue)
                sequence.Add(currentTurn.Direction.Value);

            currentTurn = currentTurn.Previous;
        }

        sequence.Reverse();
        return sequence.ToArray();
    }
}