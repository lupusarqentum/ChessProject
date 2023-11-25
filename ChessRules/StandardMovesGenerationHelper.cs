using System.Collections.Generic;

namespace ChessRules
{
    public static class StandardMovesGenerationHelper
    {
        public static IEnumerable<IMove> GetPseudoLegalMoves(IBoard board, Square from, StandardMoveType type, Square delta)
        {
            switch (type)
            {
                case StandardMoveType.Rider:
                    Square current = from;
                    Color sourceColor = board.GetPieceAt(from).Color;
                    while (board.TryShift(current, delta, out current))
                    {
                        Piece thisPiece = board.GetPieceAt(current);
                        if (thisPiece.IsEmpty == false && thisPiece.Color == sourceColor) break;
                        yield return CommonMove.Of(from, current, board);
                        if (thisPiece.IsEmpty == false) break;
                    }
                    break;
                case StandardMoveType.Leaper:
                    if (board.TryShift(from, delta, out Square next))
                    {
                        if (board.GetPieceAt(next).IsEmpty || board.GetPieceAt(next).Color != board.GetPieceAt(from).Color)
                        {
                            yield return CommonMove.Of(from, next, board);
                        }
                    }
                    break;
            }
        }
    }
}
