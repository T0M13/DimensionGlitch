using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor
{
   public override void OnInspectorGUI()
   {
      base.OnInspectorGUI();

      Inventory Inventory = (Inventory)serializedObject.targetObject;
      
      if (GUILayout.Button("InitInventory"))
      {
         Inventory.ReinitializeInventory();
      }

      if (GUILayout.Button("AddItemToInventory"))
      {
         bool SuccesfullAdd = Inventory.TryAddItem(ItemDataBaseManager.Instance.GetItemFromDataBase(0), 10);
         
         Debug.Log(SuccesfullAdd);
      }
   }
}
