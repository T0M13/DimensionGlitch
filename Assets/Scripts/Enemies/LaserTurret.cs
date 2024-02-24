using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
   [SerializeField, Min(0)] float LaserMaxDistance = 10.0f;
   [SerializeField, Min(0)] float LaserThickness = 10.0f;
   [SerializeField] DirectionContainer DirectionsOfLaseres;
   [SerializeField] Bounds RectangleBounds;
   [SerializeField] Transform[] LaserSpawnPoints;
   [SerializeField] LineRenderer[] LaserLineRenderers;
   [SerializeField] LayerMask HittableLayer;
   
   private void OnValidate()
   {
      if (!Application.isPlaying)
      {
         InitLaserPoints();
      }
   }

   private void FixedUpdate()
   {
      ShootLasers();
      //shoot lasers into four directions
   }

   void InitLaserPoints()
   {
      float RotationStep = 360 / LaserSpawnPoints.Length;
      float CurrentRotation = 0;
      
      for (byte i = 0; i < (byte)DirectionContainer.EDirections.Left; i++)
      {
         //if the direction is set in the mask
         if (((byte)DirectionsOfLaseres.GetDirectionMask() & (1 << i)) != 0)
         {
            Vector2 Offset = DirectionsOfLaseres.GetDirectionForMaskValue((DirectionContainer.EDirections)(1 << i));
            //if i is in range
            if (i < LaserSpawnPoints.Length && i < LaserLineRenderers.Length)
            {
               //set the position to the edge of the box
               Vector2 Center = RectangleBounds.center;
               Vector2 Position = Center + Offset * RectangleBounds.size * 0.5f;
               Vector2 LaserDefaultEndPosition = Center + Offset * LaserMaxDistance;
               
               LaserSpawnPoints[i].gameObject.SetActive(true);
               LaserSpawnPoints[i].transform.position = Position;
               LaserSpawnPoints[i].transform.rotation = Quaternion.AngleAxis(CurrentRotation, Vector3.forward);
               LaserLineRenderers[i].startWidth = LaserThickness;
               LaserLineRenderers[i].endWidth = LaserThickness;
               
               SetLineRenderersPositions(i, Position, LaserDefaultEndPosition);
            }
         }
         else if (i < LaserSpawnPoints.Length && i < LaserLineRenderers.Length)
         {
            LaserSpawnPoints[i].gameObject.SetActive(false);
            LaserLineRenderers[i].startWidth = LaserThickness;
            LaserLineRenderers[i].endWidth = LaserThickness;
         }

         CurrentRotation -= RotationStep;
      }
   }

   void ShootLasers()
   {
      for (int i = 0; i < LaserSpawnPoints.Length; i++)
      {
         Transform LaserSpawnPoint = LaserSpawnPoints[i];
         
         Vector2 RayStartPosition = LaserSpawnPoint.position;
         Vector2 RayDirection = LaserSpawnPoint.up;
         RaycastHit2D Hit = Physics2D.Raycast(RayStartPosition, RayDirection, LaserMaxDistance, HittableLayer);

         if (Hit)
         {
            SetLineRenderersPositions(i, RayStartPosition, Hit.point);

            if (Hit.transform.gameObject.TryGetComponent(out Stats Stats))
            {
               if (Stats.IsDamageable())
               {
                  Stats.RecieveDmg();
               }
            }
         }
         else
         {
            SetLineRenderersPositions(i, RayStartPosition, RayStartPosition + RayDirection * LaserMaxDistance);
         }
      }
   }

   void SetLineRenderersPositions(int LineRendererIndex, Vector2 StartPosition, Vector2 EndPosition)
   {
      LaserLineRenderers[LineRendererIndex].SetPosition(0, StartPosition);
      LaserLineRenderers[LineRendererIndex].SetPosition(1, EndPosition);
   }
   private void OnDrawGizmosSelected()
   {
      if (!Application.isPlaying)
      {
         RectangleBounds.center = transform.position;

         InitLaserPoints();
         Gizmos.color = Color.green;
         Gizmos.DrawWireCube(RectangleBounds.center, RectangleBounds.size);
      }
   }
}
