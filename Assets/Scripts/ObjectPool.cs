using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : Component
{
   [SerializeField] private int NumberOfObjectsToPool = 10;
   [SerializeField] protected T ObjectPrefab;
   [SerializeField] protected List<T> PooledObjects;
   
   protected T GetAvailableObject()
   {
      foreach (var PooledObject in PooledObjects)
      {
         bool IsActive = PooledObject.gameObject.activeSelf;
         
         if(!IsActive)
         {
            return PooledObject;
         }
      }

      T NewObject = Instantiate(ObjectPrefab, transform.position, Quaternion.identity);
      NewObject.transform.SetParent(transform);
      
      PooledObjects.Add(NewObject);
      
      return NewObject;
   }

   public abstract T GetObjectFromPool(Vector2 InitialPosition);
   
   public void ReinitPool()
   {
      ClearPool();
      InitPool();
   }
   void ClearPool()
   {
      for (int i = 0; i < PooledObjects.Count; i++)
      {
         DestroyImmediate(PooledObjects[i].gameObject);
      }
      
      PooledObjects.Clear();
   }
   void InitPool()
   {
      if(!ObjectPrefab) return;
      
      for (int i = 0; i < NumberOfObjectsToPool; i++)
      {
         T SpawnedObject = (T)PrefabUtility.InstantiatePrefab(ObjectPrefab, transform);
         PooledObjects.Add(SpawnedObject);
         SpawnedObject.transform.SetParent(transform);
         SpawnedObject.gameObject.SetActive(false);
      }
   }
}
