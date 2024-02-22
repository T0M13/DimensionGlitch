using UnityEngine;

public class OsciliatingMovement : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float Range;
    [SerializeField] Transform OsciliationOrigin;
    [SerializeField] DirectionContainer Direction;
    
    Vector2 DirectionOfMovement = Vector2.zero;

    void Start()
    {
        DirectionOfMovement = Direction.GetDirectionalValue();
    }
    void Update()
    {
        Move();   
    }
    void Move()
    {
        Vector2 InitialPosition = OsciliationOrigin.position;
        Vector2 NewPosition = InitialPosition + DirectionOfMovement * (Mathf.Sin(Time.time * Speed) * Range);
        transform.position = NewPosition;
    }
}
