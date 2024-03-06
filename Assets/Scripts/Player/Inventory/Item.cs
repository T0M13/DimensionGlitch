using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create/Generic", fileName = "NewItem")]
public class Item : ScriptableObject
{
    [SerializeField] ItemData ItemData;

#if UNITY_EDITOR
    public void SetItemID(int NewItemId) => ItemData.ItemID = NewItemId;
#endif
    public ItemData GetItemData() => ItemData;
}

[System.Serializable]
public struct ItemData
{
    [SerializeField] public string ItemName;
    [SerializeField] public int ItemValue;
    [SerializeField] public int ItemID;
    [SerializeField] public Sprite ItemSprite;
}
