using System;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;


public class InteractionHandler : MonoBehaviour
{
   [SerializeField, Min(1)] float InteractionRange;
   [SerializeField] InputActionReference InteractionInputAction;
   [SerializeField] LayerMask InteractableLayer;

   private void OnEnable()
   {
      InteractionInputAction.action.performed += Interact;
   }

   private void OnDisable()
   {
      InteractionInputAction.action.performed -= Interact;
   }

   void Interact(InputAction.CallbackContext _)
   {
      Debug.Log("Interacted");
      Collider2D[] FoundInteractables = Physics2D.OverlapCircleAll(transform.position, InteractionRange, InteractableLayer);

      float Nearest = float.MaxValue;
      IInteractable NearestInteractable = null;
      
      foreach (var FoundInteractable in FoundInteractables)
      {
         if (FoundInteractable.TryGetComponent(out IInteractable Interactable))
         {
            float DistanceToInteractable = Vector2.Distance(transform.position, FoundInteractable.gameObject.transform.position);
            if (DistanceToInteractable < Nearest)
            {
               Nearest = DistanceToInteractable;
               NearestInteractable = Interactable;
            }  
         }
      }
      
      NearestInteractable?.OnInteract(gameObject);
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.black;
      Gizmos.DrawWireSphere(transform.position, InteractionRange);
   }
}
