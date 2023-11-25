using System;

namespace ChessRules
{
    public struct Square
    {
        public static readonly Square None = new Square(-1, -1);

        public int X { get; private set; }
        public int Y { get; private set; }

        public Square(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Square(string square)
        {
            if (square.Length != 2
                || char.IsLetter(square[0]) == false
                || char.IsDigit(square[1]) == false)
                throw new ArgumentException("Cannot parse square because it is invalid: " + square);
            X = char.ToLower(square[0]) - 'a';
            Y = square[1] - '1';
        }

        public static bool operator ==(Square a, Square b) => a.Equals(b);
        public static bool operator !=(Square a, Square b) => a.Equals(b) == false;

        public override bool Equals(object obj)
        {
            return obj is Square square && Equals(square);
        }

        public bool Equals(Square other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X * 1000 + Y;
        }

        public override string ToString()
        {
            return Equals(None) == false ? (char)(X + 'a') + ((char)(Y + '1')).ToString() : "-";
        }
    }
}
