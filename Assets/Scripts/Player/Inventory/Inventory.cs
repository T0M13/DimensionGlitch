using System;
using System.Collections.Generic;
using System.Linq;
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
        if (TryFindSlotsWithItem(ItemToAdd.GetItemData().ItemID, out List<InventorySlot> OutSlots))
        {
            Debug.Log("Added to current item");
            //Always add to the first found slot that is a valid slot
            OutSlots[0].AddToCurrentItem(Amount);
            return true;
        }
        if (TryFindFreeSlot(out InventorySlot FreeSlot))
        {
            Debug.Log("Set current item");
            FreeSlot.SetItem(ItemToAdd, Amount);
            return true;
        }

        Debug.Log("Found no slot");
        return false;
    }

    protected bool TryFindFreeSlot(out  InventorySlot OutInventorySlot)
    {
        OutInventorySlot = null;
        
        foreach (var InventorySlot in InventorySlots)
        {
            if (ItemDataBaseManager.Instance.IsNullItemOrInvalid(InventorySlot.GetCurrentItem().ItemID))
            {
                OutInventorySlot = InventorySlot;
                return true;
            }
        }
        
        return false;
    }

    public bool TryFindSlotsWithItem(int ItemId, out List<InventorySlot> OutInventorySlots)
    {
        OutInventorySlots = new List<InventorySlot>();
        bool FoundSlotWithSameItem = false;
        
        foreach (var InventorySlot in InventorySlots)
        {
            if (InventorySlot.HasSameItem(ItemId))
            {
                OutInventorySlots.Add(InventorySlot);
                FoundSlotWithSameItem = true;
            }
        }

        return FoundSlotWithSameItem;
    }

    public void RemoveAmountOfItems(int ItemID, int AmountToRemove)
    {
        if (TryFindSlotsWithItem(ItemID, out List<InventorySlot> SlotsWithItem))
        {
            foreach (var InventorySlot in SlotsWithItem)
            {
                AmountToRemove -= InventorySlot.GetCurrentItemAmount();

                if (AmountToRemove >= 0)
                {
                    InventorySlot.RemoveCurrentItem(InventorySlot.GetCurrentItemAmount());
                    InventorySlot.CallOnEmptySlot();
                }
                else
                {
                    int Overshoot = Mathf.Abs(AmountToRemove);
                    InventorySlot.RemoveCurrentItem(InventorySlot.GetCurrentItemAmount() - Overshoot);
                    break;
                }
            }
        }
    }
    //get the amount of a specific item inside of the inventory
    public int GetAmountOfItemInInventory(int ItemID)
    {
        int AmountOfItem = 0;
     
        foreach (var InventorySlot in InventorySlots)
        {
            if (InventorySlot.HasSameItem(ItemID))
            {
                AmountOfItem += InventorySlot.GetCurrentItemAmount();
            }
        }

        return AmountOfItem;
    }
    public bool ContainsItem(int ItemID)
    {
        foreach (var InventorySlot in InventorySlots)
        {
            if (InventorySlot.GetCurrentItem().ItemID == ItemID)
            {
                return true;
            }
        }

        return false;
    }

    public List<InventorySlot> GetSlotsWithItem(int ItemID)
    {
        List<InventorySlot> InventorySlotsContainingItem = new List<InventorySlot>();
        
        foreach (var InventorySlot in InventorySlots)
        {
            if (InventorySlot.HasSameItem(ItemID))
            {
                InventorySlotsContainingItem.Add(InventorySlot);
            }
        }

        return InventorySlotsContainingItem;
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
