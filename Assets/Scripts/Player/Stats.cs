using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
   [SerializeField, Min(0)] int HealhtPoints = 3;

   public event Action OnDamage;
   public event Action OnDeath;
   
   public void RecieveDmg()
   {
      OnDamage?.Invoke();
      HealhtPoints--;
      
      if(HealhtPoints <= 0)
      {
         Die();
      }
   }
   void Die()
   {
      InputManager.Instance.DisableAllActions();
      OnDeath?.Invoke();
   }
}
