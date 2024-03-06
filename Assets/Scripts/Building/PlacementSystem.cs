using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tilemapBuildable;
    [SerializeField] private Tilemap tilemapCrops;

    [SerializeField] private GameObject selectedCropPrefab;
    private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();
    [SerializeField][ShowOnly] private PlayerController CachedPlayerController = null;

    [Header("Debug")]
    [SerializeField][ShowOnly] private Vector3 mouseHoverCellPosition;
    [SerializeField] private float playerPostionSphere = 0.2f;
    [SerializeField][ShowOnly] private Vector3 playerPosition;
    [SerializeField] private Vector3 playerPositionOffset = new Vector3(0, -0.33f, 0);
    [SerializeField][ShowOnly] private Vector3 playerCellPosition;


    private void Start()
    {
        CachedPlayerController = GameManager.Instance.GetPlayerControllerRef;
    }

    private void Update()
    {
        Vector3 mousePosition = InputManager.Instance.GetMousePositionInWorld();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseHoverCellPosition = grid.GetCellCenterWorld(gridPosition);

        playerPosition = CachedPlayerController.transform.position;
        Vector3Int playerGridPosition = grid.WorldToCell(playerPosition + playerPositionOffset);
        playerCellPosition = grid.GetCellCenterWorld(playerGridPosition);

        cellIndicator.transform.position = mouseHoverCellPosition;

        Vector3Int gridPositionDifference = gridPosition - playerGridPosition;
        bool isAdjacentOrDiagonal = Mathf.Abs(gridPositionDifference.x) <= CachedPlayerController.GetPlayerStats().BuildingRadius && Mathf.Abs(gridPositionDifference.y) <= CachedPlayerController.GetPlayerStats().BuildingRadius;


        if (CanBuildAtPosition(gridPosition) && isAdjacentOrDiagonal)
        {
            cellIndicator.SetActive(true);

            if (Input.GetMouseButtonDown(0) && selectedCropPrefab != null)
            {
                PlaceCropAtPosition(mouseHoverCellPosition);
            }
        }
        else
        {
            cellIndicator.SetActive(false);
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


    private void PlaceCropAtPosition(Vector3 position)
    {
        Vector3Int gridPosition = grid.WorldToCell(position);

        GameObject newCrop = Instantiate(selectedCropPrefab, position, Quaternion.identity);
        newCrop.transform.SetParent(tilemapCrops.transform);

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


}
