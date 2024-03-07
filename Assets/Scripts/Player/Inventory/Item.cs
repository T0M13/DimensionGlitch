using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create/Generic", fileName = "NewItem")]
public class Item : ScriptableObject
{
    [SerializeField] ItemData ItemData;
    [SerializeField, ShowOnly] int HashCode = 0;
    
#if UNITY_EDITOR
    public void SetItemID(int NewItemId) => ItemData.ItemID = NewItemId;
#endif
    public ItemData GetItemData() => ItemData;

    public void GenerateHashCode()
    { 
        unchecked // Overflow is fine, just wrap
        {
            int hash = 17; // Prime number to start with
            hash = hash * 23 + ItemData.ItemName.GetHashCode();
            hash = hash * 23 + ItemData.ItemValue.GetHashCode();
            hash = hash * 23 + ItemData.ItemID.GetHashCode();
            if (ItemData.ItemSprite)
            {
                hash = hash * 23 + ItemData.ItemSprite.GetHashCode();
                HashCode = hash;
            }
        }
        
    }
    public override int GetHashCode()
    {
        return HashCode;
    }
}

[System.Serializable]
public struct ItemData
{
    [SerializeField] public string ItemName;
    [SerializeField] public int ItemValue;
    [SerializeField] public int ItemID;
    [SerializeField] public Sprite ItemSprite;
}
