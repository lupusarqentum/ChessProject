using System.Collections.Generic;
using UnityEngine;
using ChessRules;
using ChessRules.Variants.CapablancaChess;

public class PieceInfoProvider : IPieceInfoProvider
{
    private readonly IChessGame _game;

    public PieceInfoProvider(IChessGame game)
    {
        _game = game;
    }

    public PieceUIInfo GetDefault()
    {
        return new PieceUIInfo();
    }

    public IEnumerable<(Square, PieceUIInfo)> GetAllPiecesOnBoard()
    {
        foreach (var p in _game.GetBoardView())
        {
            if (p.Item2.IsEmpty) continue;
            Texture2D texture;
            if (p.Item2.Rules is PawnRules && p.Item2.Color == ChessRules.Color.White)
                texture = TextureHolder.Get().WhitePawn;
            else if (p.Item2.Rules is PawnRules && p.Item2.Color == ChessRules.Color.Black)
                texture = TextureHolder.Get().BlackPawn;
            else if (p.Item2.Rules is RookRules && p.Item2.Color == ChessRules.Color.White)
                texture = TextureHolder.Get().WhiteRook;
            else if (p.Item2.Rules is RookRules && p.Item2.Color == ChessRules.Color.Black)
                texture = TextureHolder.Get().BlackRook;
            else if (p.Item2.Rules is KnightRules && p.Item2.Color == ChessRules.Color.White)
                texture = TextureHolder.Get().WhiteKnight;
            else if (p.Item2.Rules is KnightRules && p.Item2.Color == ChessRules.Color.Black)
                texture = TextureHolder.Get().BlackKnight;
            else if (p.Item2.Rules is BishopRules && p.Item2.Color == ChessRules.Color.White)
                texture = TextureHolder.Get().WhiteBishop;
            else if (p.Item2.Rules is BishopRules && p.Item2.Color == ChessRules.Color.Black)
                texture = TextureHolder.Get().BlackBishop;
            else if (p.Item2.Rules is QueenRules && p.Item2.Color == ChessRules.Color.White)
                texture = TextureHolder.Get().WhiteQueen;
            else if (p.Item2.Rules is QueenRules && p.Item2.Color == ChessRules.Color.Black)
                texture = TextureHolder.Get().BlackQueen;
            else if (p.Item2.Rules is KingRules && p.Item2.Color == ChessRules.Color.White)
                texture = TextureHolder.Get().WhiteKing;
            else if (p.Item2.Rules is KingRules && p.Item2.Color == ChessRules.Color.Black)
                texture = TextureHolder.Get().BlackKing;
            else if (p.Item2.Rules is ArchbishopRules && p.Item2.Color == ChessRules.Color.White)
                texture = TextureHolder.Get().WhiteArchbishop;
            else if (p.Item2.Rules is ArchbishopRules && p.Item2.Color == ChessRules.Color.Black)
                texture = TextureHolder.Get().BlackArchbishop;
            else if (p.Item2.Rules is ChancellorRules && p.Item2.Color == ChessRules.Color.White)
                texture = TextureHolder.Get().WhiteChancellor;
            else if (p.Item2.Rules is ChancellorRules && p.Item2.Color == ChessRules.Color.Black)
                texture = TextureHolder.Get().BlackChancellor;
            else
                texture = TextureHolder.Get().RejectingCross;
            MovingAnimationType animationType = MovingAnimationType.Rider;
            PieceUIInfo uiInfo = new PieceUIInfo(texture, animationType, 250f, p.Item2.Color == ChessRules.Color.Black);
            yield return (p.Item1, uiInfo);
        }
    }
}