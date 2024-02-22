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
    [SerializeField] private List<FragmentPoint> notTakenFragmentPoints;
    [SerializeField] private List<FragmentPoint> takenFragmentPoints;

    public List<FragmentPoint> NotTakenFragmentPoints { get => notTakenFragmentPoints; set => notTakenFragmentPoints = value; }
    public List<FragmentPoint> TakenFragmentPoints { get => takenFragmentPoints; set => takenFragmentPoints = value; }

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
        NotTakenFragmentPoints = new List<FragmentPoint>();
        TakenFragmentPoints = new List<FragmentPoint>();

        for (int i = 0; i < fragmentPointsParent.childCount; i++)
        {
            NotTakenFragmentPoints.Add(fragmentPointsParent.GetChild(i).GetComponent<FragmentPoint>());
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
