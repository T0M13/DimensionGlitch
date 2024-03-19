using UnityEngine;

namespace Manager
{
    [CreateAssetMenu(menuName = "NewMode/HarvestMode", fileName = "NewHarvestingMode")]
    public class HarvestingMode : Mode<Tool>
    {
        public override void OnModeEntered(Tool PlaceableSeed)
        {
            ModeItem = PlaceableSeed;
            Debug.Log("Entered the harvesting mode");
        }

        public override void UpdateMode(PlacementSystem PlacementSystem)
        {
            CellIndicator CellIndicator = PlacementSystem.GetCellIndicator();
            
            if (PlacementSystem.IsTileAdjacentToPlayer())
            {
                if (PlacementSystem.CanHarvestAtCurrentlyHoveredPosition())
                {
                    CellIndicator.SetCellIndicatorActive(true);
                    CellIndicator.SetCellIndicatorColor(false, Color.white);
                    
                    if (InputManager.Instance.LeftClickWasPerformed())
                    {
                        //harvest at given position
                        HarvestCropAtPosition(PlacementSystem);
                    }
                }
                else
                {
                    CellIndicator.SetCellIndicatorColor(false, Color.red);
                }
            }
            else
            {
                CellIndicator.SetCellIndicatorActive(false);
            }
        }

        public override void OnModeExited()
        {
            Debug.Log("Exited the harvesting mode");
            ModeItem = null;
        }

        public void HarvestCropAtPosition(PlacementSystem PlacementSystem)
        {
            Crop CropToHarvest = PlacementSystem.GetCropAtCurrentlyHoveredPosition();
            
            if (CropToHarvest.HarvestCrop())
            {
                PlacementSystem.RemoveCropAtPosition(PlacementSystem.GetCurrentGridPosition());   
            };
        }
    }
}