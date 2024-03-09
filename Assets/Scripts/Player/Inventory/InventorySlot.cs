using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
   [SerializeField, Min(1)] int MaxAmountOfItems;
   [SerializeField] protected int AmountOfItems = 0;
   [SerializeField] protected ItemData CurrentItem;
   [SerializeField] Image InventorySlotDisplay;
   [SerializeField] TextMeshProUGUI AmountOfItemsDisplay;
   [SerializeField] EventTrigger EventTrigger;
   [SerializeField] Sprite DefaultSprite;

   float LastTimeClicked = 0.0f;

   public int GetMaxAmountOfItems() => MaxAmountOfItems;
   public void SetMaxAmountOfItems(int NewAmountOfItems) => MaxAmountOfItems = NewAmountOfItems;
   public event Action<InventorySlot> OnDoubleClickSlot;
   public event Action<InventorySlot> OnMouseShiftClickSlot;
   public event Action<InventorySlot> OnMouseAltClickSlot;
   public event Action<InventorySlot> OnEndDragWithoutValidSlot;
   public event Action<InventorySlot> OnItemDroppedIntoSlot;
   public event Action<InventorySlot> OnEnteredSlot;
   public event Action<InventorySlot> OnExitSlot;
   public event Action<InventorySlot> OnEmptySlot;
   
   Coroutine DoubleClickRoutine = null;
   
   public ItemData GetCurrentItem() => CurrentItem;
   public int GetCurrentItemAmount() => AmountOfItems;
   
   void Start()
   {
      CreateMouseEvents();

      if (ItemDataBaseManager.Instance.IsNullItemOrInvalid(CurrentItem.ItemID))
      {
         InventorySlotDisplay.sprite = DefaultSprite;
      }
   }

   public void CallOnEmptySlot()
   {
      OnEmptySlot?.Invoke(this);
   }
   public bool HasSameItem(int ItemID)
   {
      return CurrentItem.ItemID == ItemID;
   }

   public bool IsFull()
   {
      return AmountOfItems == MaxAmountOfItems;
   }
   public bool CanFitItem(int AmountToAdd)
   {
      return AmountOfItems + AmountToAdd <= MaxAmountOfItems;
   }

   public int GetFreeSlotSpace()
   {
      return MaxAmountOfItems - AmountOfItems;
   }
   public bool IsEmpty()
   {
      return AmountOfItems <= 0;
   }
   public void PlaceItem(Item NewItem, int Amount)
   {
      ItemData NewItemData = NewItem.GetItemData();
      
      //Check if its the same item if so then add if not then swap 
      if (NewItemData.ItemID == CurrentItem.ItemID && CanFitItem(Amount))
      {
         AmountOfItems += Amount;
         AmountOfItemsDisplay.SetText(AmountOfItems.ToString());
         
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
      AmountOfItemsDisplay.SetText(AmountOfItems.ToString());
   }

   /// <summary>
   /// Removes x amount from the currently hold item, also returns true if it was the last item being removed
   /// </summary>
   /// <param name="Amount"></param>
   public void RemoveCurrentItem(int Amount)
   {
      AmountOfItems -= Amount;
      AmountOfItemsDisplay.SetText(AmountOfItems.ToString());
      
      if (AmountOfItems < 0)
      {
         Debug.Log("hey we removed more items then there are available");
      }
      if (IsEmpty())
      {
         SetItem(ItemDataBaseManager.Instance.GetNullItem(), 0);
      }
   }
   
   public void SetItem(Item NewItem, int NewAmount)
   {
      CurrentItem = NewItem.GetItemData();
      AmountOfItems = NewAmount;
      InventorySlotDisplay.sprite = NewItem.GetItemData().ItemSprite ? NewItem.GetItemData().ItemSprite : DefaultSprite;
      AmountOfItemsDisplay.SetText(NewAmount.ToString());
      
      OnItemDroppedIntoSlot?.Invoke(this);
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
      MouseData.CurrentlyHoveredSlot = this;
      OnEnteredSlot?.Invoke(this);
   }

   void OnMouseClickSlot(BaseEventData pointerEventData)
   {
      if (InputManager.Instance.IsShiftPressed())
      {
         Debug.Log("Shift clicked a slot");
         OnMouseShiftClickSlot?.Invoke(this);
         return;
      }
      if (InputManager.Instance.IsAltPressed())
      {
         Debug.Log("Alt Clicked slot");
         OnMouseAltClickSlot?.Invoke(this);
         return;
      }
      if (DoubleClickRoutine == null)
      {
         DoubleClickRoutine = StartCoroutine(CheckForDoubleClick());
         return;
      }
      
      StopCoroutine(DoubleClickRoutine);
      DoubleClickRoutine = null;
         
      OnDoubleClickSlot?.Invoke(this);
   }

   void OnMouseExitSlot(BaseEventData pointerEventData)
   {
      MouseData.CurrentlyHoveredSlot = null;
      OnExitSlot?.Invoke(this);
   }

   void OnMouseBeginDragSlot(BaseEventData pointerEventData)
   {
      if (!ItemDataBaseManager.Instance.IsNullItemOrInvalid(CurrentItem.ItemID))
      {
         DraggedItemDisplay.SetDisplayActive(true);
         DraggedItemDisplay.SetDisplay(CurrentItem.ItemSprite);
         MouseData.CurrentDrag = new DragData(this);
      }
   }

   void OnMouseEndDragSlot(BaseEventData pointerEventData)
   {
      //Destroy the dragged visual in here
      DraggedItemDisplay.SetDisplayActive(false);
      DraggedItemDisplay.SetDisplay(null);

      if (!MouseData.CurrentDrag.IsValid()) return;
      //Only swap items if we are over another slot
      if (!MouseData.CurrentlyHoveredSlot)
      {
         OnEndDragWithoutValidSlot?.Invoke(this);
         return;
      }
      if (MouseData.CurrentlyHoveredSlot == this)
      {
         return;
      }
      
      Item DraggedItem = ItemDataBaseManager.Instance.GetItemFromDataBase(CurrentItem.ItemID);
      MouseData.CurrentlyHoveredSlot.PlaceItem(DraggedItem, AmountOfItems);
      MouseData.CurrentDrag.Invalidate();
   }

   IEnumerator CheckForDoubleClick()
   {
      float PassedTime = 0.0f;
      while (PassedTime < 1.0f)
      {
         PassedTime += Time.deltaTime;
         yield return null;
      }

      DoubleClickRoutine = null;
   }
#endregion
}
