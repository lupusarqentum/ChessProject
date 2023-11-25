using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessRules
{
    public class KingRules : PieceRules
    {
        private const object hasmoved = null;

        public override void Setup(Piece piece)
        {
            piece._additionalInformation[nameof(hasmoved)] = false;
        }

        public override void AfterMoveApply(IBoard board, Square position, Piece piece, IMove move)
        {
            if (move.GetNotationName().EndsWith(position.ToString()))
            {
                piece._additionalInformation[nameof(hasmoved)] = true;
            }
        }

        public override IEnumerable<IMove> GetPseudoLegalMoves(IBoard board, Square kingPos)
        {
            for (int offsetX = -1; offsetX <= 1; ++offsetX)
            {
                for (int offsetY = -1; offsetY <= 1; ++offsetY)
                {
                    if (offsetX == 0 && offsetY == 0)
                    {
                        continue;
                    }
                    IEnumerable<IMove> m = StandardMovesGenerationHelper.GetPseudoLegalMoves(board, kingPos, StandardMoveType.Leaper, new Square(offsetX, offsetY));
                    if (m.Count() > 0)
                        yield return m.First();
                }
            }
            Piece thisPiece = board.GetPieceAt(kingPos);
            Color thisColor = thisPiece.Color;
            if ((bool)thisPiece._additionalInformation[nameof(hasmoved)])
            {
                yield break;
            }
            foreach (var move in GetCastlingMoves(board, kingPos, thisPiece, -1, 
                GetQueensideCastlingKingPosition(thisColor), GetQueensideCastlingRookPosition(thisColor)))
            {
                yield return move;
            }
            foreach (var move in GetCastlingMoves(board, kingPos, thisPiece, 1,
                GetKingsideCastlingKingPosition(thisColor), GetKingsideCastlingRookPosition(thisColor)))
            {
                yield return move;
            }
        }

        protected virtual Square GetQueensideCastlingKingPosition(Color thisColor)
        {
            return new Square("c" + (thisColor == Color.White ? "1" : "8"));
        }

        protected virtual Square GetQueensideCastlingRookPosition(Color thisColor)
        {
            return new Square("d" + (thisColor == Color.White ? "1" : "8"));
        }

        protected virtual Square GetKingsideCastlingKingPosition(Color thisColor)
        {
            return new Square("g" + (thisColor == Color.White ? "1" : "8"));
        }

        protected virtual Square GetKingsideCastlingRookPosition(Color thisColor)
        {
            return new Square("f" + (thisColor == Color.White ? "1" : "8"));
        }

        private IEnumerable<IMove> GetCastlingMoves(IBoard board, Square kingPos, Piece thisPiece, int offsetX, Square kingFinalPosition, Square rookFinalPosition)
        {
            Square current = kingPos;
            Square targetRook = kingPos;
            while (board.TryShift(current, new Square(offsetX, 0), out current))
            {
                Piece currentPiece = board.GetPieceAt(current);
                if (!(currentPiece.Rules is RookRules) ||
                    ((bool)board.GetPieceAt(current)._additionalInformation[nameof(hasmoved)]))
                {
                    if (currentPiece.IsEmpty) continue;
                    yield break;
                }
                targetRook = current;
                break;
            }
            int minX = Math.Min(Math.Min(kingPos.X, kingFinalPosition.X), Math.Min(targetRook.X, rookFinalPosition.X));
            int maxX = Math.Max(Math.Max(kingPos.X, kingFinalPosition.X), Math.Max(targetRook.X, rookFinalPosition.X));
            for (int x = minX; x <= maxX; ++x)
            {
                current = new Square(x, kingPos.Y);
                if (current == kingPos || current == kingFinalPosition 
                    || current == targetRook || current == rookFinalPosition) continue;
                if (board.GetPieceAt(current).IsEmpty == false) yield break;
            }
            BoardChange bc1 = new BoardChange(kingPos, Piece.CreateEmpty());
            BoardChange bc2 = new BoardChange(targetRook, Piece.CreateEmpty());
            BoardChange bc3 = new BoardChange(kingFinalPosition, thisPiece.GetClone());
            BoardChange bc4 = new BoardChange(rookFinalPosition, board.GetPieceAt(targetRook).GetClone());
            yield return new CommonMove(kingPos.ToString() + targetRook.ToString(), bc1, bc2, bc3, bc4);
        }
    }
}
