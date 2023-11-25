namespace ChessRules.Variants.CylindricalChess
{
    public class CylindricalBoard : StandardBoard
    {
        public CylindricalBoard(int width, int height) : base(width, height) { }

        public override bool TryShift(Square position, Square delta, out Square result)
        {
            Square shifted = new Square(((position.X + delta.X) % MaxWidth + MaxWidth) % MaxWidth, position.Y + delta.Y);
            if (IsOnBoard(shifted))
            {
                result = shifted;
                return true;
            }
            result = position;
            return false;
        }
    }
}
