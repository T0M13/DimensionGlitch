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
    
    void OnEnable()
    {
        MergeSlotbarSlotsWithInventorySlots();
        
    }
    void MergeSlotbarSlotsWithInventorySlots()
    {
        PlayerInventory.GetInventorySlots().AddRange(InventorySlots);
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
