using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
   [SerializeField] HealthBar PlayerHealthBar;
   [SerializeField] FragmentShiftTimer FragmentShiftTimer;
   [SerializeField] FragmentShiftPopUp FragmentShiftPopUp;
   [SerializeField] GameOverScreen GameOverScreen;

   FragmentController FragmentController;
   PlayerController PlayerController;
   Stats PlayerStats;
   
   void Start()
   {
      GameManager GameManager = GameManager.Instance;
      
      FragmentController = GameManager.FragmentController;
      PlayerController = GameManager.GetPlayerControllerRef;
      PlayerStats = PlayerController.GetPlayerStats();
      
      FragmentController.onFragmentShifting.AddListener(FragmentShiftPopUp.TriggerAnimation);
      PlayerStats.OnDamage += PlayerHealthBar.DeactivateHearts;
      PlayerStats.OnDeath += EnableGameOverScreen;
      PlayerStats.OnDeath += DisablePlayerHud;
      
      GameOverScreen.gameObject.SetActive(false);
   }

   private void OnDisable()
   { 
      FragmentController.onFragmentShifting.RemoveListener(FragmentShiftPopUp.TriggerAnimation);
      UnbindEvents();
   }
   
   void EnableGameOverScreen()
   {
      GameOverScreen.gameObject.SetActive(true);
      GameOverScreen.GameOver();
   }
   void DisablePlayerHud()
   {
      PlayerHealthBar.gameObject.SetActive(false);
      FragmentShiftTimer.gameObject.SetActive(false);
      FragmentShiftPopUp.gameObject.SetActive(false);
      UnbindEvents();
   }

   void UnbindEvents()
   {
      PlayerStats.OnDamage -= PlayerHealthBar.DeactivateHearts;
      PlayerStats.OnDeath -= DisablePlayerHud;
      PlayerStats.OnDeath -= EnableGameOverScreen;
      FragmentController.onFragmentShifting.RemoveListener(FragmentShiftPopUp.TriggerAnimation);
   }
   void DisableHUD()
   {
      gameObject.SetActive(false);
   }
}
