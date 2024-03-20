using UnityEngine;

namespace Player.Inventory.Items
{
    [CreateAssetMenu(menuName = "Items/Create/Useable/WateringCan", fileName = "NewWateringCan")]
    public class WateringCan : UseableItem
    {
        [SerializeField, Min(1.0f)] float WateringAmountPerUse = 5.0f;
        [SerializeField, Min(1.0f)] float WaterCapacity;

        public float GetWateringAmountPerUse() => WateringAmountPerUse;
        public float GetWaterCapacity() => WaterCapacity;
        
        public override void OnUseItem(GameObject User)
        {
            PlacementSystem.Instance.EnterWateringMode(this);
            Debug.Log("Using the watering can");
        }
    }
}