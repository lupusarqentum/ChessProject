using System.Collections.Generic;
using System.Linq;

namespace ChessRules
{
    public class RookRules : PieceRules
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

        public override IEnumerable<IMove> GetPseudoLegalMoves(IBoard board, Square from)
        {
            return StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Rider, new Square(0, 1))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Rider, new Square(0, -1)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Rider, new Square(1, 0)))
                .Concat(StandardMovesGenerationHelper.GetPseudoLegalMoves(board, from, StandardMoveType.Rider, new Square(-1, 0)));
        }
    }
}
