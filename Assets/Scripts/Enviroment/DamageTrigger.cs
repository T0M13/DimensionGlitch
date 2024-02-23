using UnityEngine;

[ExecuteAlways]
public class DamageTrigger : MonoBehaviour
{
   private void Awake()
   {
      if (!Application.isPlaying)
      {
         if (!GetComponent<BoxCollider2D>())
         {
            BoxCollider2D BoxCollider = gameObject.AddComponent<BoxCollider2D>();
            BoxCollider.isTrigger = true;
         }     
      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      Debug.Log("Damaged the player");
      if (other.TryGetComponent(out Stats Stats))
      {
         if (!Stats.IsDamageable())
         {
            Stats.RecieveDmg();
         } 
      }
   }
}
