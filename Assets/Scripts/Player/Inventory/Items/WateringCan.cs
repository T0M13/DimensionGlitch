using UnityEngine;

namespace Player.Inventory.Items
{
    [CreateAssetMenu(menuName = "Items/Create/Useable/WateringCan", fileName = "NewWateringCan")]
    public class WateringCan : UseableItem
    {
        public override void OnUseItem(GameObject User)
        {
            PlacementSystem.Instance.EnterWateringMode(this);
        }
    }
}