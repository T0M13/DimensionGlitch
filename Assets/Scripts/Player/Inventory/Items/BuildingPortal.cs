using UnityEngine;

namespace Player.Inventory.Items
{
    [CreateAssetMenu(menuName = "Items/Create/Useable/Building", fileName = "PlaceableSeed")]
    public class BuildingPortal : UseableItem
    {
        [SerializeField] Building.Building BuildingToBuild;
        
        public override void OnUseItem(GameObject User)
        {
            Debug.Log("Entered the building mode");
            //Enter the building mode with this item        
        }
    }
}