using Player.Inventory.Items;
using UnityEngine;

namespace Manager
{
    [CreateAssetMenu(menuName = "NewMode/BuildingMode", fileName = "NewBuildingMode")]
    public class BuildingMode : Mode<BuildingPortal>
    {
        public override void OnModeEntered(BuildingPortal ModeItem)
        {
            Debug.Log("Entered the Building mode");
            base.ModeItem = ModeItem;
        }

        public override void UpdateMode(PlacementSystem PlacementSystem)
        {
            CellIndicator CellIndicator = PlacementSystem.GetCellIndicator();
            
            if (!PlacementSystem.IsTileAdjacentToPlayer() || !PlacementSystem.CanPlaceBuildingAtCurrentlyHoveredPosition())
            {
                CellIndicator.SetCellIndicatorActive(true);
                CellIndicator.SetCellIndicatorColor(false, Color.red);    
                return;
            }
            
            CellIndicator.SetCellIndicatorActive(true);
            CellIndicator.SetCellIndicatorColor(true);
            
            if (InputManager.Instance.LeftClickWasPerformed())
            {
                //Place the building
                PlaceBuilding(PlacementSystem);
            }
        }

        public override void OnModeExited()
        {
            Debug.Log("Exited the Building mode");
            ModeItem = null;
        }

        void PlaceBuilding(PlacementSystem PlacementSystem)
        {
            Debug.Log("Placed a building");
            
            Vector3 SpawnPosition = PlacementSystem.GetCurrentGridPosition();

            HUDManager.Instance.GetPlayerInventory().RemoveAmountOfItems(ModeItem.GetItemData().ItemID, 1);
            Building.Building SpawnedBuilding = Instantiate(ModeItem.GetBuildingToSpawn(), SpawnPosition, Quaternion.identity);
            PlacementSystem.AddBuildingAtPosition(SpawnPosition, SpawnedBuilding);
        }
    }
}