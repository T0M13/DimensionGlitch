using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDimension : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Transform floorTileMap;
    [SerializeField] private Transform wallTileMap;
    [SerializeField] private Transform wallDecosTileMap;
    [Header("Fragment of Grid Settings")]
    [SerializeField] private Transform fragmentPointsParent;
    [SerializeField] private List<FragmentPoint> fragmentPoints;

    public List<FragmentPoint> FragmentPoints { get => fragmentPoints; set => fragmentPoints = value; }

    private void OnValidate()
    {
        GetFragmentPoints();
    }

    private void Awake()
    {
        GetFragmentPoints();
    }

    private void GetFragmentPoints()
    {
        FragmentPoints = new List<FragmentPoint>();

        for (int i = 0; i < fragmentPointsParent.childCount; i++)
        {
            FragmentPoints.Add(fragmentPointsParent.GetChild(i).GetComponent<FragmentPoint>());
        }
    }

    public void ResetFragmenPoints()
    {
        foreach (var fragmentpoint in fragmentPoints)
        {
            fragmentpoint.gameObject.SetActive(true);
        }
    }


    private void ActivateGrid()
    {
        floorTileMap.gameObject.SetActive(false);
        wallTileMap.gameObject.SetActive(false);
        wallDecosTileMap.gameObject.SetActive(false);
    }

    private void DeactivateGrid()
    {
        floorTileMap.gameObject.SetActive(false);
        wallTileMap.gameObject.SetActive(false);
        wallDecosTileMap.gameObject.SetActive(false);
    }

}
