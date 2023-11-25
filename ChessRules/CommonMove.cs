namespace ChessRules
{
    public sealed class CommonMove : IMove
    {
        private readonly BoardChange[] _boardChanges;
        private readonly string _name;

        public CommonMove(string name, params BoardChange[] boardChanges)
        {
            _boardChanges = boardChanges;
            _name = name;
        }

        public static CommonMove Of(Square from, Square to, IBoard board)
        {
            string name = from.ToString() + to.ToString();
            BoardChange bc1 = new BoardChange(from, Piece.CreateEmpty());
            BoardChange bc2 = new BoardChange(to, board.GetPieceAt(from).GetClone());
            return new CommonMove(name, bc1, bc2);
        }

        public IBoard ApplyMove(IBoard board)
        {
            IBoard newBoard = board.GetClone();
            foreach (BoardChange boardChange in _boardChanges)
            {
                boardChange.Apply(newBoard);
            }
            return newBoard;
        }

        public string GetNotationName()
        {
            return _name;
        }

        public override string ToString()
        {
            return GetNotationName();
        }
    }
}
