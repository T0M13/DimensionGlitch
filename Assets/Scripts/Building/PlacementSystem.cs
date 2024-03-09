using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlacementSystem : BaseSingleton<PlacementSystem>
{
    [Header("References")]
    [SerializeField][ShowOnly] private PlayerController CachedPlayerController = null;

    [Header("Grid")]
    [SerializeField] private Grid grid;
    [SerializeField][ShowOnly] private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();
    private Dictionary<Vector3Int, GameObject> farmableGameObjectsAtPositions = new Dictionary<Vector3Int, GameObject>();

    [Header("Cells")]
    [SerializeField] private GameObject normalCellIndicator;
    [SerializeField] private GameObject redCellIndicator;

    [Header("Builds")]
    [SerializeField] private Tilemap tilemapBuildable;
    //[SerializeField] private BUILD? selectedBuild;

    [Header("Farming")]
    [SerializeField] private Tool selectedTool;
    [Header("Crops")]
    [SerializeField] private Tilemap tilemapCrops;
    [SerializeField] private PlaceableSeed selectedSeed;
    [Header("Debug")]
    [Header("PlacementMode")]
    [SerializeField] private PlacementMode currentPlacementMode = PlacementMode.None;
    [Header("Mouse Position")]
    [SerializeField][ShowOnly] private Vector3 mouseHoverCellPosition;
    [SerializeField][ShowOnly] private bool isAdjacentOrDiagonal;
    [Header("Current Mouse Position On Grid")]
    [SerializeField][ShowOnly] private Vector3Int gridPosition;
    //[SerializeField][ShowOnly] private string gridInfo;
    [Header("Current Player Grid Settings")]
    [SerializeField][ShowOnly] private Vector3 playerPosition;
    [SerializeField][ShowOnly] private Vector3 playerCellPosition;
    [SerializeField] private float playerPostionSphere = 0.2f;
    [SerializeField] private Vector3 playerPositionOffset = new Vector3(0, -0.33f, 0);
    public PlacementMode CurrentPlacementMode { get => currentPlacementMode; set => currentPlacementMode = value; }

    private void Start()
    {
        CachedPlayerController = GameManager.Instance.GetPlayerControllerRef;
    }

    private void Update()
    {
        Vector3 mousePosition = InputManager.Instance.GetMousePositionInWorld();
        gridPosition = grid.WorldToCell(mousePosition);
        mouseHoverCellPosition = grid.GetCellCenterWorld(gridPosition);

        playerPosition = CachedPlayerController.transform.position;
        Vector3Int playerGridPosition = grid.WorldToCell(playerPosition + playerPositionOffset);
        playerCellPosition = grid.GetCellCenterWorld(playerGridPosition);

        normalCellIndicator.transform.position = mouseHoverCellPosition;
        redCellIndicator.transform.position = mouseHoverCellPosition;

        Vector3Int gridPositionDifference = gridPosition - playerGridPosition;
        isAdjacentOrDiagonal = Mathf.Abs(gridPositionDifference.x) <= CachedPlayerController.GetPlayerStats().BuildingRadius && Mathf.Abs(gridPositionDifference.y) <= CachedPlayerController.GetPlayerStats().BuildingRadius;

        //gridInfo = GetTileInfo(gridPosition);

        switch (CurrentPlacementMode)
        {
            case PlacementMode.None:
                //Nichts
                break;
            case PlacementMode.Planting:
                PlaceSeed();
                break;
            case PlacementMode.Farming:
                Farm();
                break;
            case PlacementMode.Building:
                break;
            default:
                break;
        }

    }

    public void ChangePlacementMode(PlacementMode newMode)
    {
        currentPlacementMode = newMode;
    }


    public void ExitCurrentPlacementMode()
    {
        ChangePlacementMode(PlacementMode.None);
        selectedSeed = null;
        selectedTool = null;
        normalCellIndicator.SetActive(false);
        redCellIndicator.SetActive(false);
    }

    #region Planting Mode

    public void EnterPlantingMode(PlaceableSeed seed)
    {
        ChangePlacementMode(PlacementMode.Planting);
        selectedSeed = seed;
    }


    public void PlaceSeed()
    {
        if (isAdjacentOrDiagonal)
        {
            if (CanPlantCropsAtPosition(gridPosition))
            {
                normalCellIndicator.SetActive(true);
                redCellIndicator.SetActive(false);
                if (Input.GetMouseButtonDown(0) && selectedSeed != null)
                {
                    PlaceCropAtPosition(mouseHoverCellPosition);
                }
            }
            else
            {
                normalCellIndicator.SetActive(false);
                redCellIndicator.SetActive(true);
            }
        }
        else
        {
            normalCellIndicator.SetActive(false);
            redCellIndicator.SetActive(false);
        }

    }

    public bool CanPlantCropsAtPosition(Vector3Int gridPosition)
    {
        StatefulTile tile = tilemapBuildable.GetTile<StatefulTile>(gridPosition);

        if (tile != null && tile.isArable && !occupiedTiles.Contains(gridPosition))
        {
            return true;
        }

        return false;
    }

    private void PlaceCropAtPosition(Vector3 position)
    {
        Vector3Int gridPosition = grid.WorldToCell(position);
        GameObject newCrop = Instantiate(selectedSeed.CropPrefab(), position, Quaternion.identity);
        newCrop.transform.SetParent(tilemapCrops.transform);
        HUDManager.Instance.GetPlayerInventory().RemoveAmountOfItems(selectedSeed.GetItemData().ItemID, 1);
        occupiedTiles.Add(gridPosition);
    }
    #endregion


    #region Farming Mode

    public void EnterFarmingMode(Tool tool)
    {
        ChangePlacementMode(PlacementMode.Farming);
        selectedTool = tool;
    }

    public void Farm()
    {
        if (isAdjacentOrDiagonal)
        {
            if (CanFarmAtPosition(gridPosition))
            {
                normalCellIndicator.SetActive(true);
                redCellIndicator.SetActive(false);
                if (Input.GetMouseButtonDown(0) && selectedTool != null)
                {
                    FarmAtPosition(mouseHoverCellPosition);
                }
            }
            else
            {
                normalCellIndicator.SetActive(false);
                redCellIndicator.SetActive(true);
            }
        }
        else
        {
            normalCellIndicator.SetActive(false);
            redCellIndicator.SetActive(false);
        }

    }

    public void AddFarmableTile(Vector3 position, GameObject farmableObject)
    {
        Vector3Int farmablePosition = grid.WorldToCell(position);
        farmableGameObjectsAtPositions.Add(farmablePosition, farmableObject);
    }

    public bool CanFarmAtPosition(Vector3Int gridPosition)
    {
        StatefulTile tile = tilemapBuildable.GetTile<StatefulTile>(gridPosition);
        //Check what kind of crop/tree tile
        if (tile != null && farmableGameObjectsAtPositions.ContainsKey(gridPosition))
        {
            return true;
        }

        return false;
    }

    private void FarmAtPosition(Vector3 position)
    {
        Vector3Int gridPosition = grid.WorldToCell(position);
        //Farm
        if (farmableGameObjectsAtPositions.TryGetValue(gridPosition, out GameObject farmableObject))
        {
            switch (selectedTool.ResourceType)
            {
                case ResourceType.Crop:
                    FarmCrop(farmableObject);
                    break;
                case ResourceType.Tree:
                    break;
                case ResourceType.Stone:
                    break;
                default:
                    break;
            }
        }

    }

    private void FarmCrop(GameObject cropGameObject)
    {
        Crop cropTemp = cropGameObject.GetComponent<Crop>();

        if (cropTemp.ResourceType == selectedTool.ResourceType)
        {
            if (!cropTemp.TryHarvesting()) return;
            farmableGameObjectsAtPositions.Remove(gridPosition);
            if (occupiedTiles.Contains(gridPosition))
                occupiedTiles.Remove(gridPosition);
        }
    }
    #endregion


    public bool CanBuildAtPosition(Vector3Int gridPosition)
    {
        StatefulTile tile = tilemapBuildable.GetTile<StatefulTile>(gridPosition);

        if (tile != null && tile.canBuildOn && !occupiedTiles.Contains(gridPosition))
        {
            return true;
        }

        return false;
    }


    public string GetTileInfo(Vector3Int gridPosition)
    {
        StatefulTile tile = tilemapBuildable.GetTile<StatefulTile>(gridPosition);
        if (tile != null)
        {
            return $"StatefulTile at {gridPosition}, BuildOn: {tile.canBuildOn} , Arable: {tile.isArable}";
        }
        else
        {
            return $"No StatefulTile found at {gridPosition}.";
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(playerCellPosition, Vector3.one);
        Gizmos.DrawWireSphere(playerPosition + playerPositionOffset, playerPostionSphere);
        Gizmos.color = Color.cyan;
        if (CachedPlayerController != null)
            Gizmos.DrawWireCube(playerCellPosition, new Vector3(3, 3, 3) * CachedPlayerController.GetPlayerStats().BuildingRadius);
    }



}
