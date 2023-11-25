using System;
using System.Collections.Generic;
using ChessRules;

public class GameInfo
{
    public static GameInfo Instance = new GameInfo();

    private readonly IChessGame _game;
    private readonly Dictionary<string, bool> _shouldAskForPromotionWhenDoingMove;
    private readonly Dictionary<string, IMove> _moveByName;

    public IPieceInfoProvider PieceInfoProvider { get; private set; }
    public int BoardWidth => _game.GetMaxWidth();
    public int BoardHeight => _game.GetMaxHeight();
    public int MaxBoardSize => Math.Max(BoardWidth, BoardHeight);
    public bool IsBoardReversed => false;
    public bool ShouldFlipPieces => false;

    private GameInfo()
    {
        _game = ChessVariants.CreateGameByVariantName("Three-check");
        PieceInfoProvider = new PieceInfoProvider(_game);
        _shouldAskForPromotionWhenDoingMove = new Dictionary<string, bool>();
        _moveByName = new Dictionary<string, IMove>();
        OnGameStateChanged();
    }

    public IEnumerable<Square> GetAllSquares()
    {
        foreach (var x in _game.GetBoardView())
        {
            yield return x.Item1;
        }
    }

    public bool CanGoFromTo(string from, string to)
    {
        return _shouldAskForPromotionWhenDoingMove.ContainsKey(from + to);
    }

    private void OnGameStateChanged()
    {
        _shouldAskForPromotionWhenDoingMove.Clear();
        foreach (IMove move in _game.GetAvailableMoves())
        {
            string code = move.ToString();
            _shouldAskForPromotionWhenDoingMove[code.Substring(0, 4)] = code.Length > 4;
            _moveByName[code] = move;
        }
    }

    public DetailsOption[] GetMoveOptions(string from, string to)
    {
        string fromto = from + to;
        if (_shouldAskForPromotionWhenDoingMove.ContainsKey(fromto) && _shouldAskForPromotionWhenDoingMove[fromto])
        {
            if (_game.GetActiveColor() == Color.White)
            {
                return new DetailsOption[]
                {
                    new DetailsOption(nameof(QueenRules), TextureHolder.Get().WhiteQueen),
                    new DetailsOption(nameof(RookRules), TextureHolder.Get().WhiteRook),
                    new DetailsOption(nameof(BishopRules), TextureHolder.Get().WhiteBishop),
                    new DetailsOption(nameof(KnightRules), TextureHolder.Get().WhiteKnight),
                    new DetailsOption("", TextureHolder.Get().RejectingCross),
                };
            }
            else
            {
                return new DetailsOption[]
                {
                    new DetailsOption(nameof(QueenRules),  TextureHolder.Get().BlackQueen),
                    new DetailsOption(nameof(RookRules), TextureHolder.Get().BlackRook),
                    new DetailsOption(nameof(BishopRules), TextureHolder.Get().BlackBishop),
                    new DetailsOption(nameof(KnightRules), TextureHolder.Get().BlackKnight),
                    new DetailsOption("", TextureHolder.Get().RejectingCross),
                };
            }
        }
        else
        {
            return new DetailsOption[] { new DetailsOption("", TextureHolder.Get().RejectingCross) };
        }
    }

    public void TryApplyMove(string move)
    {
        _game.ApplyMove(_moveByName[move]);
        OnGameStateChanged();
    }

    public string GetMove(string from, string to, string details)
    {
        return from + to + details;
    }

    public (GameStatus, Color) GetGameStatusAndWinner()
    {
        GameStatus status = _game.GetGameStatus();
        if (status != GameStatus.Checkmate) return (status, Color.None);
        return (status, _game.IsKingUnderCheck(Color.White) ? Color.Black : Color.White);
    }
}
