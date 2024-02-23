using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
   [SerializeField] float TriggerCD = 1.0f;
   [SerializeField] float TimeUntilTrapActivate = 0.5f;
   [SerializeField] BoxCollider2D TriggerCollider;
   
   float LastTimeTriggered = 0.0f;
   
   public event Action OnTrapTriggerTriggered; 
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.TryGetComponent(out Stats stats) && CanBeTriggered())
      {
         if (stats.IsDamageable())
         {
            StartCoroutine(TrapActivationRoutine());
         }
      }
   }

   IEnumerator TrapActivationRoutine()
   {
      TriggerCollider.enabled = false;
      
      yield return new WaitForSeconds(TimeUntilTrapActivate);

      LastTimeTriggered = Time.time;
      OnTrapTriggerTriggered?.Invoke();

      yield return new WaitForSeconds(TriggerCD);

      TriggerCollider.enabled = true;
   }
   bool CanBeTriggered()
   {
      return Time.time - LastTimeTriggered > TriggerCD;
   }
}
