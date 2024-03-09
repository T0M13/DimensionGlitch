using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Inventory), true)]
public class InventoryEditor : Editor
{
   public override void OnInspectorGUI()
   {
      base.OnInspectorGUI();

      Inventory Inventory = (Inventory)serializedObject.targetObject;
      
      if (GUILayout.Button("InitInventory"))
      {
         Inventory.ReinitializeInventory();
         EditorUtility.SetDirty(Inventory);
      }

      if (GUILayout.Button("AddItemToInventory"))
      {
         bool SuccesfullAdd = Inventory.TryAddItemFavorFreeSlots(ItemDataBaseManager.Instance.GetItemFromDataBase(0), 10);
         
         Debug.Log(SuccesfullAdd);
      }
   }
}
