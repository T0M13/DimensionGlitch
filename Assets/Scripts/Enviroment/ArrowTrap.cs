using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] TrapTrigger TrapTrigger;
    [SerializeField] Projectile ProjectileToShoot;

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
        Projectile ProjectileToShoot = Instantiate(this.ProjectileToShoot, transform.position, Quaternion.identity);
        ProjectileToShoot.InitProjectile(transform.right, EFraction.Player, gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (TrapTrigger)
        {
            Gizmos.DrawLine(transform.position, TrapTrigger.transform.position);
        }
    }
}
