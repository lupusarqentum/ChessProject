using System;
using System.Collections.Generic;

namespace ChessRules
{
    public class StandardBoard : IBoard
    {
        private Piece[,] _pieces;
        private readonly int _width;
        private readonly int _height;

        public StandardBoard(int width, int height)
        {
            _width = width;
            _height = height;
            _pieces = new Piece[width, height];
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    _pieces[i, j] = Piece.CreateEmpty();
                }
            }
        }

        public int MaxWidth => _width;
        public int MaxHeight => _height;

        public IBoard GetClone()
        {
            IBoard clone = GetType().GetConstructor(new Type[] { typeof(int), typeof(int) }).Invoke(new object[] { _width, _height }) as IBoard;
            foreach ((Square, Piece) t in IterateOverBoard())
            {
                clone.SetPieceAt(t.Item2.GetClone(), t.Item1);
            }
            return clone;
        }

        public Piece GetPieceAt(Square from)
        {
            return _pieces[from.X, from.Y];
        }

        public bool IsOnBoard(Square square)
        {
            return 0 <= square.X && square.X < _width &&
                   0 <= square.Y && square.Y < _height;
        }

        public void SetPieceAt(Piece piece, Square to)
        {
            _pieces[to.X, to.Y] = piece;
        }

        public virtual bool TryShift(Square position, Square delta, out Square result)
        {
            Square shifted = new Square(position.X + delta.X, position.Y + delta.Y);
            if (IsOnBoard(shifted))
            {
                result = shifted;
                return true;
            }
            result = position;
            return false;
        }

        public IEnumerable<(Square, Piece)> IterateOverBoard()
        {
            for (int i = 0; i < _width; ++i)
            {
                for (int j = 0; j < _height; ++j)
                {
                    Square square = new Square(i, j);
                    yield return (square, GetPieceAt(square));
                }
            }
        }
    }
}
