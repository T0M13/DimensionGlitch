using Player.Inventory.Items;
using UnityEngine;

namespace Manager
{
    [CreateAssetMenu(menuName = "NewMode/WateringMode", fileName = "NewWateringMode")]
    public class WateringMode : Mode<WateringCan>
    {
        [SerializeField] float MaxFillAmount = 100.0f;
        [SerializeField] float CurrentFillAmount = 100.0f;
        [SerializeField, Min(0.5f)] float WateringCooldown = 5.0f;

        float LastTimeWatered = 0.0f;
        public override void OnModeEntered(WateringCan ModeItem)
        {
            Debug.Log("Entered the watering mode");
            base.ModeItem = ModeItem;
            MaxFillAmount = ModeItem.GetWaterCapacity();
            LastTimeWatered = 0.0f;
        }

        public override void UpdateMode(PlacementSystem PlacementSystem)
        {
            CellIndicator CellIndicator = PlacementSystem.GetCellIndicator();
            
            //Check if we can water the tile in fron of us meaning that it is an arable tile 
            if (PlacementSystem.IsTileAdjacentToPlayer())
            {
                if (PlacementSystem.IsTileWaterable() && CanUseWaterCan())
                {
                    CellIndicator.SetCellIndicatorActive(true);
                    CellIndicator.SetCellIndicatorColor(true);
                    
                    if (InputManager.Instance.LeftClickWasPerformed())
                    {
                       
                        WaterTile(PlacementSystem);
                        SetLastTimeWatered();
                        
                       
                    }
                }
                else if (CanRefillWaterCan(PlacementSystem))
                {
                    CellIndicator.SetCellIndicatorActive(true);
                    CellIndicator.SetCellIndicatorColor(true);
                    
                    if (InputManager.Instance.LeftClickWasPerformed())
                    {
                        Debug.Log("Refilled the watering can");
                        RefillWateringCan();
                        
                    }
                }
                else
                {
                    CellIndicator.SetCellIndicatorActive(true);
                    CellIndicator.SetCellIndicatorColor(false, Color.red);
                }
              
            }
            else
            {
                CellIndicator.SetCellIndicatorActive(true);
                CellIndicator.SetCellIndicatorColor(false, Color.red);
            }
        }

        public override void OnModeExited()
        {
            Debug.Log("Exited the watering mode");
            ModeItem = null;
        }

        void RefillWateringCan()
        {
            CurrentFillAmount = MaxFillAmount;
        }
        void WaterTile(PlacementSystem PlacementSystem)
        {
            PlacementSystem.WaterTileAtPosition(PlacementSystem.GetCurrentGridPosition(), ModeItem.GetWateringAmountPerUse());
            
            CurrentFillAmount -= ModeItem.GetWateringAmountPerUse();
        }
        
        void SetLastTimeWatered()
        {
            LastTimeWatered = Time.time;
        }

        bool CanRefillWaterCan(PlacementSystem PlacementSystem)
        {
            return PlacementSystem.IsHoveringWater();
        }
        bool CanUseWaterCan()
        {
            float TimeDiff = Time.time - LastTimeWatered;
            
            return TimeDiff > WateringCooldown && CurrentFillAmount >= ModeItem.GetWateringAmountPerUse();
        }
    }
}