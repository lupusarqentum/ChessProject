using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PieceComponent : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private PieceUIInfo _pieceUIInfo;
    private SquareComponent _square;

    private bool _sliding = false;
    private PieceComponent _destinationPiece;

    public SquareComponent Square => _square;

    private void Awake()
    {
        _pieceUIInfo = GameInfo.Instance.PieceInfoProvider.GetDefault();
        _square = null;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = false;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Setup(SquareComponent square, PieceUIInfo info)
    {
        _pieceUIInfo = info;
        _square = square;
        _spriteRenderer.sprite = TextureHolder.GetSpriteFromPieceTexture(info.Texture);
        Vector3 _squarePos = _square.transform.position;
        transform.position = new Vector3(_squarePos.x, _squarePos.y, transform.position.z);
        Vector3 oldScale = transform.localScale;
        SpriteRenderer squareRender = _square.GetComponent<SpriteRenderer>();
        float scaleFactor = Mathf.Min(squareRender.bounds.size.x / _spriteRenderer.bounds.size.x,
            squareRender.bounds.size.y / _spriteRenderer.bounds.size.y);
        transform.localScale = new Vector3(oldScale.x * scaleFactor, oldScale.y * scaleFactor, oldScale.z);
        _spriteRenderer.flipY = info.CanBeFlipped && GameInfo.Instance.ShouldFlipPieces;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_sliding && _destinationPiece?.gameObject.name.Equals(collision.gameObject.name) == true)
        {
            _destinationPiece.BeCaptured();
        }
    }

    public void BeCaptured()
    {
        _spriteRenderer.enabled = false;
    }

    public void SlideTo(SquareComponent square, PieceComponent pieceToCapture, Action onPieceSlided)
    {
        if (_sliding)
        {
            return;
        }
        if (_pieceUIInfo.MovingAnimation == MovingAnimationType.Rider)
        {
            _sliding = true;
            _destinationPiece = pieceToCapture;
            StartCoroutine(Slide(square.transform.position, _pieceUIInfo.MillisecondsToMove, onPieceSlided));
        }
        else if (_pieceUIInfo.MovingAnimation == MovingAnimationType.Leaper)
        {
            if (pieceToCapture != null)
            {
                pieceToCapture.BeCaptured();
            }
            EndSliding(square.transform.position, onPieceSlided);
        }
    }

    private IEnumerator Slide(Vector3 destination, float inMilliseconds, Action afterSlidingAction)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        Vector3 startingPosition = transform.position;
        Vector3 direction = new Vector3(destination.x - startingPosition.x, destination.y - startingPosition.y, 0);
        int framesCount = Mathf.FloorToInt(1 / Time.deltaTime * inMilliseconds / 1000);
        float startingSpeed = 0.75f / framesCount;
        float acceleration = (2 - 2 * startingSpeed * framesCount) / (framesCount - 1) / framesCount;
        float completion = 0f;
        for (int i = 0; i < framesCount; ++i)
        {
            completion += startingSpeed + acceleration * i;
            transform.position = startingPosition + direction * completion;
            yield return null;
        }
        transform.position = new Vector3(startingPosition.x, startingPosition.y, startingPosition.z + 1);
        EndSliding(destination, afterSlidingAction);
    }

    private void EndSliding(Vector3 destination, Action afterSlidingAction)
    {
        _sliding = false;
        transform.position = new Vector3(destination.x, destination.y, transform.position.z);
        afterSlidingAction.Invoke();
    }
}
