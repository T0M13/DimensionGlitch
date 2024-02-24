using UnityEngine;

public class AutoTurret : TurretBase
{
    protected override void ShootProjectile()
    {
        Projectile ProjectileRef = ProjectilePool.GetObjectFromPool(ProjetileSpawnPoint.position);
        ProjectileRef.InitProjectile(GetDirectionToTargetNormalized(), EFraction.Player, gameObject, CurrentTarget);
        ProjectileRef.OnProjectileHit += SpriteFXPool.SpawnSpriteObjectAtPosition;
        ProjectileRef.gameObject.SetActive(true);
    }
    protected override void Aim()
    {
        Vector3 Forward = AimingPoint.forward;
        
        Vector2 UpAxis = Vector3.Cross(Vector3.forward, GetDirectionToTargetNormalized());
        Vector2 AimingPointPos = AimingPoint.position;
        AimingPoint.rotation =  Quaternion.LookRotation(Forward, UpAxis);
    }
}
