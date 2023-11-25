namespace ChessRules
{
    public interface IMove
    {
        IBoard ApplyMove(IBoard board);
        string GetNotationName();
    }
}
