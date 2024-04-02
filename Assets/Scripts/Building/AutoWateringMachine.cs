using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AutoWateringMachine : Building.Building
{
   [SerializeField] int WateringRange = 2;
   [SerializeField, Min(0)] float WateringAmount = 10.0f;
   [SerializeField, ShowOnly] Vector3Int CellCoordinatesOfBuilding;

   float CooldownBetweenWatering = 2.0f;
   float LastTimeWatered = 5.0f;
   
   private void Start()
   {
      CellCoordinatesOfBuilding = PlacementSystem.Instance.GetCellCoordinatesForPosition(transform.position);
   }

   private void Update()
   {
      if (Time.time - LastTimeWatered > CooldownBetweenWatering)
      {
         WaterTiles();
         LastTimeWatered = Time.time;
      }   
   }

   Vector3Int GetLowerLeftWateringBounds()
   {
      Vector3Int WateringExtents = new Vector3Int(WateringRange, WateringRange, 0);
      Vector3Int LowerLeftBoundsCoordinates = CellCoordinatesOfBuilding - WateringExtents;

      return LowerLeftBoundsCoordinates;
   }
   
   Vector3Int GetUpperRightWateringBounds()
   {
      Vector3Int WateringExtents = new Vector3Int(WateringRange, WateringRange, 0);
      Vector3Int LowerLeftBoundsCoordinates = CellCoordinatesOfBuilding + WateringExtents;

      return LowerLeftBoundsCoordinates;
   }

   /// <summary>
   /// key = Position of tile , Value = tile itsself
   /// </summary>
   /// <returns></returns>
   List<KeyValuePair<Vector3,StatefulTile>> GetAllWaterableTilesInRange()
   {
      List<KeyValuePair<Vector3,StatefulTile>> AllWaterableTiles = new ();
      Vector3Int UpperLeftCornerOfWateringGrid = GetUpperRightWateringBounds() - GetLowerLeftWateringBounds();
      
      int MaxX = UpperLeftCornerOfWateringGrid.x;
      int MaxY = UpperLeftCornerOfWateringGrid.y;
     
      for (int x = 0; x <= MaxX ; x++)
      {
         Vector3Int PositionIncrease = new Vector3Int(x, 0);
         
         for (int y = 0; y <= MaxY; y++)
         {
            PositionIncrease.y = y;
            
            if (PlacementSystem.Instance.TryGetStatefullTileAtCellCoordinates(GetLowerLeftWateringBounds() + PositionIncrease,
                   out StatefulTile FoundTile, out Vector3 TilePosition))
            {
               if (FoundTile.IsArable)
               {
                  AllWaterableTiles.Add(new KeyValuePair<Vector3, StatefulTile>(TilePosition, FoundTile));      
               }
            }
         }
      }

      return AllWaterableTiles;
   }

   void WaterTiles()
   {
      var WaterableTiles = GetAllWaterableTilesInRange();

      foreach (var WaterableTile in WaterableTiles)
      {
         PlacementSystem.Instance.WaterTileAtPosition(WaterableTile.Key, WateringAmount);
      }
   }
   private void OnDrawGizmosSelected()
   {
      if(!Application.isPlaying)
         return;
      
      Gizmos.color = Color.red;

      var WaterableTilesInRange = GetAllWaterableTilesInRange();
      
      foreach (var Tile in WaterableTilesInRange)
      {
            Gizmos.DrawWireCube(Tile.Key, Vector3.one);
      }
   }
}

