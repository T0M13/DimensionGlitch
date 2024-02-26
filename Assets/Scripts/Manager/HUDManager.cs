using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HUDManager : MonoBehaviour
{
   [SerializeField] HealthBar PlayerHealthBar;
   [SerializeField] FragmentShiftTimer FragmentShiftTimer;
   [SerializeField] FragmentShiftPopUp FragmentShiftPopUp;
   [SerializeField] GameEndDisplayScreen GameOverScreen;
   [SerializeField] GameEndDisplayScreen WinScreen;
   [SerializeField] FragmentCounter FragmentCounter;
   [SerializeField] DashCDBar DashCDBar;

   FragmentController FragmentController;
   PlayerController PlayerController;
   Stats PlayerStats;
   
   void OnEnable()
   {
      GameManager GameManager = GameManager.Instance;
      
      FragmentController = GameManager.FragmentController;
      PlayerController = GameManager.GetPlayerControllerRef;
      PlayerStats = PlayerController.GetPlayerStats();
      
      FragmentController.onFragmentShifting.AddListener(FragmentShiftPopUp.TriggerAnimation);
      FragmentController.onGameOverVictory.AddListener(EnableWinScreen);
      PlayerController.OnDash += DashCDBar.StartDashCD;
      PlayerStats.OnDamage += PlayerHealthBar.DeactivateHearts;
      PlayerStats.OnDeath += EnableGameOverScreen;
      PlayerStats.OnDeath += DisablePlayerHud;
      
      GameOverScreen.gameObject.SetActive(false);
      WinScreen.gameObject.SetActive(false);
   }

   private void OnDisable()
   { 
      UnbindEvents();
   }
   
   void EnableGameOverScreen()
   {
      if (WinScreen.IsScreenActive()) return;
      
      GameOverScreen.gameObject.SetActive(true);
      GameOverScreen.DisplayScreen();
   }

   void EnableWinScreen()
   {
      if(GameOverScreen.IsScreenActive()) return;
      
      WinScreen.gameObject.SetActive(true);
      WinScreen.DisplayScreen();
   }
   void DisablePlayerHud()
   {
      PlayerHealthBar.gameObject.SetActive(false);
      FragmentShiftTimer.gameObject.SetActive(false);
      FragmentShiftPopUp.gameObject.SetActive(false);
      DashCDBar.gameObject.SetActive(false);
      FragmentCounter.gameObject.SetActive(false);
      UnbindEvents();
   }

   void UnbindEvents()
   {
      PlayerController.OnDash -= DashCDBar.StartDashCD;
      PlayerStats.OnDamage -= PlayerHealthBar.DeactivateHearts;
      PlayerStats.OnDeath -= DisablePlayerHud;
      PlayerStats.OnDeath -= EnableGameOverScreen;
      FragmentController.onFragmentShifting.RemoveListener(FragmentShiftPopUp.TriggerAnimation);
      FragmentController.onGameOverVictory.RemoveListener(EnableWinScreen);
   }
   void DisableHUD()
   {
      gameObject.SetActive(false);
   }
}
