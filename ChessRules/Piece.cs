using System.Collections.Generic;

namespace ChessRules
{
    public sealed class Piece
    {
        public PieceRules Rules { get; private set; }
        public Color Color { get; private set; }
        internal Dictionary<string, object> _additionalInformation;

        public bool IsEmpty => Rules is EmptyPieceRules;

        public Piece(PieceRules rules, Color color, bool ignoreSetup = false)
        {
            Rules = rules;
            Color = color;
            _additionalInformation = new Dictionary<string, object>();
            if (ignoreSetup == false)
                Rules.Setup(this);
        }

        private Piece(PieceRules rules, Color color, Dictionary<string, object> additionalInformation)
            : this(rules, color, true)
        {
            foreach (KeyValuePair<string, object> info in additionalInformation)
            {
                _additionalInformation[info.Key] = info.Value;
            }
        }

        public bool CanCapture(Piece other)
        {
            return other.IsEmpty == false && Color != other.Color;
        }

        public static Piece CreateEmpty()
        {
            return new Piece(new EmptyPieceRules(), Color.None);
        }

        public Piece GetClone()
        {
            return new Piece(Rules, Color, _additionalInformation);
        }
    }
}
