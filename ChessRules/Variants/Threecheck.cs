using System.Collections.Generic;

namespace ChessRules.Variants
{
    [ChessVariant("Three-check")]
    public class Threecheck : ClassicChessGame
    {
        private int whiteChecks = 0;
        private int blackChecks = 0;

        public override void ApplyMove(IMove move)
        {
            base.ApplyMove(move);
            if (base.IsKingUnderCheck(_activeColor))
            {
                if (_activeColor == Color.White) whiteChecks++;
                else blackChecks++;
            }
        }

        public override IEnumerable<IMove> GetAvailableMoves()
        {
            if (whiteChecks >= 3 || blackChecks >= 3) yield break;
            foreach (var move in base.GetAvailableMoves()) yield return move;
        }

        public override GameStatus GetGameStatus()
        {
            if (whiteChecks >= 3 || blackChecks >= 3) return GameStatus.Checkmate;
            return base.GetGameStatus();
        }

        public override bool IsKingUnderCheck(Color kingsColor)
        {
            if (kingsColor == Color.White && whiteChecks >= 3) return true;
            if (kingsColor == Color.Black && blackChecks >= 3) return true;
            return base.IsKingUnderCheck(kingsColor);
        }
    }
}
