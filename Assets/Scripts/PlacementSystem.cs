using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private Grid grid;

    private void Update()
    {
        Vector3 mousePosition = InputManager.Instance.GetMousePositionInWorld();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        Vector3 cellCenterPosition = grid.GetCellCenterWorld(gridPosition);
        cellIndicator.transform.position = cellCenterPosition;

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, InputManager.Instance.BuildingLayermask);

        if (hit.collider != null)
        {
            cellIndicator.SetActive(true);
        }
        else
        {
            cellIndicator.SetActive(false);
        }

    }
}
