using UnityEngine;

[System.Serializable]
public class DirectionContainer
{
    [SerializeField] EDirections DirectionMask;

    public EDirections GetDirectionMask() => DirectionMask;
    
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

    public Vector2 GetDirectionForMaskValue(EDirections InDirection)
    {
        Vector2 OutDirection = Vector2.zero;
        
        if ((InDirection & EDirections.Up) != 0)
        {
            OutDirection = Vector2.up;
            return OutDirection;
        }

        if ((InDirection & EDirections.Down) != 0)
        {
            OutDirection = Vector2.down;
            return OutDirection;
        }

        if ((InDirection & EDirections.Left) != 0)
        {
            OutDirection = Vector2.left;
            return OutDirection;
        }

        if ((InDirection & EDirections.Right) != 0)
        {
            OutDirection = Vector2.right;
            return OutDirection;
        }

        return OutDirection;
    }
}

