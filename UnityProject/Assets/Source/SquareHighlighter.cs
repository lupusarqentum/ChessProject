using UnityEngine;

[RequireComponent(typeof(SquareComponent))]
public class SquareHighlighter : MonoBehaviour
{
    private CrossInput _input;
    private MovesRegistrator _movesRegistrator;
    private SquareComponent _squareComponent;
    private MovesRegistrator.RegistrationState _registratorState;
    private string _registratorCapturedSquare;

    private void Awake()
    {
        _squareComponent = GetComponent<SquareComponent>();
        _input = CrossInput.Instance;
    }

    public void SetRegistrator(MovesRegistrator movesRegistrator)
    {
        _movesRegistrator = movesRegistrator;
        OnStateChanged();
    }

    private void Update()
    {
        if (_movesRegistrator != null
            && (_movesRegistrator.State != _registratorState || _movesRegistrator.CapturedSquareOrEmpty != _registratorCapturedSquare))
        {
            OnStateChanged();
        }
        if (_squareComponent.HavePoint)
        {
            if (_input.CanGetPointingPosition())
            {
                Vector2 pointingPosition = CrossInput.ScreenToWorldPosition(_input.GetPointingPosition());
                bool oldHoveredValue = _squareComponent.Hovered;
                _squareComponent.Hovered = _squareComponent.ContainsPointInWorld(pointingPosition);
                if (_squareComponent.Hovered != oldHoveredValue)
                {
                    _squareComponent.UpdateView();
                }
            }
        }
    }

    private void OnStateChanged()
    {
        _registratorState = _movesRegistrator.State;
        _registratorCapturedSquare = _movesRegistrator.CapturedSquareOrEmpty;
        _squareComponent.ResetUI();
        if (_registratorState == MovesRegistrator.RegistrationState.Selected
            || _registratorState == MovesRegistrator.RegistrationState.Dragging)
        {
            string fromSquare = _movesRegistrator.CapturedSquareOrEmpty;
            string toSquare = gameObject.name;
            if (fromSquare.Equals(toSquare))
            {
                _squareComponent.Selected = true;
            }
            else
            {
                _squareComponent.HavePoint = GameInfo.Instance.CanGoFromTo(fromSquare, toSquare);
            }
        }
        _squareComponent.UpdateView();
    }
}
