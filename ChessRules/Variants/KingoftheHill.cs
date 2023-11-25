using System.Collections.Generic;

namespace ChessRules.Variants
{
    [ChessVariant("KingoftheHill")]
    public class KingoftheHill : ClassicChessGame
    {
        private Color kingOfTheHill = Color.None;

        public override void ApplyMove(IMove move)
        {
            base.ApplyMove(move);
            if (_board.GetPieceAt(new Square("d4")).Rules is KingRules)
                kingOfTheHill = _board.GetPieceAt(new Square("d4")).Color;
            else if (_board.GetPieceAt(new Square("d5")).Rules is KingRules)
                kingOfTheHill = _board.GetPieceAt(new Square("d5")).Color;
            else if (_board.GetPieceAt(new Square("e4")).Rules is KingRules)
                kingOfTheHill = _board.GetPieceAt(new Square("e4")).Color;
            else if (_board.GetPieceAt(new Square("e5")).Rules is KingRules)
                kingOfTheHill = _board.GetPieceAt(new Square("e5")).Color;
        }

        public override IEnumerable<IMove> GetAvailableMoves()
        {
            if (kingOfTheHill != Color.None) yield break;
            foreach (var move in base.GetAvailableMoves()) yield return move;
        }

        public override GameStatus GetGameStatus()
        {
            if (kingOfTheHill != Color.None) return GameStatus.Checkmate;
            return base.GetGameStatus();
        }

        public override bool IsKingUnderCheck(Color kingsColor)
        {
            if (kingOfTheHill == FlipColor(kingsColor)) return true;
            if (kingOfTheHill != Color.None) return false;
            return base.IsKingUnderCheck(kingsColor);
        }
    }
}
