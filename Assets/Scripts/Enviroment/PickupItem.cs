using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PickupItem : MonoBehaviour, ITweenable<TweenTransform>, ITweenable<Color>
{
    [SerializeField] Item ItemToAdd;
    [SerializeField] SpriteRenderer ItemWorldDisplay;
    [SerializeField] int AmountToAdd;

    [FormerlySerializedAs("TransformRequest")]
    [Header("Animation")] 
    [SerializeField] TweenRequest<TweenTransform> TransformTween;
    [SerializeField] TweenRequest<Color> ColorTween;

    private void Start()
    {
        TransformTween.ToTween = this;
        ColorTween.ToTween = this;
        TransformTween.OnTweenFinished = OnPickUpAnimationFinished;
        TransformTween.From.Position = transform.position;
        TransformTween.To.Position += TransformTween.From.Position;
    }

    public void InitializeItemPickup(Item Item, int Amount)
    {
        ItemToAdd = Item;
        AmountToAdd = Amount;
        ItemWorldDisplay.sprite = Item.GetItemData().ItemSprite;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPickUp();   
           
        }
    }

    void OnPickUp()
    {
        //Disable the collider to hinder double pickup
        var Collider = GetComponent<Collider2D>();
        Collider.enabled = false;
        
        TweenManager.Instance.StartTweenColor(ColorTween);
        TweenManager.Instance.StartTweenTransform(TransformTween);
        HUDManager.Instance.GetPlayerInventory().TryAddItem(ItemToAdd, AmountToAdd);
    }

    void OnPickUpAnimationFinished()
    {
        Destroy(gameObject);
    }

#region PickUpAnimation

    public void SetTween(TweenTransform NewValue)
    {
        transform.position = NewValue.Position;
        transform.localScale = NewValue.Scale;
        transform.eulerAngles = NewValue.EulerRotation;
    }

    public void SetTween(Color NewValue)
    {
        ItemWorldDisplay.color = NewValue;
    }

#endregion

}
