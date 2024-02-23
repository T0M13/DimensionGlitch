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

   private void OnTriggerEnter(Collider other)
   {
      if (other.TryGetComponent(out Stats Stats))
      {
         Stats.RecieveDmg();
      }
   }
}
