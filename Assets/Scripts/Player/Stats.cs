using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField, Min(0)] int HealhtPoints = 3;
    [SerializeField] private EFraction MyFraction = EFraction.Player;

    private bool IsInvincible = false;
    public bool SetInvincibility(bool IsInvincible) => this.IsInvincible = IsInvincible;
    public bool IsDamageable() => !IsInvincible && !IsDead();
    public bool IsDead() => HealhtPoints <= 0;
    public bool IsFraction(EFraction MyFraction) => this.MyFraction == MyFraction;
    public event Action<int> OnDamage;
    public event Action OnDeath;

    public virtual bool RecieveDmg()
    {
        HealhtPoints--;
        OnDamage?.Invoke(HealhtPoints);

        if (HealhtPoints <= 0)
        {
            Die();
            return true;
        }
        return false;
    }
    void Die()
    {
        OnDeath?.Invoke();
    }
}
