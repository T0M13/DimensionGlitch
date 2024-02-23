using UnityEngine;

[ExecuteAlways]
public class DamageTrigger : MonoBehaviour
{
   [SerializeField] BoxCollider2D DamageTriggerCollider;

   public ref BoxCollider2D GetTriggerCollider() => ref DamageTriggerCollider;
   public void SetTrigger(bool IsEnabled)
   {
      DamageTriggerCollider.enabled = IsEnabled;
   }
   private void Awake()
   {
      if (!Application.isPlaying)
      {
         if (!DamageTriggerCollider)
         {
            DamageTriggerCollider = gameObject.GetComponent<BoxCollider2D>();

            if (!DamageTriggerCollider)
            {
               DamageTriggerCollider = gameObject.AddComponent<BoxCollider2D>();
            }
            
            DamageTriggerCollider.isTrigger = true;
         }     
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.TryGetComponent(out Stats Stats))
      {
         if (Stats.IsDamageable())
         {
            Stats.RecieveDmg();
         } 
      }
   }
}
