using System.Collections.Generic;
using System.Linq;

namespace ChessRules.Variants
{
    [ChessVariant("Classic")]
    public class ClassicChessGame : IChessGame
    {
        protected IBoard _board;
        protected Color _activeColor = Color.White;

        public ClassicChessGame()
        {
            _board = new StandardBoard(GetMaxWidth(), GetMaxHeight());
            SetupPieces();
        }

        public virtual int GetMaxWidth() => 8;
        public virtual int GetMaxHeight() => 8;

        protected virtual void SetupPieces()
        {
            void SetPiece(int file, PieceRules rules)
            {
                _board.SetPieceAt(new Piece(rules, Color.White), new Square(file, 0));
                _board.SetPieceAt(new Piece(rules, Color.Black), new Square(file, 7));
            }
            SetPiece(0, new RookRules());
            SetPiece(1, new KnightRules());
            SetPiece(2, new BishopRules());
            SetPiece(3, new QueenRules());
            SetPiece(4, new KingRules());
            SetPiece(5, new BishopRules());
            SetPiece(6, new KnightRules());
            SetPiece(7, new RookRules());
            for (int file = 0; file <= 7; ++file)
            {
                _board.SetPieceAt(new Piece(new PawnRules(), Color.White), new Square(file, 1));
                _board.SetPieceAt(new Piece(new PawnRules(), Color.Black), new Square(file, 6));
            }
        }

        public virtual void ApplyMove(IMove move)
        {
            _board = move.ApplyMove(_board);
            _activeColor = FlipColor(_activeColor);
            foreach (var x in _board.IterateOverBoard())
            {
                x.Item2.Rules.AfterMoveApply(_board, x.Item1, x.Item2, move);
            }
        }

        public Color GetActiveColor()
        {
            return _activeColor;
        }

        public IEnumerable<(Square, Piece)> GetBoardView()
        {
            foreach ((Square, Piece) t in _board.IterateOverBoard())
            {
                yield return t;
            }
        }

        public virtual GameStatus GetGameStatus()
        {
            if (GetAvailableMoves().Count() != 0)
            {
                return GameStatus.Running;
            }
            if (IsKingUnderCheck(_activeColor))
            {
                return GameStatus.Checkmate;
            }
            return GameStatus.Stalemate;
        }

        public IEnumerable<Color> GetPlayingColors()
        {
            return new Color[2] { Color.White, Color.Black };
        }

        protected IEnumerable<IMove> GetPseudoLegalMoves(IBoard board, Color activeColor)
        {
            foreach ((Square, Piece) t in board.IterateOverBoard())
            {
                if (t.Item2.Color == activeColor)
                {
                    foreach (IMove move in t.Item2.Rules.GetPseudoLegalMoves(board, t.Item1))
                    {
                        yield return move;
                    }
                }
            }
        }

        public virtual IEnumerable<IMove> GetAvailableMoves()
        {
            foreach (IMove moveCandidate in GetPseudoLegalMoves(_board, _activeColor))
            {
                IBoard newBoard = moveCandidate.ApplyMove(_board);
                if (IsKingUnderCheckOnBoard(_activeColor, newBoard) == false)
                {
                    yield return moveCandidate;
                }
            }
        }

        protected virtual bool IsKingUnderCheckOnBoard(Color kingsColor, IBoard board)
        {
            foreach (IMove moveCandidate in GetPseudoLegalMoves(board, FlipColor(kingsColor)))
            {
                IBoard newBoard = moveCandidate.ApplyMove(board);
                bool kingSaved = false;
                foreach ((Square, Piece) t in newBoard.IterateOverBoard())
                {
                    if (t.Item2.Rules is KingRules && t.Item2.Color == kingsColor)
                    {
                        kingSaved = true;
                        break;
                    }
                }
                if (kingSaved == false) return true;
            }
            return false;
        }

        public virtual bool IsKingUnderCheck(Color kingsColor)
        {
            return IsKingUnderCheckOnBoard(kingsColor, _board);
        }

        protected Color FlipColor(Color current)
        {
            return current == Color.White ? Color.Black : Color.White;
        }
    }
}
