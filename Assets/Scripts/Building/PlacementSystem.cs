using System;
using System.Collections.Generic;
using Building;
using Manager;
using Player.Inventory.Items;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class PlacementSystem : BaseSingleton<PlacementSystem>
{

    [Header("GeneralSettings")] 
    [SerializeField] uint PlacementRadius;
    [SerializeField] Grid WorldGrid;
    [SerializeField] Tilemap DynamicTileMap;
    [SerializeField] CellIndicator CellIndicator;
    [SerializeField] HarvestingMode HarvestingMode;
    [SerializeField] PlantingMode PlantingMode;
    [SerializeField] BuildingMode BuildingMode;
    [SerializeField] WateringMode WateringMode;

    [Header("Planting")] [SerializeField, Min(5)] float MinWateringAmountToPlant = 5.0f;
    
    [Header("Debug")] 
    [SerializeField] PlacementMode CurrentPlacementMode;
    [SerializeField, ShowOnly] Vector3 CurrentMousePosition;
    [SerializeField, ShowOnly] Vector3Int CurrentGridPosition;
    [SerializeField, ShowOnly] Vector3 CurrentCellCenter;
    [SerializeField, ShowOnly] PlayerController CachedPlayerController = null;
    [SerializeField, ShowOnly] Tile CurrentlyHoveredTile;

    Dictionary<Vector3Int, Crop> AllPlacedCrops = new();
    Dictionary<Vector3Int, Building.BuilidngPositionsPair> AllPlacedBuildings = new();
    Dictionary<Vector3Int, float> AllWateredTiles = new();
    HashSet<Vector3Int> AllBlockedTiles = new();

    public Vector3 GetCurrentGridPosition() => CurrentCellCenter; 
    public CellIndicator GetCellIndicator() => CellIndicator;
    
    private void Start()
    {
        CachedPlayerController = GameManager.Instance.GetPlayerControllerRef;
    }

    private void Update()
    {
        CurrentMousePosition = InputManager.Instance.GetMousePositionInWorld();
        CurrentGridPosition  = WorldGrid.WorldToCell(CurrentMousePosition);
        CurrentCellCenter = WorldGrid.GetCellCenterWorld(CurrentGridPosition);
        CurrentlyHoveredTile = DynamicTileMap.GetTile<Tile>(CurrentGridPosition);
        TrySetCellIndicatorPosition();
        
        switch (CurrentPlacementMode)
        {
            case PlacementMode.Harvesting:
            {
                HarvestingMode.UpdateMode(this);
                break;
            }
            case PlacementMode.Planting:
            {
                PlantingMode.UpdateMode(this);
                break;
            }
            case PlacementMode.Building:
            {
                BuildingMode.UpdateMode(this);
                break;
            }
            case PlacementMode.Watering:
            {
                WateringMode.UpdateMode(this);
                break;
            }
        }

    }

    public void PaintTileAtPosition(Vector3 Position ,Tile TileToPaint)
    {
        Vector3Int CellCoordinates = DynamicTileMap.WorldToCell(Position);
        
        DynamicTileMap.SetTile(CellCoordinates, TileToPaint);
    }
    public void SetPlacementMode(PlacementMode NewMode)
    {
        CurrentPlacementMode = NewMode;
    }
    public void EnterHarvestingMode(Tool tool)
    {
        //enter Harvesting mode here
        SetPlacementMode(PlacementMode.Harvesting);
        HarvestingMode.OnModeEntered(tool);
    }

    public void EnterBuildingMode(BuildingPortal BuildingPortal)
    {
        //enter the building mode
        SetPlacementMode(PlacementMode.Building);
        BuildingMode.OnModeEntered(BuildingPortal);
    }
    public void EnterWateringMode(WateringCan WateringCan)
    {
        SetPlacementMode(PlacementMode.Watering);
        WateringMode.OnModeEntered(WateringCan);
        //enter the watering mode
    }
    
    public void ExitCurrentPlacementMode()
    {
        switch (CurrentPlacementMode)
        {
            case PlacementMode.None:
                break;
            case PlacementMode.Planting:
                PlantingMode.OnModeExited();
                break;
            case PlacementMode.Harvesting:
                HarvestingMode.OnModeExited();
                break;
            case PlacementMode.Building:
                BuildingMode.OnModeExited();
                break;
            case PlacementMode.Watering:
                WateringMode.OnModeExited();
                break;
            default:
               break;
        }
        
        SetPlacementMode(PlacementMode.None);
        
        CellIndicator.SetCellIndicatorActive(false);
    }

    public bool IsTileAdjacentToPlayer()
    {
        Vector3 PlayerPosition = CachedPlayerController.transform.position;
        Vector3Int PlayerGridPosition = DynamicTileMap.WorldToCell(PlayerPosition);
        Vector3 PlayerGridCellCenterPos = DynamicTileMap.GetCellCenterWorld(PlayerGridPosition);
        Vector3 GridPositionDifference = (CurrentCellCenter - PlayerGridCellCenterPos);
       
        bool IsAdjacentOrDiagonal = Mathf.Abs(GridPositionDifference.x) <= PlacementRadius 
                                    && Mathf.Abs(GridPositionDifference.y) <= PlacementRadius;
        
        return IsAdjacentOrDiagonal;
    }

    public bool CanHarvestAtCurrentlyHoveredPosition()
    {
        return AllPlacedCrops.ContainsKey(CurrentGridPosition);
    }

    public void AddBuildingAtPosition(Vector3 Position, Building.Building Building)
    {
        Vector3Int CellCoordinates = WorldGrid.WorldToCell(Position);
        Vector2Int BuildingMeasurements = Building.GetBuildingGrid();
        BuilidngPositionsPair PlacedBuilding =
            new BuilidngPositionsPair(BuildingMeasurements.x, BuildingMeasurements.y, Building);
        
        //Get all tiles blocked by the building and add them to the all blocked tiles
        Vector2Int BuildingGrid = Building.GetBuildingGrid();
        int BuildingHeight = BuildingGrid.x;
        int BuildingWidth = BuildingGrid.y;
        
        for (int x = 0; x < BuildingWidth; x++)
        {
            for (int y = 0; y < BuildingHeight; y++)
            {
                Vector3Int PositionToBlock = CellCoordinates + new Vector3Int(x, y);
                PlacedBuilding.CellsThisBuildingIsBuiltOn.Add(PositionToBlock);
                
                AllPlacedBuildings.TryAdd(PositionToBlock, PlacedBuilding);
                AllBlockedTiles.Add(PositionToBlock);
            }
        }
    }

    public void RemoveBuildingAtPosition(Vector3 Position)
    {
        Vector3Int CellCoordinates = WorldGrid.WorldToCell(Position);

        BuilidngPositionsPair BuildingToRemove = AllPlacedBuildings[CellCoordinates];

        foreach (var CellThisBuildingIsBuiltOn in BuildingToRemove.CellsThisBuildingIsBuiltOn)
        {
            AllPlacedBuildings.Remove(CellThisBuildingIsBuiltOn);
            AllBlockedTiles.Remove(CellThisBuildingIsBuiltOn);
        }
        
        Destroy(BuildingToRemove.Building.gameObject);
    }
    public void AddCropAtPosition(Vector3 Position, Crop Crop)
    {
        Vector3Int CellCoordinates = WorldGrid.WorldToCell(Position);
       
        AllBlockedTiles.Add(CellCoordinates);
        AllPlacedCrops.TryAdd(CellCoordinates, Crop);
    }
    
    public void RemoveCropAtPosition(Vector3 Position)
    {
        Vector3Int CellCoordinates = WorldGrid.WorldToCell(Position);
       
        AllBlockedTiles.Remove(CellCoordinates);
        AllPlacedCrops.Remove(CellCoordinates);
    }
    
    public Crop GetCropAtCurrentlyHoveredPosition()
    {
        return AllPlacedCrops.TryGetValue(CurrentGridPosition, out var crop) ? crop : null;
    }

    public Building.Building GetBuildingAtCurrentlyHoveredPosition()
    {
        return   AllPlacedBuildings.TryGetValue(CurrentGridPosition, out var Building) ? Building.Building : null;
    }
    private void TrySetCellIndicatorPosition()
    {
        if (CellIndicator.IsIndicatorActive())
        {
            CellIndicator.SetCellIndicatorPosition(CurrentCellCenter);
        }
    }

    public void WaterTileAtPosition(Vector3 Position, float WateringAmount)
    {
        Vector3Int CellCoordinates = DynamicTileMap.WorldToCell(Position);

        //If there is already a watered tile at this position then just add water to it
        if (AllWateredTiles.ContainsKey(CellCoordinates))
        {
            //Should clamp the max watering amount
            AllWateredTiles[CellCoordinates] += WateringAmount;
            Debug.Log(AllWateredTiles[CellCoordinates] + " Watered an already watered tile");
            return;
        }
        
        //If ther isnt add the cell and 
        Debug.Log( " Watered an unwatered tile");
        AllWateredTiles.Add(CellCoordinates, WateringAmount);
    }

    void RemoveWateredTileAtPosition(Vector3 Position)
    {
        Vector3Int CellCoordinates = DynamicTileMap.WorldToCell(Position);
        AllWateredTiles.Remove(CellCoordinates);
    }
    bool IsCurrentlyHoveringStateFullTile()
    {
        return CurrentlyHoveredTile as StatefulTile;
    }
    public void EnterPlantingMode(PlaceableSeed seed)
    {
        SetPlacementMode(PlacementMode.Planting);
        //Enter the planting mode here
        PlantingMode.OnModeEntered(seed);
    }
    public bool CanPlantCropsAtPosition(Vector3 Position)
    {
        StatefulTile StatefullTile = CurrentlyHoveredTile as StatefulTile;
       
        if (!StatefullTile) 
            return false;

        Vector3Int CellCoordinates = WorldGrid.WorldToCell(Position);

        if (AllWateredTiles.TryGetValue(CellCoordinates, out float WateringAmount))
        {
            return StatefullTile.IsArable 
                   && WateringAmount >= MinWateringAmountToPlant 
                   && !AllBlockedTiles.Contains(CellCoordinates);
        }
       
        return false;
    }

    public bool CanPlaceBuildingAtCurrentlyHoveredPosition()
    {
        StatefulTile StatefullTile = CurrentlyHoveredTile as StatefulTile;
       
        if (!StatefullTile) 
            return false;
        
        //In here we need to check all positions that this building would take in an place them in the building array
        
        return StatefullTile.CanBuildOn && !AllBlockedTiles.Contains(DynamicTileMap.WorldToCell(CurrentMousePosition));
    }

    public bool IsTileWaterable()
    {
        StatefulTile StatefullTile = CurrentlyHoveredTile as StatefulTile;
       
        if (!StatefullTile) 
            return false;
        
        return true;
    }
    
    private void OnDrawGizmos()
    {
        if(!CachedPlayerController) return;
        
        Gizmos.color = Color.blue;
        Vector3Int CellCoordinates = DynamicTileMap.WorldToCell(CachedPlayerController.transform.position);
        Gizmos.DrawWireCube(WorldGrid.GetCellCenterWorld(CellCoordinates), Vector3.one);
        Gizmos.DrawWireSphere(CurrentCellCenter, 2);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(CurrentCellCenter, Vector3.one);
    }
}
