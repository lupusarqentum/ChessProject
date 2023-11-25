using System.Collections.Generic;
using UnityEngine;

public class SquaresLoader : MonoBehaviour
{
    [SerializeField] private GameObject _squarePrefab;
    [SerializeField] private SpriteRenderer _sheet;
    [SerializeField] private float _maxWidthPercentage = 0.9f;
    [SerializeField] private float _maxHeightPercentage = 0.5f;

    private List<SquareComponent> _squares;

    public string GetSquareNameByPositionOrNull(Vector2 worldPos)
    {
        return GetSquareByPositionOrNull(worldPos)?.gameObject.name;
    }

    public SquareComponent GetSquareByPositionOrNull(Vector2 worldPos)
    {
        foreach (SquareComponent square in _squares)
        {
            if (square.ContainsPointInWorld(worldPos))
            {
                return square;
            }
        }
        return null;
    }


    public void SetupSquares(MovesRegistrator movesRegistrator)
    {
        bool fromWhitePerspective = GameInfo.Instance.IsBoardReversed == false;
        _squares = new List<SquareComponent>();
        int boardSquaresWidth = GameInfo.Instance.BoardWidth;
        int boardSquaresHeight = GameInfo.Instance.BoardHeight;
        SpriteRenderer sheetSpriteRenderer = _sheet.GetComponent<SpriteRenderer>();
        float sheetWidth = sheetSpriteRenderer.bounds.size.x;
        float sheetHeight = sheetSpriteRenderer.bounds.size.y;
        float squareSize = CalculateSquareSize(boardSquaresWidth, boardSquaresHeight, sheetWidth, sheetHeight);
        float boardWidth = boardSquaresWidth * squareSize;
        float boardHeight = boardSquaresHeight * squareSize;
        float yOffset = (sheetHeight - boardHeight + squareSize) / 2;
        float xOffset = (sheetWidth - boardWidth + squareSize) / 2;
        foreach (var sq in GameInfo.Instance.GetAllSquares())
        {
            int row = sq.Y;
            int file = sq.X;
            int rowToDraw = fromWhitePerspective ? row : GameInfo.Instance.BoardHeight - row - 1;
            int fileToDraw = fromWhitePerspective ? file : GameInfo.Instance.BoardWidth - file - 1;
            float x = _sheet.bounds.min.x + xOffset + fileToDraw * squareSize;
            float y = _sheet.bounds.min.y + yOffset + rowToDraw * squareSize;
            bool isCurrentSquareBlack = (file + row) % 2 == 0;
            InstantiateNewSquare(squareSize, new Vector2(x, y), isCurrentSquareBlack, sq.ToString(), movesRegistrator);
        }
    }

    private void InstantiateNewSquare(float squareSize, Vector2 position, bool isBlack, string name, MovesRegistrator movesRegistrator)
    {
        SquareComponent sq = Instantiate(_squarePrefab).GetComponent<SquareComponent>();
        SpriteRenderer renderer = sq.GetComponent<SpriteRenderer>();
        float scaleFactor = squareSize / renderer.size.x;
        Vector3 unadjustedScale = sq.transform.localScale;
        sq.transform.localScale = new Vector3(unadjustedScale.x * scaleFactor, unadjustedScale.y * scaleFactor, unadjustedScale.z);
        sq.transform.position = new Vector3(position.x, position.y, sq.transform.position.z);
        if (isBlack) sq.SetColorBlack();
        else sq.SetColorWhite();
        sq.gameObject.name = name;
        _squares.Add(sq);
        sq.GetComponent<SquareHighlighter>()?.SetRegistrator(movesRegistrator);
    }

    private float CalculateSquareSize(int boardSquaresWidth, int boardSquaresHeight, float sheetWidth, float sheetHeight)
    {
        float maxAllowedBoardWidth = _maxWidthPercentage * sheetWidth;
        float maxAllowedBoardHeight = _maxHeightPercentage * sheetHeight;
        float squareSize = Mathf.Min(maxAllowedBoardWidth / boardSquaresWidth, maxAllowedBoardHeight / boardSquaresHeight);
        return squareSize;
    }
}