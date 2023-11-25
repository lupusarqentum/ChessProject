using System.Collections.Generic;

namespace ChessRules
{
    public class PawnRules : PieceRules
    {
        private const object hasmoved = null;
        private const object canbecapturedenpassant = null;

        public override void Setup(Piece piece)
        {
            piece._additionalInformation[nameof(hasmoved)] = false;
            piece._additionalInformation[nameof(canbecapturedenpassant)] = false;
        }

        public override void AfterMoveApply(IBoard board, Square position, Piece piece, IMove move)
        {
            if (move.GetNotationName().EndsWith(position.ToString()))
            {
                piece._additionalInformation[nameof(hasmoved)] = true;
            } 
            else
            {
                piece._additionalInformation[nameof(canbecapturedenpassant)] = false;
            }
        }

        public override IEnumerable<IMove> GetPseudoLegalMoves(IBoard board, Square from)
        {
            Piece thisPiece = board.GetPieceAt(from);
            Color thisColor = thisPiece.Color;
            bool hasMoved = (bool)thisPiece._additionalInformation[nameof(hasmoved)];
            int direction = GetDirectionForColor(thisColor);
            if (board.TryShift(from, new Square(0, direction), out Square next)
                && board.GetPieceAt(next).IsEmpty)
            {
                foreach (var m in AddMoves(board, from, next))
                {
                    yield return m;
                }
                if (hasMoved == false &&
                    board.TryShift(next, new Square(0, direction), out next) &&
                    board.GetPieceAt(next).IsEmpty)
                {
                    BoardChange bc1 = new BoardChange(from, Piece.CreateEmpty());
                    Piece clonedPiece = thisPiece.GetClone();
                    clonedPiece._additionalInformation[nameof(canbecapturedenpassant)] = true;
                    BoardChange bc2 = new BoardChange(next, clonedPiece);
                    yield return new CommonMove(from.ToString() + next.ToString(), bc1, bc2);
                }
            }
            foreach (int offsetX in new int[2] { -1, 1})
            {
                if (board.TryShift(from, new Square(offsetX, direction), out Square attacked))
                {
                    if (thisPiece.CanCapture(board.GetPieceAt(attacked)))
                    {
                        foreach (var m in AddMoves(board, from, attacked))
                        {
                            yield return m;
                        }
                    }
                    if (board.TryShift(attacked, new Square(0, -direction), out Square enemyPawnSquare))
                    {
                        Piece targetPiece = board.GetPieceAt(enemyPawnSquare);
                        if (thisPiece.CanCapture(targetPiece) && targetPiece.Rules is PawnRules
                            && (bool)targetPiece._additionalInformation[nameof(canbecapturedenpassant)])
                        {
                            BoardChange bc1 = new BoardChange(enemyPawnSquare, Piece.CreateEmpty());
                            BoardChange bc2 = new BoardChange(from, Piece.CreateEmpty());
                            BoardChange bc3 = new BoardChange(attacked, thisPiece.GetClone());
                            yield return new CommonMove(from.ToString() + attacked.ToString(), bc1, bc2, bc3);
                        }
                    }
                }
            }
        }

        private IEnumerable<IMove> AddMoves(IBoard board, Square from, Square to)
        {
            if (IsValidSquareToPromote(to) == false)
            {
                yield return CommonMove.Of(from, to, board);
                yield break;
            }
            Color thisColor = board.GetPieceAt(from).Color;
            foreach (PieceRules promotionOption in GetPromotionOptions())
            {
                yield return new CommonMove(from.ToString() + to.ToString() + promotionOption.GetType().Name, 
                    new BoardChange(from, Piece.CreateEmpty()), new BoardChange(to, new Piece(promotionOption, thisColor)));
            }
        }

        protected virtual int GetDirectionForColor(Color color)
        {
            return color == Color.White ? 1 : -1;
        }

        protected virtual bool IsValidSquareToPromote(Square to)
        {
            return to.Y == 0 || to.Y == 7;
        }

        protected virtual PieceRules[] GetPromotionOptions()
        {
            return new PieceRules[] { new KnightRules(), new BishopRules(), new RookRules(), new QueenRules() };
        }
    }
}
