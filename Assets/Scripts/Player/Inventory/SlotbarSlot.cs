using UnityEngine;
using UnityEngine.InputSystem;

public class SlotbarSlot : InventorySlot
{
    [Header("SlotbarSlot")]
    [SerializeField] InputActionReference Hotkey;

    public void SetHotkey(InputActionReference NewHotkey) => Hotkey = NewHotkey;
    private void OnEnable()
    {
        Hotkey.action.performed += UseItem;
        OnEmptySlot += OnSlotEmptied;
    }
    
    private void OnDisable()
    {
        Hotkey.action.performed -= UseItem;
        OnEmptySlot -= OnSlotEmptied;
    }

    void OnSlotEmptied(InventorySlot Self)
    {
        GameManager.Instance.GetPlacementSystemRef.ExitCurrentPlacementMode();
    }
    void UseItem(InputAction.CallbackContext _)
    {
        UseableItem UseableItem = ItemDataBaseManager.Instance.GetItemFromDataBase(CurrentItem.ItemID) as UseableItem;
        
        //always reset the current placement mode to default if we should enter a dfiferent mode it will be handled by the item
        GameManager.Instance.GetPlacementSystemRef.ExitCurrentPlacementMode();
        
        if (UseableItem && UseableItem.ShouldRemoveOnInitialUse())
        {
            UseableItem.OnUseItem(GameManager.Instance.GetPlayerControllerRef.gameObject);
            RemoveCurrentItem(1);
        }
        else if (UseableItem)
        {
            UseableItem.OnUseItem(GameManager.Instance.GetPlayerControllerRef.gameObject);
        }
    }
}
