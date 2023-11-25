using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CameraSizeAdjuster : MonoBehaviour
{
    private SpriteRenderer _rink;
    private int _screenWidth;
    private int _screenHeight;

    private void Start()
    {
        _rink = GetComponent<SpriteRenderer>();
        AdjustSize();
        StartCoroutine(CheckScreenSize());
    }

    private IEnumerator CheckScreenSize()
    {
        while (true)
        {
            if (Screen.width != _screenWidth || Screen.height != _screenHeight)
            {
                AdjustSize();
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void AdjustSize()
    {
        _rink = GetComponent<SpriteRenderer>();
        Vector3 cameraPos = Camera.main.transform.position;
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, cameraPos.z);
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = _rink.bounds.size.x / _rink.bounds.size.y;
        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = _rink.bounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = _rink.bounds.size.y / 2 * differenceInSize;
        }
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
    }
}
