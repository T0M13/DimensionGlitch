using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBaseManager : BaseSingleton<ItemDataBaseManager>
{
   //Consider putting this into groups and assigning items groups for faster search and possibly easy unlockable items
   [SerializeField] List<ItemDataBase> ItemDataBases;
   [SerializeField] Item NullItem;
   
   public Item GetItemFromDataBase(int ItemId)
   {
      Item ItemFromDataBase = ItemDataBases[0].GetItemFromDataBase(ItemId);
      return ItemFromDataBase ? ItemFromDataBase : GetNullItem();
   }

   public Item GetNullItem()
   {
      return NullItem;
   }

   /// <summary>
   /// check if something contains a null item, meaning the slot is empty or in general inaccesible
   /// </summary>
   /// <param name="ItemId"></param>
   /// <returns></returns>
   public bool IsNullItemOrInvalid(int ItemId)
   {
      return ItemId == NullItem.GetItemData().ItemID || ItemId < 0;
   }
}
