using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] TrapTrigger TrapTrigger;
    [SerializeField] ProjectilePool ProjectilePool;
    [SerializeField] SpriteFXPool FXPool;
    [SerializeField] OneShotAudioPlayer ShootSound;
    [SerializeField] Transform projectileShootPosition;

    void Start()
    {
        TrapTrigger.OnTrapTriggerTriggered += ShootProjectile;
    }
    private void OnDisable()
    {
        TrapTrigger.OnTrapTriggerTriggered -= ShootProjectile;
    }
    void ShootProjectile()
    {
        Projectile ProjectileToShoot = ProjectilePool.GetObjectFromPool(projectileShootPosition.position);
        ProjectileToShoot.InitProjectile(transform.right, EFraction.Player, gameObject);
        ProjectileToShoot.OnProjectileHit += FXPool.SpawnSpriteObjectAtPosition;
        ShootSound.PlayOneShotAudioClip();
        ProjectileToShoot.gameObject.SetActive(true);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (TrapTrigger)
        {
            Gizmos.DrawLine(transform.position, TrapTrigger.transform.position);
        }
        Gizmos.color = Color.red;
        if (projectileShootPosition)
            Gizmos.DrawWireSphere(projectileShootPosition.position, 0.2f);
    }
}
