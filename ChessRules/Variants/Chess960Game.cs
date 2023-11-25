using System;
using System.Collections.Generic;

namespace ChessRules.Variants
{
    [ChessVariant("Chess960")]
    public sealed class Chess960Game : ClassicChessGame
    {
        private readonly static Random Random = new Random();
        
        protected override void SetupPieces()
        {
            base.SetupPieces();
            void SetPiece(int file, PieceRules rules)
            {
                _board.SetPieceAt(new Piece(rules, Color.White), new Square(file, 0));
                _board.SetPieceAt(new Piece(rules, Color.Black), new Square(file, 7));
            }
            int firstBishopFile = Random.Next(0, 4) * 2;
            int secondBishopFile = Random.Next(0, 4) * 2 + 1;
            SetPiece(firstBishopFile, new BishopRules());
            SetPiece(secondBishopFile, new BishopRules());
            List<int> filesLeft = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
            filesLeft.Remove(firstBishopFile);
            filesLeft.Remove(secondBishopFile);
            int nextFile;
            nextFile = filesLeft[Random.Next(filesLeft.Count)];
            filesLeft.Remove(nextFile);
            SetPiece(nextFile, new QueenRules());
            nextFile = filesLeft[Random.Next(filesLeft.Count)];
            filesLeft.Remove(nextFile);
            SetPiece(nextFile, new KnightRules());
            nextFile = filesLeft[Random.Next(filesLeft.Count)];
            filesLeft.Remove(nextFile);
            SetPiece(nextFile, new KnightRules());
            SetPiece(filesLeft[0], new RookRules());
            SetPiece(filesLeft[2], new RookRules());
            SetPiece(filesLeft[1], new KingRules());
        }
    }
}
