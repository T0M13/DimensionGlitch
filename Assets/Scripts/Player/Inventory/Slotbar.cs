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
    
    void Start()
    {
        MergeSlotbarSlotsWithInventorySlots();
    }

    void MergeSlotbarSlotsWithInventorySlots()
    {
        PlayerInventory.GetInventorySlots().AddRange(SlotBarSlots);
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
