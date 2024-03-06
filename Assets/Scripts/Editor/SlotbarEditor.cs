using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Slotbar))]
public class SlotbarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("InitializeSlotbar"))
        {
            Slotbar Slotbar = (Slotbar)serializedObject.targetObject;
            
            Slotbar.ReinitializeSlotbar();
        }
    }
}
