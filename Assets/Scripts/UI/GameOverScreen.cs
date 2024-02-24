using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
   [SerializeField] float AnimationTime = 2.0f;
   [SerializeField] float TextBlinkSpeed = 1.5f;
   [SerializeField] float MainAudioFadeTime = 5.0f;
   [SerializeField] private string GamePlayScene = "GameplayScene";
   [SerializeField] TextMeshProUGUI GameOverText;
   [SerializeField] Image GameOverImage;
   [SerializeField] Button RestartButton;
   [SerializeField] OneShotAudioPlayer AudioPlayer;

   Color DefaultGameOverTextColor;
   Color DefaultGameOverImageColor;
   Color DefaultButtonColor;
   bool IsInGameOverScreen = true;

   private void Start()
   {
      RestartButton.onClick.AddListener(RestartGame);
   }

   public void GameOver()
   {
      DefaultGameOverTextColor = GameOverText.color;
      DefaultGameOverImageColor = GameOverImage.color;
      DefaultButtonColor = RestartButton.image.color;
      RestartButton.interactable = false;
      StartCoroutine(Animate());
      
      AudioManager.Instance.FadeMainAudio(MainAudioFadeTime, false);
      AudioPlayer.PlayOneShotAudioClip();
   }

   void RestartGame()
   {
      RestartButton.onClick.RemoveListener(RestartGame);
      RestartButton.interactable = false;
      SceneManager.LoadSceneAsync(GamePlayScene);
   }
   IEnumerator Animate()
   {
      float PassedTime = 0.0f;
      
      while (PassedTime < AnimationTime)
      {
         PassedTime += Time.deltaTime;
         float InterpValue = PassedTime / AnimationTime;
         
         Color NewColorText = Color.Lerp(Color.clear, DefaultGameOverTextColor, InterpValue);
         Color NewColorImage = Color.Lerp(Color.clear, DefaultGameOverImageColor, InterpValue);
         Color NewColorRestartButton = Color.Lerp(Color.clear, DefaultButtonColor, InterpValue);
         
         GameOverText.color = NewColorText;
         GameOverImage.color = NewColorImage;
         RestartButton.image.color = NewColorRestartButton;
         yield return null;
      }

      RestartButton.interactable = true;
      StartCoroutine(BlinkGameOverText());
   }

   IEnumerator BlinkGameOverText()
   {
      while (IsInGameOverScreen)
      {
         float Sin = Mathf.Abs(Mathf.Sin(Time.time * TextBlinkSpeed));
         GameOverText.color = Color.Lerp(DefaultGameOverTextColor, Color.clear, Sin);
         yield return null;
      }
   }
}
