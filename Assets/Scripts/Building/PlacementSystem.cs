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
    [FormerlySerializedAs("harvestingMode")] [FormerlySerializedAs("HarvestinMode")] [SerializeField] HarvestingMode HarvestingMode;
    [SerializeField] PlantingMode PlantingMode;

    [Header("Debug")] 
    [SerializeField] PlacementMode CurrentPlacementMode;
    [SerializeField, ShowOnly] Vector3 CurrentMousePosition;
    [SerializeField, ShowOnly] Vector3Int CurrentGridPosition;
    [SerializeField, ShowOnly] Vector3 CurrentCellCenter;
    [SerializeField, ShowOnly] PlayerController CachedPlayerController = null;
    [SerializeField, ShowOnly] Tile CurrentlyHoveredTile;

    Dictionary<Vector3Int, Crop> AllPlacedCrops = new();

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
            case PlacementMode.Farming:
            {
                HarvestingMode.UpdateMode(this);
                break;
            }
            case PlacementMode.Planting:
            {
                PlantingMode.UpdateMode(this);
                break;
            }
        }

    }
    
   
    public void SetPlacementMode(PlacementMode NewMode)
    {
        CurrentPlacementMode = NewMode;
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
            case PlacementMode.Farming:
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

    public bool IsTileAdjacentOrDiagonalToPlayer()
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

    public void AddCropAtPosition(Vector3 Position, Crop Crop)
    {
        Vector3Int CellCoordinates = WorldGrid.WorldToCell(Position);
        
        if (!AllPlacedCrops.ContainsKey(CellCoordinates))
        {
            AllPlacedCrops.Add(CellCoordinates, Crop);
        }
    }
    
    public void RemoveCropAtPosition(Vector3 Position)
    {
        Vector3Int CellCoordinates = WorldGrid.WorldToCell(Position);
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
        StatefulTile tile = DynamicTileMap.GetTile<StatefulTile>(CurrentGridPosition);
       
        if (!tile) 
            return false;

        Vector3Int CellCoordinates = WorldGrid.WorldToCell(Position);
        
        return tile.IsArable && !AllPlacedCrops.ContainsKey(CellCoordinates);
    }
  
    
    // #region Harvesting Mode
    //
    public void EnterHarvestingMode(Tool tool)
    {
        SetPlacementMode(PlacementMode.Farming);
        //enter Harvesting mode here
        HarvestingMode.OnModeEntered(tool);
    }
    // private void FarmAtPosition(Vector3 position)
    // {
    //     Vector3Int gridPosition = WorldGrid.WorldToCell(position);
    //     //Farm
    //     if (farmableGameObjectsAtPositions.TryGetValue(gridPosition, out GameObject farmableObject))
    //     {
    //         switch (selectedTool.ResourceType)
    //         {
    //             case ResourceType.Crop:
    //                 FarmCrop(farmableObject);
    //                 break;
    //             case ResourceType.Tree:
    //                 break;
    //             case ResourceType.Stone:
    //                 break;
    //             default:
    //                 break;
    //         }
    //     }
    //
    // }
    //
    // private void FarmCrop(GameObject cropGameObject)
    // {
    //     Crop cropTemp = cropGameObject.GetComponent<Crop>();
    //
    //     if (cropTemp.ResourceType == selectedTool.ResourceType)
    //     {
    //         if (!cropTemp.TryHarvesting()) return;
    //         farmableGameObjectsAtPositions.Remove(gridPosition);
    //     }
    // }
    // #endregion
    //
    //
    // #region Building Mode
    //
    //
    // public void EnterBuildingMode(BuildingPortal buildingToBuild)
    // {
    //     ChangePlacementMode(PlacementMode.Building);
    //     SelectedBuildingPortal = buildingToBuild;
    // }
    //
    //
    // public void TryPlaceBuilding()
    // {
    //     if (isAdjacentOrDiagonal)
    //     {
    //         if (CanBuildAtPosition(gridPosition))
    //         {
    //             CellIndicator.SetActive(true);
    //             redCellIndicator.SetActive(false);
    //             if (Input.GetMouseButtonDown(0) && SelectedBuildingPortal)
    //             {
    //                 PlaceBuildAtPosition(mouseHoverCellPosition);
    //             }
    //         }
    //         else
    //         {
    //             CellIndicator.SetActive(false);
    //             redCellIndicator.SetActive(true);
    //         }
    //     }
    //     else
    //     {
    //         CellIndicator.SetActive(false);
    //         redCellIndicator.SetActive(false);
    //     }
    //
    // }
    //
    // public bool CanBuildAtPosition(Vector3Int gridPosition)
    // {
    //     StatefulTile tile = tilemapBuildable.GetTile<StatefulTile>(gridPosition);
    //
    //     if (tile && tile.canBuildOn && !buildableGameObjectsAtPositions.ContainsKey(gridPosition))
    //     {
    //         return true;
    //     }
    //
    //     return false;
    // }
    //
    // private void PlaceBuildAtPosition(Vector3 position)
    // {
    //     Vector3Int gridPosition = WorldGrid.WorldToCell(position);
    //     Building.Building newBuild = Instantiate(SelectedBuildingPortal.GetBuildingToSpawn(), position, Quaternion.identity);
    //     newBuild.transform.SetParent(tilemapCrops.transform);
    //     HUDManager.Instance.GetPlayerInventory().RemoveAmountOfItems(SelectedBuildingPortal.GetItemData().ItemID, 1);
    //     buildableGameObjectsAtPositions.Add(gridPosition, newBuild.gameObject);
    // }
    // #endregion
    //
    // public string GetTileInfo(Vector3Int gridPosition)
    // {
    //     StatefulTile tile = tilemapBuildable.GetTile<StatefulTile>(gridPosition);
    //     if (tile != null)
    //     {
    //         return $"StatefulTile at {gridPosition}, BuildOn: {tile.canBuildOn} , Arable: {tile.isArable}";
    //     }
    //     else
    //     {
    //         return $"No StatefulTile found at {gridPosition}.";
    //     }
    // }
    //
    //
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
