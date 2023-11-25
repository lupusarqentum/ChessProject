namespace ChessRules.Variants
{
    [ChessVariant("Horde")]
    public class Horde : ClassicChessGame
    {
        public Horde() : base()
        {
            _activeColor = Color.Black;
        }

        protected override void SetupPieces()
        {
            void SetPiece(int file, PieceRules rules)
            {
                _board.SetPieceAt(new Piece(rules, Color.Black), new Square(file, 7));
            }
            SetPiece(0, new RookRules());
            SetPiece(1, new KnightRules());
            SetPiece(2, new BishopRules());
            SetPiece(3, new QueenRules());
            SetPiece(4, new KingRules());
            SetPiece(5, new BishopRules());
            SetPiece(6, new KnightRules());
            SetPiece(7, new RookRules());
            for (int file = 0; file <= 7; ++file)
            {
                _board.SetPieceAt(new Piece(new PawnRules(), Color.Black), new Square(file, 6));
            }
            Piece pawn = new Piece(new PawnRules(), Color.White);
            pawn._additionalInformation["hasmoved"] = true;
            for (int row = 0; row <= 3; ++row)
            {
                for (int file = 0; file <= 7; ++file)
                {

                    _board.SetPieceAt(pawn.GetClone(), new Square(file, row));
                }
            }
        }

        protected override bool IsKingUnderCheckOnBoard(Color kingsColor, IBoard board)
        {
            if (kingsColor == Color.Black) return base.IsKingUnderCheckOnBoard(kingsColor, board);
            bool whitePawnExist = false;
            foreach (var p in GetBoardView())
            {
                if (p.Item2.Rules is PawnRules && p.Item2.Color == Color.White)
                {
                    whitePawnExist = true;
                    break;
                }
            }
            return whitePawnExist == false;
        }
    }
}
