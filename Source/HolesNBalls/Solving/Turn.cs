namespace HolesNBalls.Solving;

public class Turn
{
    public int Number { get; }
    public Board Board { get; }
    public BoardState State { get; }
    public MoveDirection? Direction { get; }
    public Turn? Previous { get; }

    public Turn(int number, Board board, BoardState state, MoveDirection? direction = null, Turn? previous = null)
    {
        Number = number;
        Board = board;
        State = state;
        Direction = direction;
        Previous = previous;
    }
}