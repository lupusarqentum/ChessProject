using System.Collections.Generic;

namespace ChessRules
{
    public abstract class PieceRules
    {
        public abstract IEnumerable<IMove> GetPseudoLegalMoves(IBoard board, Square from);
        public virtual void Setup(Piece piece) { }
        public virtual void AfterMoveApply(IBoard board, Square position, Piece piece, IMove move) { }
    }
}
