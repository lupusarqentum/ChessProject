using System;
using UnityEngine;

public class MovesRegistrator : MonoBehaviour
{
    public enum RegistrationState : byte
    {
        None,
        Dragging,
        Selected,
        Waiting
    }

    [SerializeField] private float _eps;
    [SerializeField] private PiecesSetup _piecesSetup;
    [SerializeField] private SquaresLoader _squaresSetup;
    [SerializeField] private MoveDetailsChoosingTab _choosingTab;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] [HideInInspector] private CrossInput _input = null;

    private RegistrationState _state = RegistrationState.None;
    private PieceComponent _capturedPiece = null;

    private string _from;
    private string _to;

    public RegistrationState State => _state;
    public string CapturedSquareOrEmpty
    {
        get
        {
            if (State != RegistrationState.Selected && State != RegistrationState.Dragging)
            {
                return "";
            }
            return _capturedPiece.Square.gameObject.name;    
        }
    }

    private void Start()
    {
        _choosingTab.DetailsChoosen += ChooseDetails;
        _choosingTab.DetailsChoosingRejected += RejectDetailsChoice;
    }

    public void SetGameManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void SetInput(CrossInput input)
    {
        if (_input != null)
        {
            _input.Pressed -= OnPressing;
            _input.PressingFinished -= OnPressingFinished;
        }
        _input = input;
        _input.Pressed += OnPressing;
        _input.PressingFinished += OnPressingFinished;
    }

    public void OnPressing(object sender, Vector2 screenPosition)
    {
        if ((_state == RegistrationState.None || _state == RegistrationState.Selected)
            && (screenPosition - _input.InitialPressingScreenPosition).sqrMagnitude > _eps)
        {
            if (_state == RegistrationState.None)
            {
                _capturedPiece = TryCapture(screenPosition);
                if (_capturedPiece != null)
                {
                    _state = RegistrationState.Dragging;
                }
            }
            else
            {
                if (GetSquareNameOrNull(_input.InitialPressingScreenPosition).Equals(_capturedPiece.Square.gameObject.name))
                {
                    _state = RegistrationState.Dragging;
                }
            }
        }
        else if (_state == RegistrationState.Dragging)
        {
            float oldZ = _capturedPiece.transform.position.z;
            Vector2 pointingPosition = _input.GetPointingPosition();
            Vector3 worldPos = CrossInput.ScreenToWorldPosition(pointingPosition);
            _capturedPiece.transform.position = new Vector3(worldPos.x, worldPos.y, oldZ);
        }
    }

    public void OnPressingFinished(object sender, Vector2 screenPosition)
    {
        Vector2 worldPosition = CrossInput.ScreenToWorldPosition(screenPosition);
        if (_state == RegistrationState.Dragging)
        {
            string toSquareName = GetSquareNameOrNull(screenPosition);
            string fromSquareName = _capturedPiece.Square.gameObject.name;
            if (toSquareName == null || toSquareName.Equals(fromSquareName))
            {
                DropDraggedCapturedPiece();
            }
            else
            {
                Vector3 oldPos = _capturedPiece.transform.position;
                Vector3 squarePos = GetSquareOrNull(screenPosition).transform.position;
                _capturedPiece.transform.position = new Vector3(squarePos.x, squarePos.y, oldPos.z);
                if (GameInfo.Instance.CanGoFromTo(fromSquareName, toSquareName))
                {
                    _piecesSetup.GetPieceOnOrNullBySquareName(toSquareName)?.BeCaptured();
                    RegisterMove(fromSquareName, toSquareName, worldPosition);
                }
                else
                {
                    DropDraggedCapturedPiece();
                }
            }
        }
        else if (_state == RegistrationState.None)
        {
            _capturedPiece = TryCapture(screenPosition);
            if (_capturedPiece != null)
            {
                _state = RegistrationState.Selected;
            }
        }
        else if (_state == RegistrationState.Selected)
        {
            string fromSquare = _capturedPiece.Square.gameObject.name;
            string toSquareName = GetSquareNameOrNull(screenPosition);
            if (toSquareName == null || toSquareName.Equals(fromSquare))
            {
                _state = RegistrationState.None;
            }
            else
            {
                if (GameInfo.Instance.CanGoFromTo(fromSquare, toSquareName))
                {
                    RegisterMoveViaSelection(fromSquare, toSquareName, worldPosition);
                }
                else
                {
                    _capturedPiece = TryCapture(screenPosition);
                    if (_capturedPiece == null)
                    {
                        _state = RegistrationState.None;
                    }
                }
            }
        }
    }

    private void DropDraggedCapturedPiece()
    {
        if (_capturedPiece != null)
        {
            Vector3 squarePos = _capturedPiece.Square.transform.position;
            _capturedPiece.transform.position = new Vector3(squarePos.x, squarePos.y, _capturedPiece.transform.position.z);
        }
        _state = RegistrationState.None;
    }

    private string GetSquareNameOrNull(Vector2 screenPosition)
    {
        Vector3 worldPos = CrossInput.ScreenToWorldPosition(screenPosition);
        return _squaresSetup.GetSquareNameByPositionOrNull(worldPos);
    }

    private SquareComponent GetSquareOrNull(Vector2 screenPosition)
    {
        Vector3 worldPos = CrossInput.ScreenToWorldPosition(screenPosition);
        return _squaresSetup.GetSquareByPositionOrNull(worldPos);
    }

    private PieceComponent TryCapture(Vector2 position)
    {
        string squareName = GetSquareNameOrNull(position);
        return squareName == null ? null : _piecesSetup.GetPieceOnOrNullBySquareName(squareName);
    }

    private void RegisterMoveViaSelection(string from, string to, Vector2 at)
    {
        void OnPieceSlided() => RegisterMove(from, to, at);

        _state = RegistrationState.Waiting;
        SquareComponent square = GameObject.Find(to).GetComponent<SquareComponent>();
        PieceComponent pieceComponent = _piecesSetup.GetPieceOnOrNullBySquareName(from);
        pieceComponent.SlideTo(square, _piecesSetup.GetPieceOnOrNullBySquareName(to), OnPieceSlided);
    }

    private void RegisterMove(string from, string to, Vector2 at)
    {
        _state = RegistrationState.Waiting;
        DetailsOption[] options = GameInfo.Instance.GetMoveOptions(from, to);
        _from = from;
        _to = to;
        if (options.Length > 1)
        {
            _choosingTab.AskDetails(options, at);
        }
        else
        {
            ChooseDetails(options[0].DetailsCode);
        }
    }

    private void RejectDetailsChoice()
    {
        _state = RegistrationState.None;
        _gameManager.Refresh();
    }

    private void ChooseDetails(string detailsCode)
    {
        _state = RegistrationState.None;
        string move = GameInfo.Instance.GetMove(_from, _to, detailsCode);
        _gameManager.AcceptMove(move);
    }

    public void Reset()
    {
        _state = RegistrationState.None;
        _capturedPiece = null;
        _from = null;
        _to = null;
    }
}