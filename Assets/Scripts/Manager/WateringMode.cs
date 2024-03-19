using Player.Inventory.Items;
using UnityEngine;

namespace Manager
{
    [CreateAssetMenu(menuName = "NewMode/WateringMode", fileName = "NewWateringMode")]
    public class WateringMode : Mode<WateringCan>
    {
        public override void OnModeEntered(WateringCan ModeItem)
        {
            Debug.Log("Entered the watering mode");
            base.ModeItem = ModeItem;
        }

        public override void UpdateMode(PlacementSystem PlacementSystem)
        {
            //Check if we can water the tile in fron of us meaning that it is an arable tile 
        }

        public override void OnModeExited()
        {
            Debug.Log("Exited the watering mode");
            ModeItem = null;
        }
    }
}