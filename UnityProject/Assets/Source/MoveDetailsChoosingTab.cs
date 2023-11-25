using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveDetailsChoosingTab : MonoBehaviour
{
    public event Action DetailsChoosingRejected;
    public event Action<string> DetailsChoosen;

    [SerializeField] private GameObject _panelPrefab;
    [SerializeField] private GameObject _iconPrefab;
    [SerializeField] private CrossInput _input;
    [SerializeField] private SpriteRenderer _sheet;
    [SerializeField] [Range(0f, 1f)] private float _preferredSheetWidth;
    [SerializeField] Color _baseColor;
    [SerializeField] Color _hoverColor;

    private DetailsOption[] _options = new DetailsOption[0];
    private SpriteRenderer[] _iconBackgroundRenderers = new SpriteRenderer[0];
    private List<GameObject> _tempObjects = new List<GameObject>();
    private bool _asking = false;

    private void Start()
    {
        _input.PressingFinished += OnPressed;
    }

    public void AskDetails(DetailsOption[] options, Vector2 at)
    {
        _options = options;
        _asking = true;
        _tempObjects.Clear();
        Visualize(at);
    }

    private void OnPressed(object sender, Vector2 screenPosition)
    {
        if (_asking)
        {
            _asking = false;
            Vector2 worldPosition = CrossInput.ScreenToWorldPosition(screenPosition);
            for (int i = 0; i < _options.Length; ++i)
            {
                Vector3 p = new Vector3(worldPosition.x, worldPosition.y, _iconBackgroundRenderers[i].bounds.center.z);
                if (_iconBackgroundRenderers[i].bounds.Contains(p))
                {
                    DestroyTempObjects();
                    DetailsChoosen.Invoke(_options[i].DetailsCode);
                    return;
                }
            }
            DestroyTempObjects();
            DetailsChoosingRejected.Invoke();
        }
    }

    private void DestroyTempObjects()
    {
        foreach (GameObject temp in _tempObjects)
        {
            Destroy(temp);
        }
    }

    private void Visualize(Vector2 at)
    {
        if (_options.Length == 0)
        {
            return;
        }
        _iconBackgroundRenderers = new SpriteRenderer[_options.Length];
        float sheetWidth = _sheet.bounds.size.x;
        float tabWidth = sheetWidth * _preferredSheetWidth;
        float iconSize = tabWidth / _iconBackgroundRenderers.Length;
        GameObject tab = Instantiate(_panelPrefab);
        _tempObjects.Add(tab);
        SpriteRenderer tabRender = tab.GetComponent<SpriteRenderer>();
        tabRender.color = _baseColor;
        Vector3 oldScale = tab.transform.localScale;
        tab.transform.localScale = new Vector3(oldScale.x * tabWidth / tabRender.bounds.size.x, 
            oldScale.y * iconSize / tabRender.bounds.size.y, oldScale.z);
        Vector3 oldPos = tab.transform.position;
        float posY = (at.y < _sheet.bounds.center.y ? iconSize : -iconSize) / 2 + at.y;
        tab.transform.position = new Vector3(_sheet.transform.position.x, posY, oldPos.z);
        Vector3 tabMinPos = tabRender.bounds.min;
        for (int i = 0; i < _options.Length; ++i)
        {
            SpriteRenderer iconBackground = Instantiate(_panelPrefab).GetComponent<SpriteRenderer>();
            SpriteRenderer icon = Instantiate(_iconPrefab).GetComponent<SpriteRenderer>();
            icon.sprite = TextureHolder.GetSpriteFromPieceTexture(_options[i].Icon);
            _tempObjects.Add(icon.gameObject);
            _tempObjects.Add(iconBackground.gameObject);
            iconBackground.color = _baseColor;
            icon.color = _baseColor;
            _iconBackgroundRenderers[i] = iconBackground;
            Vector3 oldBackgroundPos = iconBackground.transform.position;
            Vector3 oldBackgroundScale = iconBackground.transform.localScale;
            iconBackground.transform.position = new Vector3(tabMinPos.x + iconSize / 2 + iconSize * i, tabMinPos.y + iconSize / 2, oldBackgroundPos.z);
            iconBackground.transform.localScale = new Vector3(oldBackgroundScale.x * iconSize / iconBackground.bounds.size.x,
                oldBackgroundScale.y * iconSize / iconBackground.bounds.size.y, oldBackgroundScale.z);
            Vector3 backGroundPosition = iconBackground.transform.position;
            icon.transform.position = new Vector3(backGroundPosition.x, backGroundPosition.y, backGroundPosition.z - 1);
            Vector3 oldIconScale = icon.transform.localScale;
            icon.transform.localScale = new Vector3(oldIconScale.x * iconSize / icon.bounds.size.x,
                oldIconScale.y * iconSize / icon.bounds.size.y, oldIconScale.z);
        }
    }

    private void Update()
    {
        if (_asking && _input.CanGetPointingPosition())
        {
            Vector3 pointingPos = CrossInput.ScreenToWorldPosition(_input.GetPointingPosition());
            foreach (SpriteRenderer backgroundRender in _iconBackgroundRenderers)
            {
                Vector3 p = new Vector3(pointingPos.x, pointingPos.y, backgroundRender.bounds.center.z);
                if (backgroundRender.bounds.Contains(p))
                {
                    backgroundRender.color = _hoverColor;
                }
                else
                {
                    backgroundRender.color = _baseColor;
                }
            }
        }
    }
}