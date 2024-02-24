using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
   [SerializeField] float AnimationTime = 2.0f;
   [SerializeField] float TextBlinkSpeed = 1.5f;
   [SerializeField] float MainAudioFadeTime = 5.0f;
   [SerializeField] TextMeshProUGUI GameOverText;
   [SerializeField] Image GameOverImage;
   [SerializeField] OneShotAudioPlayer AudioPlayer;

   Color DefaultGameOverTextColor;
   Color DefaultGameOverImageColor;
   bool IsInGameOverScreen = true;
   
   public void GameOver()
   {
      DefaultGameOverTextColor = GameOverText.color;
      DefaultGameOverImageColor = GameOverImage.color;
      StartCoroutine(Animate());
      
      AudioManager.Instance.FadeMainAudio(MainAudioFadeTime, false);
      AudioPlayer.PlayOneShotAudioClip();
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
         
         GameOverText.color = NewColorText;
         GameOverImage.color = NewColorImage;
         yield return null;
         
      }

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
