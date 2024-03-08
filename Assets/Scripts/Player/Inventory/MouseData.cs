using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DragData
{
    public DragData(InventorySlot FromSlot)
    {
        this.FromSlot = FromSlot;
    }
    
    public InventorySlot FromSlot;

    public bool IsValid()
    {
        return FromSlot != null;
    }

    public void Invalidate()
    {
        FromSlot = null;
    }

    public ItemData GetCurrentlyDraggedItem()
    {
        return FromSlot.GetCurrentItem();
    }
}
public static class MouseData
{
    public static InventorySlot CurrentlyHoveredSlot = null;
    public static DragData CurrentDrag;
    
}
