using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject normalCellIndicator;
    [SerializeField] private GameObject redCellIndicator;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tilemapBuildable;
    [SerializeField] private Tilemap tilemapCrops;

    private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();
    [SerializeField][ShowOnly] private PlayerController CachedPlayerController = null;

    [Header("Crops")]
    [SerializeField] private PlaceableSeed selectedSeed;
    [Header("Debug")]
    [SerializeField] private PlacementMode currentPlacementMode = PlacementMode.None;
    [SerializeField][ShowOnly] private Vector3 mouseHoverCellPosition;
    [SerializeField][ShowOnly] private Vector3Int gridPosition;
    [SerializeField] private float playerPostionSphere = 0.2f;
    [SerializeField][ShowOnly] private Vector3 playerPosition;
    [SerializeField] private Vector3 playerPositionOffset = new Vector3(0, -0.33f, 0);
    [SerializeField][ShowOnly] private Vector3 playerCellPosition;
    [SerializeField][ShowOnly] private bool isAdjacentOrDiagonal;
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

        switch (CurrentPlacementMode)
        {
            case PlacementMode.None:
                //Nichts
                break;
            case PlacementMode.Planting:
                PlaceSeed();
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

    public void EnterPlantingMode(PlaceableSeed seed)
    {
        ChangePlacementMode(PlacementMode.Planting);
        selectedSeed = seed;
    }

    public void ExitCurrentPlacementMode()
    {
        ChangePlacementMode(PlacementMode.None);
        selectedSeed = null;
        normalCellIndicator.SetActive(false);
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

    public bool CanBuildAtPosition(Vector3Int gridPosition)
    {
        StatefulTile tile = tilemapBuildable.GetTile<StatefulTile>(gridPosition);

        if (tile != null && tile.canBuildOn && !occupiedTiles.Contains(gridPosition))
        {
            return true;
        }

        return false;
    }

    public bool CanPlantCropsAtPosition(Vector3Int gridPosition)
    {
        StatefulTile tile = tilemapBuildable.GetTile<StatefulTile>(gridPosition);

        if (tile != null && tile.IsArable && !occupiedTiles.Contains(gridPosition))
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(playerCellPosition, Vector3.one);
        Gizmos.DrawWireSphere(playerPosition + playerPositionOffset, playerPostionSphere);
        Gizmos.color = Color.cyan;
        if (CachedPlayerController != null)
            Gizmos.DrawWireCube(playerCellPosition, new Vector3(3, 3, 3) * CachedPlayerController.GetPlayerStats().BuildingRadius);
    }

    public enum PlacementMode
    {
        None = 0,
        Planting = 100,
        Building = 200,
    }

}
