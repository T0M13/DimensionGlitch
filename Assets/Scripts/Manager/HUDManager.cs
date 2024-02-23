using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
   [SerializeField] HealthBar PlayerHealthBar;

   PlayerController PlayerController;

   void Start()
   {
      PlayerController = GameManager.Instance.GetPlayerControllerRef;
      Stats PlayerStats = PlayerController.GetPlayerStats();

      PlayerStats.OnDamage += PlayerHealthBar.DeactivateHearts;
   }
}
