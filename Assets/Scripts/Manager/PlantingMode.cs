using UnityEngine;

namespace Manager
{
    [CreateAssetMenu(menuName = "NewMode/PlantingMode", fileName = "NewPlantingMode")]
    public class PlantingMode : Mode<PlaceableSeed>
    {
        public override void OnModeEntered(PlaceableSeed ModeItem)
        {
            this.ModeItem = ModeItem;
            Debug.Log("Entered the Planting mode");
        }

        public override void UpdateMode(PlacementSystem PlacementSystem)
        {
            CellIndicator CellIndicator = PlacementSystem.GetCellIndicator();
            Vector3 PositionToPlaceSeedAt = PlacementSystem.GetCurrentGridPosition();
            
            if(!PlacementSystem.IsTileAdjacentToPlayer() || !PlacementSystem.CanPlantCropsAtPosition(PositionToPlaceSeedAt))
            {
                CellIndicator.SetCellIndicatorActive(true);
                CellIndicator.SetCellIndicatorColor(false, Color.red);
                return;
            }
           
            if (InputManager.Instance.LeftClickWasPerformed())
            { 
               
                PlaceSeedAtPosition(PositionToPlaceSeedAt, PlacementSystem);
            }
            
            CellIndicator.SetCellIndicatorActive(true);
            CellIndicator.SetCellIndicatorColor(false, Color.white);
        }

        public override void OnModeExited()
        {
            Debug.Log("Exited the Planting mode");
            ModeItem = null;
        }

        void PlaceSeedAtPosition(Vector3 Position, PlacementSystem PlacementSystem)
        {
            HUDManager.Instance.GetPlayerInventory().RemoveAmountOfItems(ModeItem.GetItemData().ItemID, 1);
            Crop PlacedCrop = Instantiate(ModeItem.GetCropPrefab(), Position, Quaternion.identity);
            PlacementSystem.AddCropAtPosition(Position, PlacedCrop);
        }
    }
}