using System;
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

    private void OnDisable()
    {
        ItemDescription.SetDescriptionActive(false);
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
        
        Item ItemToAdd = ItemDataBaseManager.Instance.GetItemFromDataBase(InventorySlot.GetCurrentItem().ItemID);

        if (TryAddItem(ItemToAdd, InventorySlot.GetCurrentItemAmount(), InventorySlot))
        {
            InventorySlot.SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
        };
    }

    void QuickAssignToSlotbar(InventorySlot InventorySlot)
    {
        if(ItemDataBaseManager.Instance.IsNullItemOrInvalid(InventorySlot.GetCurrentItem().ItemID)) return;
        
        Item ItemToAdd = ItemDataBaseManager.Instance.GetItemFromDataBase(InventorySlot.GetCurrentItem().ItemID);
        
        if(Slotbar.TryAddItem(ItemToAdd, InventorySlot.GetCurrentItemAmount()))
        {
            InventorySlot.SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
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
