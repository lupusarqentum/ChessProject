namespace ChessRules
{
    public struct BoardChange
    {
        public readonly Square Square;
        public readonly Piece NewPiece;

        public BoardChange(Square Square, Piece NewPiece)
        {
            this.Square = Square;
            this.NewPiece = NewPiece;
        }

        public void Apply(IBoard target)
        {
            target.SetPieceAt(NewPiece, Square);
        }
    }
}