using System;
using System.Collections.Generic;
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
    
    [Header("Debug")] 
    [SerializeField] PlacementMode CurrentPlacementMode;
    [SerializeField, ShowOnly] Vector3 CurrentMousePosition;
    [SerializeField, ShowOnly] Vector3Int CurrentGridPosition;
    [SerializeField, ShowOnly] Vector3 CurrentCellCenter;
    [SerializeField, ShowOnly] PlayerController CachedPlayerController = null;
    [SerializeField, ShowOnly] Tile CurrentlyHoveredTile;

    Dictionary<Vector3Int, Crop> AllPlacedCrops = new();
    Dictionary<Vector3Int, Building.Building> AllPlacedBuildings = new();
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
        }

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
       
        //Get all tiles blocked by the building and add them to the all blocked tiles
        Vector2Int BuildingGrid = Building.GetBuildingGrid();
        int BuildingHeight = BuildingGrid.x;
        int BuildingWidth = BuildingGrid.y;
        
        for (int x = 0; x < BuildingWidth; x++)
        {
            for (int y = 0; y < BuildingHeight; y++)
            {
                Vector3Int PositionToBlock = CellCoordinates + new Vector3Int(x, y);
                AllBlockedTiles.Add(PositionToBlock);
            }
        }
        
        AllPlacedBuildings.TryAdd(CellCoordinates, Building);
    }

    public void RemoveBuildingAtPosition(Vector3 Position)
    {
        Vector3Int CellCoordinates = WorldGrid.WorldToCell(Position);

        Building.Building BuildingToRemove = AllPlacedBuildings[CellCoordinates];
        
        Vector2Int BuildingGrid = BuildingToRemove.GetBuildingGrid();
        int BuildingHeight = BuildingGrid.x;
        int BuildingWidth = BuildingGrid.y;
        
        for (int x = 0; x < BuildingWidth; x++)
        {
            for (int y = 0; y < BuildingHeight; y++)
            {
                Vector3Int PositionToBlock = CellCoordinates + new Vector3Int(x, y);
                AllBlockedTiles.Remove(PositionToBlock);
            }
        }
        
        AllPlacedBuildings.Remove(CellCoordinates);
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
    private void TrySetCellIndicatorPosition()
    {
        if (CellIndicator.IsIndicatorActive())
        {
            CellIndicator.SetCellIndicatorPosition(CurrentCellCenter);
        }
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
        
        return StatefullTile.IsArable && !AllBlockedTiles.Contains(CellCoordinates);
    }

    public bool CanPlaceBuildingAtCurrentlyHoveredPosition()
    {
        StatefulTile StatefullTile = CurrentlyHoveredTile as StatefulTile;
       
        if (!StatefullTile) 
            return false;
        
        //In here we need to check all positions that this building would take in an place them in the building array
        
        return StatefullTile.CanBuildOn && !AllBlockedTiles.Contains(DynamicTileMap.WorldToCell(CurrentMousePosition));
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
