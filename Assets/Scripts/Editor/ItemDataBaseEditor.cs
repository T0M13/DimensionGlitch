using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemDataBase))]
public class ItemDataBaseEditor : Editor
{
   public override void OnInspectorGUI()
   {
      base.OnInspectorGUI();

      ItemDataBase ItemDataBase = (ItemDataBase)serializedObject.targetObject;

      if (GUILayout.Button("ReinitializeItemIDS"))
      {
         
         for (int i = 0; i < ItemDataBase.GetAllItems().Count; i++)
         {
            ItemDataBase.GetAllItems()[i].SetItemID(i);
            ItemDataBase.GetAllItems()[i].GenerateHashCode();
            EditorUtility.SetDirty(ItemDataBase.GetAllItems()[i]);
         }
      }
   }
}
