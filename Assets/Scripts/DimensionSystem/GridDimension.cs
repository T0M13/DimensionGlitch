using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class GridDimension : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Transform floorTileMap;
    [SerializeField] private Transform wallTileMap;
    [SerializeField] private Transform wallDecosTileMap;
    [Header("Fragment of Grid Settings")]
    [SerializeField] private Transform fragmentPointsParent;
    [SerializeField] private List<FragmentPoint> fragmentPoints;
    [SerializeField] private List<Fragment> currentFragmentsInDimension;
    [Header("Dimension Portal Settings")]
    [SerializeField] private Transform portalsParent;
    [SerializeField] private List<DimensionPortal> portals;
    [Header("Volume Settings")]
    [SerializeField] private Volume dimensionVolume;

    public UnityEvent onDimensionChange;

    public List<FragmentPoint> FragmentPoints { get => fragmentPoints; set => fragmentPoints = value; }
    public List<Fragment> CurrentFragmentsInDimension { get => currentFragmentsInDimension; set => currentFragmentsInDimension = value; }
    public List<DimensionPortal> Portals { get => portals; set => portals = value; }

    private void OnValidate()
    {
        GetFragmentPoints();
        GetPortals();
    }

    private void Awake()
    {
        GetFragmentPoints();
        GetPortals();
        currentFragmentsInDimension = new List<Fragment>();
    }

    private void GetFragmentPoints()
    {
        FragmentPoints = new List<FragmentPoint>();

        for (int i = 0; i < fragmentPointsParent.childCount; i++)
        {
            FragmentPoints.Add(fragmentPointsParent.GetChild(i).GetComponent<FragmentPoint>());
        }
    }

    private void GetPortals()
    {
        Portals = new List<DimensionPortal>();
        for (int i = 0; i < portalsParent.childCount; i++)
        {
            Portals.Add(portalsParent.GetChild(i).GetComponent<DimensionPortal>());
        }
    }

    public void ResetFragmenPoints()
    {
        foreach (var fragmentpoint in fragmentPoints)
        {
            fragmentpoint.gameObject.SetActive(true);
        }
    }

    public void ResetFragmentsInDimension()
    {
        currentFragmentsInDimension.Clear();
    }

    public void ResetPortals()
    {
        foreach (var portal in Portals)
        {
            portal.ResetTraveledThrough();
        }
    }


}
