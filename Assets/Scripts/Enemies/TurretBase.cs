using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurretBase : MonoBehaviour
{
    [SerializeField, Min(0.1f), Tooltip("Per second")] float AttackSpeed = 2;
    [SerializeField, Min(0)] float AttackRange = 3.0f;
    [SerializeField] LayerMask VisibilityBlockingMask; 
    [SerializeField] CircleCollider2D AttackRangeTrigger = null;
    [SerializeField] protected Transform AimingPoint;
    [SerializeField] protected Transform ProjetileSpawnPoint;
    [SerializeField] protected ProjectilePool ProjectilePool;
    [SerializeField] protected SpriteFXPool SpriteFXPool;

    float LastTimeAttacked = 0.0f;
    float AttackCD = 0.0f;
    protected Stats CurrentTarget = null;
    
    protected abstract void ShootProjectile();
    protected abstract void Aim();

    protected Vector2 GetDirectionToTargetNormalized() =>
        CurrentTarget ? (CurrentTarget.transform.position - AimingPoint.position).normalized : Vector2.zero;
    protected Vector2 GetDirectionToTarget() =>
        CurrentTarget ? CurrentTarget.transform.position - AimingPoint.position : Vector2.zero;

    private void OnValidate()
    {
        if (AttackRangeTrigger)
        {
            AttackRangeTrigger.isTrigger = true;
            AttackRangeTrigger.radius = AttackRange;
        }
    }

    void Start()
    {
        CalculateAttackCD();
    }
    
    void Update()
    {
        if(!HasTarget()) return;
        
        Aim();
        
        if (CanAttack())
        {
            LastTimeAttacked = Time.time;
            ShootProjectile();
        }
    }
    void CalculateAttackCD()
    {
        AttackCD = 1 / AttackSpeed;
    }
    void ResetTarget()
    {
        CurrentTarget.OnDeath -= ResetTarget;
        CurrentTarget = null;
    }

    bool HasTarget()
    {
        return CurrentTarget;
    }
    bool CanAttack()
    {
        //also check for visibility
        return Time.time - LastTimeAttacked > AttackCD && CanSeeTarget();
    }

    bool CanSeeTarget()
    {
        Vector2 RayStartPosition = AimingPoint.position;
        Vector2 RayEndPosition = RayStartPosition + GetDirectionToTarget();
        Vector2 RayDirection = GetDirectionToTargetNormalized();
        
        float RayDistance = Vector2.Distance(RayStartPosition, RayEndPosition);
        bool CanSeeTarget = Physics2D.Raycast(RayStartPosition, RayDirection, RayDistance, VisibilityBlockingMask);
        
        return !CanSeeTarget;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out Stats Stats))
        {
            if (Stats.IsFraction(EFraction.Player))
            {
                CurrentTarget = Stats;
                CurrentTarget.OnDeath += ResetTarget;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Stats Stats))
        {
            if (Stats.Equals(CurrentTarget))
            {
                ResetTarget();
            };
        }
    }
}
