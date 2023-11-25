using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessRules.Variants
{
    [ChessVariant("Antichess")]
    public class Antichess : ClassicChessGame
    {
        public override IEnumerable<IMove> GetAvailableMoves()
        {
            int minimalPiecesCountAfterMove = int.MaxValue;
            IMove[] moveCandidates = GetPseudoLegalMoves(_board, _activeColor).ToArray();
            foreach (var moveCandidate in moveCandidates)
            {
                minimalPiecesCountAfterMove = Math.Min(minimalPiecesCountAfterMove, 
                    CountPiecesOnBoard(moveCandidate.ApplyMove(_board)));
            }
            foreach (var moveCandidate in moveCandidates)
            {
                if (CountPiecesOnBoard(moveCandidate.ApplyMove(_board)) == minimalPiecesCountAfterMove)
                {
                    yield return moveCandidate;
                }
            }
        }
        
        private int CountPiecesOnBoard(IBoard board)
        {
            int piecesCount = 0;
            foreach (var p in board.IterateOverBoard())
            {
                if (p.Item2.IsEmpty == false)
                {
                    piecesCount++;
                }
            }
            return piecesCount;
        }

        protected override bool IsKingUnderCheckOnBoard(Color kingsColor, IBoard board)
        {
            int piecesCount = 0;
            foreach (var p in GetBoardView())
            {
                if (p.Item2.IsEmpty == false && p.Item2.Color == kingsColor)
                {
                    piecesCount++;
                }
            }
            return piecesCount == 0;
        }
    }
}
