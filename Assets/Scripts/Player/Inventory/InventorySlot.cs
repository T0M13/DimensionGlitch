using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
   [SerializeField, Min(1)] int MaxAmountOfItems;
   [SerializeField] protected int AmountOfItems = 0;
   [SerializeField] protected ItemData CurrentItem;
   [SerializeField] Image InventorySlotDisplay;
   [SerializeField] EventTrigger EventTrigger;

   Sprite DefaultSprite = null;
   public ItemData GetCurrentItem() => CurrentItem;
   public int GetCurrentItemAmount() => AmountOfItems;
   
   void Start()
   {
      CreateMouseEvents();

      DefaultSprite = InventorySlotDisplay.sprite;
   }

   public bool HasSameItem(int ItemID)
   {
      return CurrentItem.ItemID == ItemID;
   }
   public void PlaceItem(Item NewItem, int Amount)
   {
      ItemData NewItemData = NewItem.GetItemData();
    
      
      //Check if its the same item if so then add if not then swap 
      if (NewItemData.ItemID == CurrentItem.ItemID)
      {
         AmountOfItems += Amount;
         
         if (MouseData.CurrentDrag.IsValid())
         {
            //zero the dragged slot items
            MouseData.CurrentDrag.FromSlot.SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
         }
         return;
      }
      
      //else swap with the dragged slots item
      SwapItemsWithDraggedSlot();
   }
   
   /// <summary>
   ///   Adds x Amount to the currently hold item
   /// </summary>
   /// <param name="Amount"></param>
   public void AddToCurrentItem(int Amount)
   {
      AmountOfItems += Amount;
   }

   /// <summary>
   /// Removes x amount from the currently hold item
   /// </summary>
   /// <param name="Amount"></param>
   public void RemoveCurrentItem(int Amount)
   {
      AmountOfItems -= Amount;
      Debug.Log("Removed item");
      if (AmountOfItems < 0)
      {
         Debug.Log("hey we removed more items then there are available");
      }
      if (AmountOfItems <= 0)
      {
         SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
      }
   }
   
   public void SetItem(Item NewItem, int NewAmount)
   {
      CurrentItem = NewItem.GetItemData();
      AmountOfItems = NewAmount;
      InventorySlotDisplay.sprite = NewItem.GetItemData().ItemSprite ? NewItem.GetItemData().ItemSprite : DefaultSprite;
      Debug.Log("Current item is" + CurrentItem.ItemName + "with an amount of " + AmountOfItems);
   }
   void SwapItemsWithDraggedSlot()
   {
      ItemDataBaseManager ItemDataBaseManager = ItemDataBaseManager.Instance;
         
      ItemData FromSlotItemData = MouseData.CurrentDrag.GetCurrentlyDraggedItem();
      ItemData MyCurrentItemData = CurrentItem;
         
      int FromSlotItemAmount = MouseData.CurrentDrag.FromSlot.GetCurrentItemAmount();
      InventorySlot FromSlot = MouseData.CurrentDrag.FromSlot;
         
         
      FromSlot.SetItem(ItemDataBaseManager.GetItemFromDataBase(MyCurrentItemData.ItemID), AmountOfItems);
      SetItem(ItemDataBaseManager.GetItemFromDataBase(FromSlotItemData.ItemID), FromSlotItemAmount);
   }
   
#region MouseEvents

   
   void CreateMouseEvents()
   {
      EventTrigger.Entry OnMouseEnter = new EventTrigger.Entry();
      EventTrigger.Entry OnMouseExit = new EventTrigger.Entry();
      EventTrigger.Entry OnClick = new EventTrigger.Entry();
      EventTrigger.Entry OnDragBegin = new EventTrigger.Entry();
      EventTrigger.Entry OnDragEnd = new EventTrigger.Entry();
      
      OnMouseEnter.eventID = EventTriggerType.PointerEnter;
      OnMouseEnter.callback.AddListener(OnMouseEnterSlot);

      OnMouseExit.eventID = EventTriggerType.PointerExit;
      OnMouseExit.callback.AddListener(OnMouseExitSlot);

      OnClick.eventID = EventTriggerType.PointerClick;
      OnClick.callback.AddListener(OnMouseClickSlot);

      OnDragBegin.eventID = EventTriggerType.BeginDrag;
      OnDragBegin.callback.AddListener(OnMouseBeginDragSlot);

      OnDragEnd.eventID = EventTriggerType.EndDrag;
      OnDragEnd.callback.AddListener(OnMouseEndDragSlot);
      
      EventTrigger.triggers.Add(OnMouseEnter);
      EventTrigger.triggers.Add(OnMouseExit);
      EventTrigger.triggers.Add(OnClick);
      EventTrigger.triggers.Add(OnDragBegin);
      EventTrigger.triggers.Add(OnDragEnd);
      
   }
   void OnMouseEnterSlot(BaseEventData pointerEventData)
   {
      Debug.Log("Entered a slot with item " + CurrentItem.ItemName + "and amount of " + AmountOfItems);
      MouseData.CurrentlyHoveredSlot = this;
   }

   void OnMouseClickSlot(BaseEventData pointerEventData)
   {
     
   }

   void OnMouseExitSlot(BaseEventData pointerEventData)
   {
      
      MouseData.CurrentlyHoveredSlot = null;
   }

   void OnMouseBeginDragSlot(BaseEventData pointerEventData)
   {
     
      MouseData.CurrentDrag = new DragData(this);
   }

   void OnMouseEndDragSlot(BaseEventData pointerEventData)
   {
      //Destroy the dragged visual in here
      if(!MouseData.CurrentlyHoveredSlot) return;
      
      Item DraggedItem = ItemDataBaseManager.Instance.GetItemFromDataBase(CurrentItem.ItemID);
      MouseData.CurrentlyHoveredSlot.PlaceItem(DraggedItem, AmountOfItems);
   }
   

#endregion
   
}
