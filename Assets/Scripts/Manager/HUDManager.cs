using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HUDManager : BaseSingleton<HUDManager>
{
   [SerializeField] HealthBar PlayerHealthBar;
   [SerializeField] FragmentShiftTimer FragmentShiftTimer;
   [SerializeField] PlayerInventory PlayerInventory;
   
   FragmentController FragmentController;
   PlayerController PlayerController;
   Stats PlayerStats;

   public ref PlayerInventory GetPlayerInventory() => ref PlayerInventory;
   
   void OnEnable()
   {
      GameManager GameManager = GameManager.Instance;
      
      PlayerController = GameManager.GetPlayerControllerRef;
   }

   private void OnDisable()
   { 
      UnbindEvents();
   }
   
   void DisablePlayerHud()
   {
      PlayerHealthBar.gameObject.SetActive(false);
      FragmentShiftTimer.gameObject.SetActive(false);
      UnbindEvents();
   }

   void UnbindEvents()
   {
   }
   void DisableHUD()
   {
      gameObject.SetActive(false);
   }
}
