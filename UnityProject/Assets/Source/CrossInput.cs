using System;
using UnityEngine;

public class CrossInput : MonoBehaviour
{
    public static CrossInput Instance { get; private set; }

    public event EventHandler<Vector2> PressingStarted;
    public event EventHandler<Vector2> Pressed;
    public event EventHandler<Vector2> PressingFinished;

    private bool _pressing = false;
    private Vector2 _initialPressingPosition = Vector2.zero;

#if UNITY_ANDROID
    private int _currentFingerId = -1;
    private bool _touching = false;
#endif

    public Vector2 InitialPressingScreenPosition => _initialPressingPosition;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            StartPressing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopPressing();
        }
        else if (_pressing)
        {
            Pressed?.Invoke(this, GetPointingPosition());
        }
#endif
#if UNITY_ANDROID
        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && _touching == false)
            {
                _currentFingerId = touch.fingerId;
                StartPressing();
                return;
            }
            else if (touch.phase == TouchPhase.Ended && _touching == true && touch.fingerId == _currentFingerId)
            {
                StopPressing();
                return;
            }
        }
        if (_pressing)
        {
            Pressed?.Invoke(this, GetPointingPosition());
        }
#endif
    }

    private void StartPressing()
    {
        _pressing = true;
#if UNITY_ANDROID
        _touching = true;
#endif
        _initialPressingPosition = GetPointingPosition();
        PressingStarted?.Invoke(this, _initialPressingPosition);
    }

    private void StopPressing()
    {
        PressingFinished?.Invoke(this, GetPointingPosition());
        _pressing = false;
#if UNITY_ANDROID
        _touching = false;
#endif
    }

    public bool CanGetPointingPosition()
    {
#if UNITY_STANDALONE
        return true;
#endif
#if UNITY_ANDROID
        if (_touching == false)
        {
            return false;
        }
        foreach (var touch in Input.touches)
        {
            if (touch.fingerId == _currentFingerId)
            {
                return true;
            }
        }
        return false;
#endif
    }

    public Vector2 GetPointingPosition()
    {
#if UNITY_STANDALONE
        return Input.mousePosition;
#endif
#if UNITY_ANDROID
        if (CanGetPointingPosition() == false)
        {
            throw new InvalidOperationException("Cannot get pointing position because there is no finger touching the screen");
        }
        foreach (var touch in Input.touches)
        {
            if (touch.fingerId == _currentFingerId)
            {
                return touch.position;
            }
        }
        return Vector2.zero;
#endif
    }

    public static Vector3 ScreenToWorldPosition(Vector2 screenPosition)
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0));
    }
}