using System.Collections.Generic;
using System.Linq;

namespace ChessRules
{
    public sealed class EmptyPieceRules : PieceRules
    {
        public override IEnumerable<IMove> GetPseudoLegalMoves(IBoard board, Square from)
        {
            return Enumerable.Empty<IMove>();
        }
    }
}
