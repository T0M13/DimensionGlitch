using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float ProjectileSpeed = 0.0f;
    [SerializeField] protected float InitialLifetime = 0.0f;
    [SerializeField] protected Rigidbody2D ProjectileRb;
    [SerializeField] protected OneShotAudioPlayer ImpactAudio;
    
    protected GameObject Owner = null;
    protected EFraction FractionToHit = EFraction.Player;
    public event Action<Vector2> OnProjectileHit;
    
    public abstract void InitProjectile(Vector2 Direction, EFraction TargetFraction, GameObject Owner, Stats Target = null);
    protected abstract void ProjectileBehaviour();

    private void OnEnable()
    {
        StartCoroutine(DeactivationTimer());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        OnProjectileHit = null;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Stats Stats))
        {
            if (Stats.IsFraction(FractionToHit) && Stats.IsDamageable())
            {
                Stats.RecieveDmg();
                ImpactAudio.PlayOneShotAudioClip();
                OnProjectileHit?.Invoke(transform.position);
                gameObject.SetActive(false);
                
            }
        }
        else if(other.gameObject != Owner)
        {
            StopAllCoroutines();
            ImpactAudio.PlayOneShotAudioClip();
            OnProjectileHit?.Invoke(transform.position);
            gameObject.SetActive(false);
        }
    }
    IEnumerator DeactivationTimer()
    {
        yield return new WaitForSeconds(InitialLifetime);
      
        gameObject.SetActive(false);
    }
}
