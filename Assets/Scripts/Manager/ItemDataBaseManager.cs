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
}
