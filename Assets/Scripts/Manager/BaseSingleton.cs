using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
   private static T instance = null;
   
   public static T Instance
   {
      get
      {
         if (!instance)
         {
            instance = FindObjectOfType<T>();

            if (!instance)
            {
               instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
            }

            return instance;
         }
        
         return instance;
      }
   }
   
   protected void Awake()
   {
      if (instance && instance != this)
      {
         Destroy(this);
      }
   }
}
