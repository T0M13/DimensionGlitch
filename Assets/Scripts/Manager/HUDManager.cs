using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HUDManager : BaseSingleton<HUDManager>
{
   [SerializeField] HealthBar PlayerHealthBar;
   [SerializeField] FragmentShiftTimer FragmentShiftTimer;
   [SerializeField] Inventory PlayerInventory;
   
   FragmentController FragmentController;
   PlayerController PlayerController;
   Stats PlayerStats;

   public ref Inventory GetPlayerInventory() => ref PlayerInventory;
   
   void OnEnable()
   {
      GameManager GameManager = GameManager.Instance;
      
      PlayerController = GameManager.GetPlayerControllerRef;

      //FragmentController.onFragmentShifting.AddListener(FragmentShiftPopUp.TriggerAnimation);
      //FragmentController.onGameOverVictory.AddListener(EnableWinScreen);
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
