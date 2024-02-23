using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
   [SerializeField] Image[] PlayerHealth;

   public void DeactivateHearts(int CurrentHealth)
   {
      int HeartsToDeactivate = PlayerHealth.Length - CurrentHealth;

      for (int i = PlayerHealth.Length - 1; i >= PlayerHealth.Length - HeartsToDeactivate; i--)
      {
         PlayerHealth[i].enabled = false;
      }
   }
}
