using System;
using System.Collections;
using System.Collections.Generic;
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
    }
    
    private void OnDisable()
    {
        Hotkey.action.performed -= UseItem;
    }

    void UseItem(InputAction.CallbackContext _)
    {
        UseableItem UseableItem = ItemDataBaseManager.Instance.GetItemFromDataBase(CurrentItem.ItemID) as UseableItem;
        
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
