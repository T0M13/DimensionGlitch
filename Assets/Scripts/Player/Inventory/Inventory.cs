using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField, Min(1)] int AmountOfSlotsPerRow;
    [SerializeField, Min(1)] int AmountOfSlotsPerColumn;
    [SerializeField] float SpacingBetweenSlots;
    [SerializeField] InventorySlot InventorySlotTemplate;
    
    [Header("Debug")]
    [SerializeField] protected List<InventorySlot> InventorySlots;

    public List<InventorySlot> GetInventorySlots() => InventorySlots;
    
    public bool TryAddItem(Item ItemToAdd, int Amount)
    {
        if (TryFindSlotWithItem(ItemToAdd.GetItemData().ItemID, out InventorySlot OutSlot))
        {
            OutSlot.AddToCurrentItem(Amount);
            return true;
        }
        if (TryFindFreeSlot(out InventorySlot FreeSlot))
        {
            FreeSlot.SetItem(ItemToAdd, Amount);
            return true;
        }

        return false;
    }

    bool TryFindFreeSlot(out  InventorySlot OutInventorySlot)
    {
        OutInventorySlot = null;
        
        foreach (var InventorySlot in InventorySlots)
        {
            if (InventorySlot.GetCurrentItemAmount() == 0)
            {
                OutInventorySlot = InventorySlot;
                return true;
            }
        }
        
        return false;
    }

    bool TryFindSlotWithItem(int ItemId, out InventorySlot OutInventorySlot)
    {
        OutInventorySlot = null;
        
        foreach (var InventorySlot in InventorySlots)
        {
            if (InventorySlot.HasSameItem(ItemId))
            {
               OutInventorySlot = InventorySlot;
                return true;
            }
        }

        return false;
    }
    
#region InventoryIntitialization
#if UNITY_EDITOR
  public void ReinitializeInventory()
    {
        foreach (var InventorySlot in InventorySlots)
        {
            DestroyImmediate(InventorySlot.gameObject);
        }
        InventorySlots.Clear();
        
        InitializeInventory();
    }
   void InitializeInventory()
    {
        Vector3 StartPos = GetLowerLeftGridStartPoint();
        Vector3 IncrementY = new Vector2(0, SpacingBetweenSlots);
        Vector3 IncrementX = new Vector2(SpacingBetweenSlots, 0);
        
        for (int x = 0; x < AmountOfSlotsPerRow; x++)
        {
            //Copy the initial pos for the upwards grid slots
            Vector3 InitialPos = StartPos;
            
            for (int y = 0; y < AmountOfSlotsPerColumn; y++)
            {
                InventorySlot NewSlot = (InventorySlot)PrefabUtility.InstantiatePrefab(InventorySlotTemplate, transform);
                NewSlot.transform.position = InitialPos;
                InitialPos += IncrementY;
                
                InventorySlots.Add(NewSlot);
            }

            //Afterwards go one spacing to the left
            StartPos += IncrementX;
        }
    }

    Vector3 GetLowerLeftGridStartPoint()
    {
        //We have to takke one less then the actual amount because the for loop also stops invrementing the position one befor max
        float HalfSizeX = (AmountOfSlotsPerRow - 1)  * 0.5f;
        float HalfSizeY = (AmountOfSlotsPerColumn -1) * 0.5f;
        
        float LowerLeftCornerOffsetX = HalfSizeX * SpacingBetweenSlots;
        float LowerLeftCornerOffsetY = HalfSizeY * SpacingBetweenSlots;
        
        Vector3 InventoryCenterPos = transform.position;
        Vector3 LowerLeft = InventoryCenterPos - new Vector3(LowerLeftCornerOffsetX, LowerLeftCornerOffsetY);
        
        return LowerLeft;
    }

#endif
#endregion
  
}
