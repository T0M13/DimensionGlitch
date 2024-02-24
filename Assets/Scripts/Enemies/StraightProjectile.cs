using UnityEngine;

public class StraightProjectile : Projectile
{
    Vector2 MovementDirection = Vector2.zero;
    
    void FixedUpdate()
    {
        ProjectileBehaviour();
    }

    public override void InitProjectile(Vector2 Direction, EFraction TargetFraction, GameObject Owner, Stats Target = null)
    {
        FractionToHit = TargetFraction;
        MovementDirection = Direction;
        Vector2 UpAxis = Vector3.Cross(Vector3.forward, MovementDirection);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, UpAxis);
        this.Owner = Owner;
        
    }

    protected override void ProjectileBehaviour()
    {
        if (!Owner || !Owner.activeSelf)
        {
            gameObject.SetActive(false);
            return;
        }
        
        Vector2 CurrentProjectilePosition = ProjectileRb.position;
        Vector2 DirectionOfMovement = MovementDirection * (ProjectileSpeed * Time.deltaTime);
        ProjectileRb.MovePosition(CurrentProjectilePosition + DirectionOfMovement);
    }
}
