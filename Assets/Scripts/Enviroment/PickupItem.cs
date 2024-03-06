using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] Item ItemToAdd;
    [SerializeField] int AmountToAdd;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HUDManager.Instance.GetPlayerInventory().TryAddItem(ItemToAdd, AmountToAdd);
            Debug.Log("Added an item");
        }
    }
}
