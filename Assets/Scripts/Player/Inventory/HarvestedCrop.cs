using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create/Useable/HarvestedCrop", fileName = "HarvestedCrop")]
public class HarvestedCrop : UseableItem
{
    public override void OnUseItem(GameObject User)
    {
        Debug.Log("Used the item");
    }
}
