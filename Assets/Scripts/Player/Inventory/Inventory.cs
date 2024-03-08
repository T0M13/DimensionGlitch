using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField, Min(1)] int AmountOfSlotsPerRow;
    [SerializeField, Min(1)] int AmountOfSlotsPerColumn;
    [SerializeField, Min(10)] int MaxAmountOfItemsPerSlot = 10;
    [SerializeField] float SpacingBetweenSlots;
    [SerializeField] protected InventorySlot InventorySlotTemplate;
   
    [Header("Debug")]
    [SerializeField] protected List<InventorySlot> InventorySlots;

    public int GetMaxAmountOfItemsPerSlot() => MaxAmountOfItemsPerSlot;
    public List<InventorySlot> GetInventorySlots() => InventorySlots;

    public bool TryAddItem(Item ItemToAdd, int Amount, InventorySlot SlotToIgnore = null)
    {
        if (TryFindSlotsWithItem(ItemToAdd.GetItemData().ItemID, out List<InventorySlot> OutSlots, SlotToIgnore))
        {
            //First look if any of the found slots can fit the item amount we want to add
            foreach (var InventorySlot in OutSlots)
            {
                if (InventorySlot.CanFitItem(Amount))
                {
                    InventorySlot.AddToCurrentItem(Amount);
                    return true;
                }
            }
            
            OutSlots.AddRange(GetAllFreeSlots());
            //first find out if all free slots can fit the amount that we want to add
            int FreeSpace = 0;
            foreach (var SlotWithFreeSpace in OutSlots)
            {
                FreeSpace += SlotWithFreeSpace.GetFreeSlotSpace();
            }

            //Means that we can fit the item into this slot
            if (FreeSpace >= Amount)
            {
                foreach (var SlotWithFreeSpace in OutSlots)
                {
                    int DiffToAdd = Amount - SlotWithFreeSpace.GetFreeSlotSpace();
                    Amount -= SlotWithFreeSpace.GetFreeSlotSpace();

                    if (DiffToAdd >= 0)
                    {
                        SlotWithFreeSpace.SetItem(ItemToAdd,SlotWithFreeSpace.GetMaxAmountOfItems());
                        Debug.Log(SlotWithFreeSpace.GetCurrentItemAmount());
                    }
                    else
                    {
                        SlotWithFreeSpace.SetItem(ItemToAdd, SlotWithFreeSpace.GetCurrentItemAmount() + SlotWithFreeSpace.GetFreeSlotSpace() + Amount);
                        Debug.Log(SlotWithFreeSpace.GetCurrentItemAmount());
                        break;
                    }
                }
                
                return true;
            }

            return false;
        }
        if (TryFindFreeSlot(out InventorySlot FreeSlot))
        {
            //If we can fit all items in the first free slot just return true
            if (FreeSlot.CanFitItem(Amount))
            {
                FreeSlot.SetItem(ItemToAdd, Amount);
                return true;
            }
          
            List<InventorySlot> AllFreeSlots = GetAllFreeSlots();

            int FreeSlotSpace = 0;
            foreach (var FreeInventorySlot in AllFreeSlots)
            {
                FreeSlotSpace += FreeInventorySlot.GetFreeSlotSpace();
            }

            if (FreeSlotSpace >= Amount)
            {
                foreach (var SlotWithFreeSpace in AllFreeSlots)
                {
                    int DiffToAdd = Amount - SlotWithFreeSpace.GetFreeSlotSpace();
                    Amount -= SlotWithFreeSpace.GetFreeSlotSpace();

                    if (DiffToAdd >= 0)
                    {
                        SlotWithFreeSpace.SetItem(ItemToAdd, SlotWithFreeSpace.GetMaxAmountOfItems());
                        Debug.Log(SlotWithFreeSpace.GetCurrentItemAmount());
                    }
                    else
                    {
                        SlotWithFreeSpace.SetItem(ItemToAdd,
                            SlotWithFreeSpace.GetCurrentItemAmount() + SlotWithFreeSpace.GetFreeSlotSpace() +
                            Amount);
                        Debug.Log(SlotWithFreeSpace.GetCurrentItemAmount());
                        break;
                    }
                }

                return true;
            }
        }
        
        return false;
    }
    
    protected List<InventorySlot> GetAllFreeSlots()
    {
        List<InventorySlot> FreeSlots = new();

        foreach (var InventorySlot in InventorySlots)
        {
            if (InventorySlot.IsEmpty())
            {
                FreeSlots.Add(InventorySlot);
            }
        }

        return FreeSlots;
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

    public bool TryFindSlotsWithItem(int ItemId, out List<InventorySlot> OutInventorySlots, InventorySlot SlotToIgnore = null)
    {
        OutInventorySlots = new List<InventorySlot>();
        bool FoundSlotWithSameItem = false;
        
        foreach (var InventorySlot in InventorySlots)
        {
            if (InventorySlot.HasSameItem(ItemId))
            {
                if (SlotToIgnore != InventorySlot)
                {
                    OutInventorySlots.Add(InventorySlot);
                    FoundSlotWithSameItem = true;
                }
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
  public virtual void ReinitializeInventory()
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
                NewSlot.SetMaxAmountOfItems(MaxAmountOfItemsPerSlot);
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
