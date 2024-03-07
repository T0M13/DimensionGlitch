using UnityEngine;

public abstract class UseableItem : Item
{
   [SerializeField] bool RemoveOnIntialUse = false;

   public bool ShouldRemoveOnInitialUse() => RemoveOnIntialUse;

   public abstract void OnUseItem(GameObject User);
}
