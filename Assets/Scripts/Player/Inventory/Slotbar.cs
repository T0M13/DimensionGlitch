using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class Slotbar : Inventory
{
    [Header("Slotbar")] 
    [SerializeField] Inventory PlayerInventory;
    [SerializeField] InputActionReference[] Hotkeys;
    [SerializeField] GameObject SlotbarSelectionFrame;
    
    void OnEnable()
    {
        MergeSlotbarSlotsWithInventorySlots();
        BindToOnItemSelected();
    }
    void MergeSlotbarSlotsWithInventorySlots()
    {
        PlayerInventory.GetInventorySlots().AddRange(InventorySlots);
    }

    void BindToOnItemSelected()
    {
        foreach (InventorySlot InventorySlot in InventorySlots)
        {
            SlotbarSlot SlotbarSlot = InventorySlot as SlotbarSlot;

            if (SlotbarSlot)
            {
                SlotbarSlot.OnItemSelected += SetSelectionFramPosition;
                SlotbarSlot.OnEmptySlot += SetSelectionFrameInactive;
            }
        }
    }

    void SetSelectionFrameInactive(InventorySlot _)
    {
        SlotbarSelectionFrame.gameObject.SetActive(false);
    }
    void SetSelectionFramPosition(SlotbarSlot SlotbarSlot)
    {
        if (ItemDataBaseManager.Instance.IsNullItemOrInvalid(SlotbarSlot.GetCurrentItem().ItemID))
        {
            SetSelectionFrameInactive(SlotbarSlot);
            return;
        }
        SlotbarSelectionFrame.gameObject.SetActive(true);
        SlotbarSelectionFrame.transform.position = SlotbarSlot.transform.position;
    }
    
#region Initialization
#if UNITY_EDITOR
    
    public override void ReinitializeInventory()
    {
        base.ReinitializeInventory();

        for (int i = 0; i < InventorySlots.Count; ++i)
        {
            SlotbarSlot SlotbarSlot = (SlotbarSlot)InventorySlots[i];
            
            SlotbarSlot.SetHotkey(Hotkeys[i]);
            SlotbarSlot.SetMaxAmountOfItems(PlayerInventory.GetMaxAmountOfItemsPerSlot());
        }
    }

#endif
#endregion
}
