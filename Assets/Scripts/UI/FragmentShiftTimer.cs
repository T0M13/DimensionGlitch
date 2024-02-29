using System.Text;
using TMPro;
using UnityEngine;

public class FragmentShiftTimer : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI TimerText;
   [SerializeField, Min(0)] float MaxIntensityScale = 2.8f;
   [SerializeField, Min(0)] float MinIntensityScale = 1.4f;
   [SerializeField, Min(0)] float MaxIntensitySpeed = 1.4f;
   [SerializeField] Color EmergencyColor;
   [SerializeField] Color DefaultColor;

   float CurrentTimerValue = 0.0f;
   float MaxTimerValue = 0.0f;
   StringBuilder StringBuilder = new StringBuilder();
   
   private void Start()
   {
      TimerText.color = DefaultColor;
      MaxTimerValue = GameManager.Instance.FragmentController.FragmentShiftTimerCooldown;
   }
   void Update()
   {
      CurrentTimerValue = GameManager.Instance.FragmentController.FragmentShiftTimer;

      StringBuilder.Clear();
      StringBuilder.Append(CurrentTimerValue.ToString("F2"));
      TimerText.SetText(StringBuilder.ToString());
      
      Animate();
   }

   void Animate()
   {
      float IntensityScale = CurrentTimerValue / MaxTimerValue;
      float IntensitySpeed = MaxIntensitySpeed * IntensityScale;
      
      float Sine = Mathf.Abs(Mathf.Sin(Time.time * IntensitySpeed) * IntensityScale);
      float ScaleXY = Mathf.Lerp(MinIntensityScale, MaxIntensityScale, Sine);
      Color IntensityColor = Color.Lerp(DefaultColor, EmergencyColor, Sine);

      TimerText.color = IntensityColor;
      TimerText.transform.localScale = new Vector3(ScaleXY, ScaleXY, 1);
   }
}
