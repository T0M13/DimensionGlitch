using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashCDBar : MonoBehaviour
{
   [SerializeField] Image DashCdImage;
   [SerializeField] OneShotAudioPlayer DashReadySound;
   
   public void StartDashCD()
   {
      float DashCD = GameManager.Instance.GetPlayerControllerRef.GetDashCD();

      StartCoroutine(AnimateDashCD(DashCD));
   }

   IEnumerator AnimateDashCD(float DashCD)
   {
      float PassedTime = 0.0f;

      while (PassedTime < DashCD)
      {
         PassedTime += Time.deltaTime;
         float InterpValue = PassedTime / DashCD;

         DashCdImage.fillAmount = InterpValue;
         
         yield return null;
      }
      
      DashReadySound.PlayOneShotAudioClip();
   }
}
