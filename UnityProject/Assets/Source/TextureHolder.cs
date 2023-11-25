using UnityEngine;

public class TextureHolder : MonoBehaviour
{
    private static readonly string GameObjectName = "TextureHolder";

    [SerializeField] private Texture2D _whitePawn;
    [SerializeField] private Texture2D _whiteKing;
    [SerializeField] private Texture2D _whiteRook;
    [SerializeField] private Texture2D _whiteQueen;
    [SerializeField] private Texture2D _whiteKnight;
    [SerializeField] private Texture2D _whiteBishop;
    [SerializeField] private Texture2D _whiteArchbishop;
    [SerializeField] private Texture2D _whiteChancellor;
    [SerializeField] private Texture2D _blackPawn;
    [SerializeField] private Texture2D _blackKing;
    [SerializeField] private Texture2D _blackRook;
    [SerializeField] private Texture2D _blackQueen;
    [SerializeField] private Texture2D _blackKnight;
    [SerializeField] private Texture2D _blackBishop;
    [SerializeField] private Texture2D _blackChancellor;
    [SerializeField] private Texture2D _rejectingCross;

    private Texture2D _blackArchbishop = null;

    public Texture2D WhitePawn => _whitePawn;
    public Texture2D WhiteKing => _whiteKing;
    public Texture2D WhiteRook => _whiteRook;
    public Texture2D WhiteQueen => _whiteQueen;
    public Texture2D WhiteKnight => _whiteKnight;
    public Texture2D WhiteBishop => _whiteBishop;
    public Texture2D WhiteArchbishop => _whiteArchbishop;
    public Texture2D WhiteChancellor => _whiteChancellor;
    public Texture2D BlackPawn => _blackPawn;
    public Texture2D BlackKing => _blackKing;
    public Texture2D BlackRook => _blackRook;
    public Texture2D BlackQueen => _blackQueen;
    public Texture2D BlackKnight => _blackKnight;
    public Texture2D BlackBishop => _blackBishop;
    public Texture2D BlackArchbishop
    {
        get
        {
            if (_blackArchbishop == null) _blackArchbishop = GetNegativeTexture(_whiteArchbishop);
            return _blackArchbishop;
        }
    }
    public Texture2D BlackChancellor => _blackChancellor;
    public Texture2D RejectingCross => _rejectingCross;

    private void Start()
    {
        gameObject.name = GameObjectName;
    }

    public static TextureHolder Get()
    {
        return GameObject.Find(GameObjectName).GetComponent<TextureHolder>();
    }

    public static Sprite GetSpriteFromPieceTexture(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), Vector2.one / 2);
    }

    private Texture2D GetNegativeTexture(Texture2D originalTexture)
    {
        int width = originalTexture.width;
        int height = originalTexture.height;
        Texture2D negativeTexture = new Texture2D(width, height);
        Color[] originalPixels = originalTexture.GetPixels();
        Color[] negativePixels = new Color[originalPixels.Length];
        for (int i = 0; i < originalPixels.Length; i++)
        {
            Color originalPixel = originalPixels[i];
            Color negativePixel = new Color(1 - originalPixel.r, 1 - originalPixel.g, 1 - originalPixel.b, originalPixel.a);
            negativePixels[i] = negativePixel;
        }
        negativeTexture.SetPixels(negativePixels);
        negativeTexture.Apply();
        return negativeTexture;
    }
}