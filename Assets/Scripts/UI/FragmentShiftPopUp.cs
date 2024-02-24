using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FragmentShiftPopUp : MonoBehaviour
{
   [SerializeField] Color EndColor;
   [SerializeField, Min(0)] float AnimationTime = 10.0f;
   [SerializeField, Min(0)] float MaxScale = 4.0f;
   [SerializeField] TextMeshProUGUI ShiftText;
   private void Start()
   {
      gameObject.SetActive(false);
   }

  public void TriggerAnimation()
   {
      gameObject.SetActive(true);
      StartCoroutine(Animate());
   }

   IEnumerator Animate()
   {
      float PassedTime = 0.0f;

      Color DefaultColor = ShiftText.color;
      
      while (PassedTime < AnimationTime)
      {
         PassedTime += Time.deltaTime;

         float InterpolationValue = PassedTime / AnimationTime;

         float ScaleXY = Mathf.Lerp(0, MaxScale, InterpolationValue);
        
         transform.localScale = new Vector2(ScaleXY, ScaleXY);
         ShiftText.color = Color.Lerp(DefaultColor, EndColor, InterpolationValue);

         yield return null;
      }

      ShiftText.color = DefaultColor;
      gameObject.SetActive(false);
   }
}
