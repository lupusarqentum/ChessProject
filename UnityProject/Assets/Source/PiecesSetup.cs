using System.Collections.Generic;
using UnityEngine;

public class PiecesSetup : MonoBehaviour
{
    [SerializeField] private GameObject _piecePrefab;

    private static Dictionary<string, PieceComponent> piecesSetuped = new Dictionary<string, PieceComponent>();
    
    public void RefreshPieces()
    {
        foreach (var x in piecesSetuped)
        {
            Destroy(x.Value.gameObject);
        }
        piecesSetuped.Clear();
        piecesSetuped = new Dictionary<string, PieceComponent>();
        foreach (var x in GameInfo.Instance.PieceInfoProvider.GetAllPiecesOnBoard())
        {
            SquareComponent square = GameObject.Find(x.Item1.ToString()).GetComponent<SquareComponent>();
            PieceUIInfo info = x.Item2;
            PieceComponent piece = Instantiate(_piecePrefab).GetComponent<PieceComponent>();
            piece.Setup(square, info);
            piecesSetuped.Add(square.gameObject.name, piece);
        }
    }

    public PieceComponent GetPieceOnOrNullBySquareName(string squareName)
    {
        if (piecesSetuped.ContainsKey(squareName))
        {
            return piecesSetuped[squareName];
        }
        return null;
    }
}