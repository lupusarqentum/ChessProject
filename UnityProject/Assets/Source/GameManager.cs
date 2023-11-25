using UnityEngine;
using UnityEngine.UI;
using ChessRules;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CrossInput _crossInput;
    [SerializeField] private SquaresLoader _squaresLoader;
    [SerializeField] private PiecesSetup _piecesSetup;
    [SerializeField] private MovesRegistrator _movesRegistrator;
    [SerializeField] private Text _gameStatusLabel;

    private void Start()
    {
        _movesRegistrator.SetInput(_crossInput);
        _movesRegistrator.SetGameManager(this);
        _squaresLoader.SetupSquares(_movesRegistrator);
        _piecesSetup.RefreshPieces();
    }

    public void Refresh()
    {
        _piecesSetup.RefreshPieces();
        _movesRegistrator.Reset();
        if (_gameStatusLabel != null)
        {
            string label = GetGameStatusLabelText();
            _gameStatusLabel.text = label;
        }
    }

    private static string GetGameStatusLabelText()
    {
        string label = "";
        var gameStatus = GameInfo.Instance.GetGameStatusAndWinner();
        if (gameStatus.Item2 != ChessRules.Color.None)
        {
            if (gameStatus.Item2 == ChessRules.Color.White)
            {
                label = "Победа белых";
            }
            else
            {
                label = "Победа чёрных";
            }
        }
        else if (gameStatus.Item1 == GameStatus.Stalemate)
        {
            label = "Пат";
        }

        return label;
    }

    public void AcceptMove(string move)
    {
        GameInfo.Instance.TryApplyMove(move);
        Refresh();
    }
}