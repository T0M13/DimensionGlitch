using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/DataBases", fileName = "NewDataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField] List<Item> AllItems;
    
    public List<Item> GetAllItems() => AllItems;
    
    public Item GetItemFromDataBase(int ItemID)
    {
        if (ItemID < 0 || AllItems.Count < ItemID) return null;
        
        return AllItems[ItemID];
    }
}
