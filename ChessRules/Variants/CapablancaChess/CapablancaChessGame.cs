namespace ChessRules.Variants.CapablancaChess
{
    [ChessVariant("CapablancaChess")]
    public class CapablancaChessGame : ClassicChessGame
    {
        public override int GetMaxWidth() => 10;

        protected override void SetupPieces()
        {
            void SetPiece(int file, PieceRules rules)
            {
                _board.SetPieceAt(new Piece(rules, Color.White), new Square(file, 0));
                _board.SetPieceAt(new Piece(rules, Color.Black), new Square(file, 7));
            }
            SetPiece(0, new RookRules());
            SetPiece(1, new KnightRules());
            SetPiece(2, new ArchbishopRules());
            SetPiece(3, new BishopRules());
            SetPiece(4, new QueenRules());
            SetPiece(5, new KingRules());
            SetPiece(6, new BishopRules());
            SetPiece(7, new ChancellorRules());
            SetPiece(8, new KnightRules());
            SetPiece(9, new RookRules());
            for (int file = 0; file <= 9; ++file)
            {
                _board.SetPieceAt(new Piece(new PawnRules(), Color.White), new Square(file, 1));
                _board.SetPieceAt(new Piece(new PawnRules(), Color.Black), new Square(file, 6));
            }
        }
    }
}
