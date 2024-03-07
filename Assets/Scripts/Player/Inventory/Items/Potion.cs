using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create/Useable/Potion", fileName = "NewPotion")]
public class Potion : UseableItem
{
    public override void OnUseItem(GameObject User)
    {
        Debug.Log("Used the item");
    }
}
