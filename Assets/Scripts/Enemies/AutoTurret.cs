using UnityEngine;

public class AutoTurret : TurretBase
{
    protected override void ShootProjectile()
    {
        Projectile ProjectileRef = Instantiate(TurretProjectile, ProjetileSpawnPoint.position, Quaternion.identity);
        ProjectileRef.InitProjectile(GetDirectionToTargetNormalized(), EFraction.Player, gameObject, CurrentTarget);
    }
    protected override void Aim()
    {
        AimingPoint.rotation =  Quaternion.LookRotation(AimingPoint.forward, GetDirectionToTargetNormalized());
    }
}
