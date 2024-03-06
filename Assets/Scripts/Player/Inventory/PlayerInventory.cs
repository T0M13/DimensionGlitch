using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    [SerializeField] Slotbar Slotbar;
    private void Start()
    {
        BindToDoubleClick();
    }

    void BindToDoubleClick()
    {
        foreach (var InventorySlot in InventorySlots)
        {
            if (InventorySlot is SlotbarSlot)
            {
                //Bind to on double click to move the item from the slotbar slot to the 
                InventorySlot.OnDoubleClickSlot += OnDoubleClickSlotbarSlot;
            }
            else if (InventorySlot is InventorySlot)
            {
                InventorySlot.OnDoubleClickSlot += OnDoubleClickInventorySlot;
                Debug.Log("Inventory Slots");
            }
        }
    }

    void OnDoubleClickSlotbarSlot(InventorySlot InventorySlot)
    {
        if(ItemDataBaseManager.Instance.IsNullItemOrInvalid(InventorySlot.GetCurrentItem().ItemID)) return;
        
        Debug.Log("Double clicked slotbar slot");
        Item ItemToAdd = ItemDataBaseManager.Instance.GetItemFromDataBase(InventorySlot.GetCurrentItem().ItemID);

        if (TryFindFreeSlot(out InventorySlot FreeSlot))
        {
            FreeSlot.SetItem(ItemToAdd, InventorySlot.GetCurrentItemAmount());
            InventorySlot.SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
        }
        else if (TryFindSlotsWithItem(ItemToAdd.GetItemData().ItemID, out List<InventorySlot> SlotsWithSameItem))
        {
            foreach (var SlotWithSameItem in SlotsWithSameItem)
            {
                if (!SlotWithSameItem.Equals(InventorySlot))
                {
                    SlotWithSameItem.AddToCurrentItem(InventorySlot.GetCurrentItemAmount());
                    InventorySlot.SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
                    return;
                }
            }
        }
    }

    void OnDoubleClickInventorySlot(InventorySlot InventorySlot)
    {
        if(ItemDataBaseManager.Instance.IsNullItemOrInvalid(InventorySlot.GetCurrentItem().ItemID)) return;
        
        Item ItemToAdd = ItemDataBaseManager.Instance.GetItemFromDataBase(InventorySlot.GetCurrentItem().ItemID);
        
        if(Slotbar.TryAddItemToSlotbarSlot(ItemToAdd, InventorySlot.GetCurrentItemAmount()))
        {
            InventorySlot.SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
            Debug.Log("Double clicked inventory slot");
        }
    }
}
