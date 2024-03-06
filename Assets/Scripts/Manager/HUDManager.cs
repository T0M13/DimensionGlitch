using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class HUDManager : BaseSingleton<HUDManager>
{
   [SerializeField] HealthBar PlayerHealthBar;
   [SerializeField] FragmentShiftTimer FragmentShiftTimer;
   [SerializeField] PlayerInventory PlayerInventory;

   [Header("HotKeys")] 
   [SerializeField] InputActionReference OpenInventoryAction;
   
   FragmentController FragmentController;
   PlayerController PlayerController;
   Stats PlayerStats;

   public ref PlayerInventory GetPlayerInventory() => ref PlayerInventory;
   
   void OnEnable()
   {
      GameManager GameManager = GameManager.Instance;
      
      PlayerController = GameManager.GetPlayerControllerRef;
      OpenInventoryAction.action.performed += OpenInventory;
   }
   
   private void OnDisable()
   { 
      UnbindEvents();
   }

   void OpenInventory(InputAction.CallbackContext _)
   {
      PlayerInventory.gameObject.SetActive(!PlayerInventory.gameObject.activeSelf);
   }
   
   void DisablePlayerHud()
   {
      PlayerHealthBar.gameObject.SetActive(false);
      FragmentShiftTimer.gameObject.SetActive(false);
      UnbindEvents();
   }

   void UnbindEvents()
   {
      OpenInventoryAction.action.performed -= OpenInventory;
   }
   void DisableHUD()
   {
      gameObject.SetActive(false);
   }
}
