using UnityEngine;

public class FPSReducer : MonoBehaviour
{
    [SerializeField] private int _targetFrameRate;

    private void Start()
    {
        LimitFramerate(_targetFrameRate);
    }

#if UNITY_EDITOR
    private void Update()
    {
        LimitFramerate(_targetFrameRate);
    }
#endif

    private void LimitFramerate(int value)
    {
        Application.targetFrameRate = value;
    }
}
