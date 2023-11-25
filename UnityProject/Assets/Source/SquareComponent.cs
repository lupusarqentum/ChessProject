using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SquareComponent : MonoBehaviour
{
    [SerializeField] private GameObject _pointPrefab;
    [SerializeField] private float _pointSizePercentage;
    private SpriteRenderer _spriteRenderer;
    private SquareColorSet _colors;
    private GameObject _point;

    [SerializeField] private bool _isWhite = true;

    [field: SerializeField] public bool HavePoint { get; set; }
    [field: SerializeField] public bool JustMoved { get; set; }
    [field: SerializeField] public bool Selected { get; set; }
    [field: SerializeField] public bool Hovered { get; set; }

    public void SetColorWhite()
    {
        _isWhite = true;
        UpdateView();
    }

    public void SetColorBlack()
    {
        _isWhite = false;
        UpdateView();
    }

#if UNITY_EDITOR
    private void FixedUpdate() => UpdateView();
#endif

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateView();
    }

    public void UpdateView()
    {
        _colors = _isWhite ? SquareColorSet.White : SquareColorSet.Black;
        if (JustMoved) _spriteRenderer.color = _colors.JustMoved;
        else if (Selected) _spriteRenderer.color = _colors.Selected;
        else if (Hovered) _spriteRenderer.color = _colors.Hovered;
        else _spriteRenderer.color = _colors.Normal;
        if (_point != null) Destroy(_point);
        if (HavePoint && Hovered == false)
        {
            Vector3 myPos = transform.position;
            _point = Instantiate(_pointPrefab);
            _point.transform.position = new Vector3(myPos.x, myPos.y, _point.transform.position.z);
            SpriteRenderer pointRenderer = _point.GetComponent<SpriteRenderer>();
            pointRenderer.color = _colors.Point;
            float xCurrentPercentage = pointRenderer.bounds.size.x / _spriteRenderer.bounds.size.x;
            float yCurrentPercentage = pointRenderer.bounds.size.y / _spriteRenderer.bounds.size.y;
            float xFactor = _pointSizePercentage / xCurrentPercentage;
            float yFactor = _pointSizePercentage / yCurrentPercentage;
            Vector3 pointScale = _point.transform.localScale;
            _point.transform.localScale = new Vector3(pointScale.x * xFactor, pointScale.y * yFactor, pointScale.z);
        }
    }

    public bool ContainsPointInWorld(Vector2 point)
    {
        Bounds bounds = _spriteRenderer.bounds;
        return bounds.Contains(new Vector3(point.x, point.y, bounds.center.z));
    }

    public void ResetUI()
    {
        HavePoint = false;
        JustMoved = false;
        Selected = false;
        Hovered = false;
    }

    private class SquareColorSet
    {
        public static SquareColorSet White { get; private set; }
        public static SquareColorSet Black { get; private set; }

        static SquareColorSet()
        {
            Color blackNormal = new Color32(121, 85, 61, 255);
            Color darkGreen = Color.Lerp(Color.green, blackNormal, 0.33f);
            Color whiteNormal = Color.Lerp(
                Color.Lerp(blackNormal, Color.white, 0.70f), Color.red, 0.08f);
            White = new SquareColorSet(whiteNormal, Color.Lerp(whiteNormal, Color.yellow, 0.45f),
                Color.Lerp(whiteNormal, Color.green, 0.5f), Color.Lerp(whiteNormal, Color.green, 0.25f),
                Color.Lerp(Color.Lerp(Color.green, blackNormal, 0.33f), Color.white, 0.3f));
            Black = new SquareColorSet(blackNormal, Color.Lerp(blackNormal, Color.yellow, 0.33f),
                Color.Lerp(blackNormal, darkGreen, 0.25f), Color.Lerp(blackNormal, darkGreen, 0.4f),
                Color.Lerp(darkGreen, whiteNormal, 0.5f));
        }

        public readonly Color Normal;
        public readonly Color JustMoved;
        public readonly Color Selected;
        public readonly Color Hovered;
        public readonly Color Point;

        public SquareColorSet(Color normal, Color justMoved, Color selected, Color hovered, Color point)
        {
            Normal = normal;
            JustMoved = justMoved;
            Selected = selected;
            Hovered = hovered;
            Point = point;
        }
    }
}