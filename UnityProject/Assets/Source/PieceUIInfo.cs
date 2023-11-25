using UnityEngine;

public struct PieceUIInfo
{
    public Texture2D Texture;
    public MovingAnimationType MovingAnimation;
    public float MillisecondsToMove;
    public bool CanBeFlipped;

    public PieceUIInfo(Texture2D texture, MovingAnimationType movingAnimation, float millisecondsToMove, bool canBeFlipped)
    {
        Texture = texture;
        MovingAnimation = movingAnimation;
        MillisecondsToMove = millisecondsToMove;
        CanBeFlipped = canBeFlipped;
    }
}