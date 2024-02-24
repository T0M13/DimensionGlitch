using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingMovement : MonoBehaviour
{
   [SerializeField] float RotationSpeed = 10.0f;

   private void Update()
   {
      Rotate();
   }

   void Rotate()
   {
      transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);
   }
}
