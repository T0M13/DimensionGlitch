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
        transform.rotation = Quaternion.LookRotation(Vector3.forward, MovementDirection);
        this.Owner = Owner;
    }

    protected override void ProjectileBehaviour()
    {
        if (!Owner.gameObject.activeSelf)
        {
            Destroy(gameObject);
            return;
        }
        
        Vector2 CurrentProjectilePosition = ProjectileRb.position;
        Vector2 DirectionOfMovement = MovementDirection * (ProjectileSpeed * Time.deltaTime);
        ProjectileRb.MovePosition(CurrentProjectilePosition + DirectionOfMovement);
    }
}
