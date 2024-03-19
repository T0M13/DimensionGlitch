using Player.Inventory.Items;
using UnityEngine;

namespace Manager
{
    [CreateAssetMenu(menuName = "NewMode/BuildingMode", fileName = "NewBuildingMode")]
    public class BuildingMode : Mode<BuildingPortal>
    {
        [SerializeField, Min(1)] float TimeUntilBuildingGetsDestroyed;
        
        Building.Building LastHoveredBuilding = null;
        
        float DestructionTimer = 0.0f;
        public override void OnModeEntered(BuildingPortal ModeItem)
        {
            Debug.Log("Entered the Building mode");
            base.ModeItem = ModeItem;
        }

        public override void UpdateMode(PlacementSystem PlacementSystem)
        {
            CellIndicator CellIndicator = PlacementSystem.GetCellIndicator();
            
            //we want to place a building
            if (InputManager.Instance. RightClickWasReleasedThisFrame())
            {
                Debug.Log("Interrupted destroying a building");
                LastHoveredBuilding = null;
                DestructionTimer = 0.0f;
                return;
            }
            //means we want to destroy a building
            if (InputManager.Instance.RightClickIsPressed() && PlacementSystem.GetBuildingAtCurrentlyHoveredPosition())
            {
                Building.Building CurrentlyHoveredBuilding = PlacementSystem.GetBuildingAtCurrentlyHoveredPosition();
                
                if (!LastHoveredBuilding)
                {
                    LastHoveredBuilding = CurrentlyHoveredBuilding;
                }
                else if (LastHoveredBuilding == CurrentlyHoveredBuilding)
                {
                    DestructionTimer += Time.deltaTime;
                    if (DestructionTimer >= TimeUntilBuildingGetsDestroyed)
                    {
                        Debug.Log("Destroyed the building");
                        DestructionTimer = 0.0f;
                        PlacementSystem.RemoveBuildingAtPosition(PlacementSystem.GetCurrentGridPosition());
                    }
                    //Increment the destruction timer
                }
                else
                {
                    Debug.Log("Stopped destroying a building");
                    DestructionTimer = 0.0f;
                    //reset the destruction timer
                }
                
                return;
            }
          
          
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