using System;
using System.Collections.Generic;
using Manager;
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
    
    void QuickAssignToInventory(InventorySlot InventorySlot)
    {
        if(ItemDataBaseManager.Instance.IsNullItemOrInvalid(InventorySlot.GetCurrentItem().ItemID)) return;

        Item ItemToAdd = ItemDataBaseManager.Instance.GetItemFromDataBase(InventorySlot.GetCurrentItem().ItemID);
        
        //If we have a valid context inventory add the item to this inventory
        if(TryMoveItemToContextInventory(ItemToAdd, InventorySlot)) return;
        
        if (TryAddItemFavorFreeSlots(ItemToAdd, InventorySlot.GetCurrentItemAmount(), InventorySlot))
        {
            InventorySlot.SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
        };
    }

    void QuickAssignToSlotbar(InventorySlot InventorySlot)
    {
        if(ItemDataBaseManager.Instance.IsNullItemOrInvalid(InventorySlot.GetCurrentItem().ItemID)) return;
        
        Item ItemToAdd = ItemDataBaseManager.Instance.GetItemFromDataBase(InventorySlot.GetCurrentItem().ItemID);
        
        if(TryMoveItemToContextInventory(ItemToAdd, InventorySlot)) return;
        
        if(Slotbar.TryAddItemFavorMatchingSlots(ItemToAdd, InventorySlot.GetCurrentItemAmount()))
        {
            InventorySlot.SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
        }
    }

    void DropItem(InventorySlot SlotToDropItemFrom)
    {
        //If we dragged an empty slot do nothing
        if(ItemDataBaseManager.Instance.IsNullItemOrInvalid(SlotToDropItemFrom.GetCurrentItem().ItemID)) return;
        
        ItemDataBaseManager.Instance.CreateItemDrop(SlotToDropItemFrom.GetCurrentItem().ItemID, SlotToDropItemFrom.GetCurrentItemAmount(), InputManager.Instance.GetMousePositionInWorld());
        SlotToDropItemFrom.SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
    }

    bool TryMoveItemToContextInventory(Item ItemToAdd, InventorySlot SlotToMoveItemFrom)
    {
        if (InventoryContextManager.Instance.HasValidContext())
        {
            if (InventoryContextManager.Instance.GetContextInventory()
                .TryAddItemFavorFreeSlots(ItemToAdd, SlotToMoveItemFrom.GetCurrentItemAmount()))
            {
                SlotToMoveItemFrom.SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
                return true;
            }
        }

        return false;
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
