using UnityEngine;

[System.Serializable]
public class DirectionContainer
{
    [SerializeField] EDirections DirectionMask;
    
    [System.Flags]
    public enum EDirections : byte
    {
        Up = 1 << 0,
        Right = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3
    }

    public Vector2 GetDirectionalValue()
    {
        Vector2 OutDirection = Vector2.zero;

        if ((DirectionMask & EDirections.Up) != 0)
        {
            OutDirection += Vector2.up;
        }

        if ((DirectionMask & EDirections.Down) != 0)
        {
            OutDirection += Vector2.down;
        }

        if ((DirectionMask & EDirections.Left) != 0)
        {
            OutDirection += Vector2.left;
        }

        if ((DirectionMask & EDirections.Right) != 0)
        {
            OutDirection += Vector2.right;
        }

        return OutDirection;
    }
}

