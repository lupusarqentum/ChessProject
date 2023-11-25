namespace ChessRules.Variants.CylindricalChess
{
    [ChessVariant("CylindricalChess")]
    public class CylindricalChessGame : ClassicChessGame
    {
        public CylindricalChessGame()
        {
            _board = new CylindricalBoard(GetMaxWidth(), GetMaxHeight());
            SetupPieces();
        }
    }
}
