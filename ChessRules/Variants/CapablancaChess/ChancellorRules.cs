using System.Collections.Generic;
using System.Linq;

namespace ChessRules.Variants.CapablancaChess
{
    public class ChancellorRules : PieceRules
    {
        public override IEnumerable<IMove> GetPseudoLegalMoves(IBoard board, Square from)
        {
            return StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Rider, new Square(0, 1))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Rider, new Square(0, -1)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Rider, new Square(1, 0)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Rider, new Square(-1, 0)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Leaper, new Square(2, 1)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Leaper, new Square(2, -1)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Leaper, new Square(-2, 1)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Leaper, new Square(-2, -1)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Leaper, new Square(1, 2)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Leaper, new Square(1, -2)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Leaper, new Square(-1, 2)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Leaper, new Square(-1, -2)));
        }
    }
}
