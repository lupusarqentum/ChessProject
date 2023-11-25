using System.Collections.Generic;

namespace ChessRules
{
    public interface IBoard
    {
        bool IsOnBoard(Square square);
        Piece GetPieceAt(Square from);
        void SetPieceAt(Piece piece, Square to);
        bool TryShift(Square position, Square delta, out Square result);
        IBoard GetClone();
        IEnumerable<(Square, Piece)> IterateOverBoard();
        int MaxWidth { get; }
        int MaxHeight { get; }
    }
}
