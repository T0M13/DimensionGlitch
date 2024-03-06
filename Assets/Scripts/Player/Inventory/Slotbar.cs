using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class Slotbar : MonoBehaviour
{
    [Header("Slotbar")] 
    [SerializeField, Range(0, 9)] int NumSlotbarSlots = 0;
    [SerializeField] float SlotbarSpacing = 50.0f;
    [SerializeField] Inventory PlayerInventory;
    [SerializeField] SlotbarSlot SlotbarSlotPrefab;
    [SerializeField, ShowOnly] List<SlotbarSlot> SlotBarSlots;
    [SerializeField] InputActionReference[] Hotkeys;
    
    void OnEnable()
    {
        MergeSlotbarSlotsWithInventorySlots();
    }

    void MergeSlotbarSlotsWithInventorySlots()
    {
        PlayerInventory.GetInventorySlots().AddRange(SlotBarSlots);
    }

    public bool TryAddItemToSlotbarSlot(Item ItemToAdd, int Amount )
    {
        //first search for a slot containing the item
        foreach (var SlotbarSlot in SlotBarSlots)
        {
            if (SlotbarSlot.HasSameItem(ItemToAdd.GetItemData().ItemID))
            {
                SlotbarSlot.AddToCurrentItem(Amount);
                return true;
            }
        }
        
        //If we dont find a slot that already holds the item try o find an empty slot
        foreach (var SlotbarSlot in SlotBarSlots)
        {
            if (SlotbarSlot.IsEmpty())
            {
                SlotbarSlot.SetItem(ItemToAdd, Amount);
                return true;
            }
        }

        return false;
    }
    
#region Initialization
#if UNITY_EDITOR

    public void ReinitializeSlotbar()
    {
        foreach (var SlotbarSlot in SlotBarSlots)
        {
            DestroyImmediate(SlotbarSlot.gameObject);
        }
        
        SlotBarSlots.Clear();
        
        InitializeSlotbar();
    }
    void InitializeSlotbar()
    {
        float WholeTakenSpace = (NumSlotbarSlots -1)  * SlotbarSpacing;
        float HalfSpace = WholeTakenSpace * 0.5f;
        
        Vector3 Increment = new Vector3(SlotbarSpacing, 0, 0);
        Vector3 SpawnPos = transform.position + new Vector3(-HalfSpace, 0, 0);
        
        for (int i = 0; i < NumSlotbarSlots; i++)
        {
            var NewSlotbarSlot = (SlotbarSlot)PrefabUtility.InstantiatePrefab(SlotbarSlotPrefab, transform);
            NewSlotbarSlot.transform.position = SpawnPos;
            NewSlotbarSlot.SetHotkey(Hotkeys[i]);
            SpawnPos += Increment;
            
            SlotBarSlots.Add(NewSlotbarSlot);
        }
    }

#endif
#endregion
}
