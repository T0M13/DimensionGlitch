using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float ProjectileSpeed = 0.0f;
    [SerializeField] protected float InitialLifetime = 0.0f;
    [SerializeField] protected Rigidbody2D ProjectileRb;

    protected GameObject Owner = null;
    
    protected EFraction FractionToHit = EFraction.Player;
    
    public abstract void InitProjectile(Vector2 Direction, EFraction TargetFraction, GameObject Owner, Stats Target = null);
    protected abstract void ProjectileBehaviour();

    protected virtual void Start()
    {
        Destroy(gameObject, InitialLifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Stats Stats))
        {
            if (Stats.IsFraction(FractionToHit) && Stats.IsDamageable())
            {
                Stats.RecieveDmg();
                Destroy(gameObject);
            }
        }
        else if(other.gameObject != Owner)
        {
            Destroy(gameObject);
        }
    }
}
