
using UnityEditor;
using UnityEngine;

public abstract class ObjectPoolEditor<T> : Editor where T : Component
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw the default Inspector for the target

        ObjectPool<T> ObjectPool = (ObjectPool<T>)target;

        // Check if the target is a prefab instance and disable the button if it is
        if (PrefabUtility.IsPartOfPrefabInstance(ObjectPool.gameObject))
        {
            EditorGUILayout.HelpBox("This operation cannot be performed on prefab instances.", MessageType.Warning);
            GUI.enabled = false; // Disable the button
        }

        // Create a button to fill the array and set the GameObject inactive
        if (GUILayout.Button("Init ObjectPool"))
        {
            FillObjectPool(ObjectPool);
            EditorUtility.SetDirty(ObjectPool);
        }

        GUI.enabled = true; // Re-enable GUI
    }

    void FillObjectPool(ObjectPool<T> ObjectPool)
    {
        ObjectPool.ReinitPool();
    }
}
