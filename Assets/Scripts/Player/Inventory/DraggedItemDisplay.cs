using System;
using UnityEngine;
using UnityEngine.UI;

public class DraggedItemDisplay : MonoBehaviour
{
   [SerializeField] Image Displayimage;

   private static Image ItemDisplayStaticRef;

   private void OnEnable()
   {
      ItemDisplayStaticRef = Displayimage;
      SetPosition();
   }

   private void Start()
   {
      SetDisplayActive(false);
   }

   void Update()
   {
      if (gameObject.activeSelf)
      {
         SetPosition();
      }
   }

   static public void SetDisplayActive(bool Active)
   {
      Debug.Log(ItemDisplayStaticRef);
      ItemDisplayStaticRef.gameObject.SetActive(Active);
   }
   static public void SetDisplay(Sprite NewItemSprite)
   {
      ItemDisplayStaticRef.sprite = NewItemSprite;
   }
   void SetPosition()
   {
      transform.position = InputManager.Instance.GetMousePositionScreen();
   }
}
