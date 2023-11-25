using System.Collections.Generic;
using System.Linq;

namespace ChessRules
{
    public class BishopRules : PieceRules
    {
        public override IEnumerable<IMove> GetPseudoLegalMoves(IBoard board, Square from)
        {
            return StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Rider, new Square(1, 1))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Rider, new Square(-1, -1)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Rider, new Square(1, -1)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Rider, new Square(-1, 1)));
        }
    }
}
