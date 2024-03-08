using System.Collections.Generic;
using Player.Inventory;
using UnityEngine;

public class PlayerInventory : Inventory
{
    [SerializeField] ItemDescription ItemDescription;
    [SerializeField] Slotbar Slotbar;
    private void Start()
    {
        BindToSlotEvents();
    }

    void BindToSlotEvents()
    {
        foreach (var InventorySlot in InventorySlots)
        {
            if(InventorySlot == null) continue;
            
            if (InventorySlot is SlotbarSlot)
            {
                //Bind to on double click to move the item from the slotbar slot to the 
                InventorySlot.OnDoubleClickSlot += QuickAssignToInventory;
                InventorySlot.OnMouseShiftClickSlot += QuickAssignToInventory;
                InventorySlot.OnMouseAltClickSlot += SplitStack;
                InventorySlot.OnEndDragWithoutValidSlot += DropItem;
            }
            else if (InventorySlot is InventorySlot)
            {
                InventorySlot.OnDoubleClickSlot += QuickAssignToSlotbar;
                InventorySlot.OnMouseShiftClickSlot += QuickAssignToSlotbar;
                InventorySlot.OnMouseAltClickSlot += SplitStack;
                InventorySlot.OnEndDragWithoutValidSlot += DropItem;
            }

            InventorySlot.OnEnteredSlot += ActivateItemDescription;
            InventorySlot.OnExitSlot += DeactivateItemDescription;
        }
    }

    void SplitStack(InventorySlot InventorySlot)
    {
        if(ItemDataBaseManager.Instance.IsNullItemOrInvalid(InventorySlot.GetCurrentItem().ItemID)) return;
        if(InventorySlot.GetCurrentItemAmount() <= 1) return;

        Item ClickedSlotItem = ItemDataBaseManager.Instance.GetItemFromDataBase(InventorySlot.GetCurrentItem().ItemID);
        int SplitStackAmount = Mathf.RoundToInt(Mathf.Floor(InventorySlot.GetCurrentItemAmount() * 0.5f));

        if (TryFindFreeSlot(out InventorySlot FreeSlotForStack))
        {
            FreeSlotForStack.SetItem(ClickedSlotItem, SplitStackAmount);
            InventorySlot.RemoveCurrentItem(SplitStackAmount);
        }
    }
    void QuickAssignToInventory(InventorySlot InventorySlot)
    {
        if(ItemDataBaseManager.Instance.IsNullItemOrInvalid(InventorySlot.GetCurrentItem().ItemID)) return;
        
        Debug.Log("Double clicked slotbar slot");
        Item ItemToAdd = ItemDataBaseManager.Instance.GetItemFromDataBase(InventorySlot.GetCurrentItem().ItemID);

        //If the count is bigger then one of the found slots it means that we have found a slot except ourselve that contains the item
        if (TryFindSlotsWithItem(ItemToAdd.GetItemData().ItemID, out List<InventorySlot> SlotsWithSameItem) && SlotsWithSameItem.Count > 1)
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
        else if (TryFindFreeSlot(out InventorySlot FreeSlot))
        {
            FreeSlot.SetItem(ItemToAdd, InventorySlot.GetCurrentItemAmount());
            InventorySlot.SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
        }
    }

    void QuickAssignToSlotbar(InventorySlot InventorySlot)
    {
        if(ItemDataBaseManager.Instance.IsNullItemOrInvalid(InventorySlot.GetCurrentItem().ItemID)) return;
        
        Item ItemToAdd = ItemDataBaseManager.Instance.GetItemFromDataBase(InventorySlot.GetCurrentItem().ItemID);
        
        if(Slotbar.TryAddItemToSlotbarSlot(ItemToAdd, InventorySlot.GetCurrentItemAmount()))
        {
            InventorySlot.SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
            Debug.Log("Double clicked inventory slot");
        }
    }

    void DropItem(InventorySlot SlotToDropItemFrom)
    {
        ItemDataBaseManager.Instance.CreateItemDrop(SlotToDropItemFrom.GetCurrentItem().ItemID, SlotToDropItemFrom.GetCurrentItemAmount(), InputManager.Instance.GetMousePositionInWorld());
        SlotToDropItemFrom.SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
    }

    void ActivateItemDescription(InventorySlot EnteredSlot)
    {
        if(ItemDataBaseManager.Instance.IsNullItemOrInvalid(EnteredSlot.GetCurrentItem().ItemID)) return;
        if(MouseData.CurrentDrag.IsValid()) return;
        
        //Cast to rect transform to offset by half the width and half the heigth
        RectTransform RectTransform = (RectTransform)ItemDescription.transform;
        Rect Rect = RectTransform.rect;
        Vector3 Offset = new Vector2(Rect.width * 0.5f, Rect.height * 0.5f);
        
        ItemDescription.transform.position = InputManager.Instance.GetMousePositionScreen() + Offset;
        ItemDescription.SetItemDescription(EnteredSlot.GetCurrentItem(), EnteredSlot.GetCurrentItemAmount());
        ItemDescription.SetDescriptionActive(true);
    }

    void DeactivateItemDescription(InventorySlot ExitedSlot)
    {
        ItemDescription.SetDescriptionActive(false);
    }
}
