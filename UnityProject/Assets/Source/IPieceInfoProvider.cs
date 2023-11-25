using ChessRules;
using System.Collections.Generic;

public interface IPieceInfoProvider
{
    PieceUIInfo GetDefault();
    IEnumerable<(Square, PieceUIInfo)> GetAllPiecesOnBoard();
}