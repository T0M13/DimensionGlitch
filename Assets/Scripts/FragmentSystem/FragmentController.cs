using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FragmentController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GridDimension> gridDimensions;

    [Header("Fragments Settings")]
    [SerializeField] private GameObject fragmentPrefab;
    [Range(3, 5)][SerializeField] private int fragmentAmount = 3;
    [SerializeField] private List<GameObject> notCollectedFragments;
    [SerializeField] private List<GameObject> collectedFragments;

    [Header("Fragments Timer")]
    [SerializeField] private float fragmentShiftTimerCooldown = 4f;
    [SerializeField] private float fragmentShiftTimer;


    private void Awake()
    {
        notCollectedFragments = new List<GameObject>();
        collectedFragments = new List<GameObject>();

        for (int i = 0; i < fragmentAmount; i++)
        {
            GameObject fragment = Instantiate(fragmentPrefab);
            fragment.transform.parent = transform;
            fragment.SetActive(false);
            notCollectedFragments.Add(fragment);
        }
    }

    private void Update()
    {
        if (notCollectedFragments.Count <= 0)
        {
            Debug.Log("No more Fragments to be collected");
            return;
        }

        if (gridDimensions.Count <= 0)
        {
            Debug.Log("No GridDimensions");
            return;
        }

        fragmentShiftTimer += Time.deltaTime;
        if (fragmentShiftTimer >= fragmentShiftTimerCooldown)
        {
            RepositionFragments();
            fragmentShiftTimer = 0;
        }
    }

    private void RepositionFragments()
    {
        //Clear FragmentPoints --> So they can be taken
        foreach (var gridDimension in gridDimensions)
        {
            gridDimension.ResetFragmenPoints();
        }

        //Reposition Every Fragment
        foreach (var fragment in notCollectedFragments)
        {
            RepositionFragment(fragment);
        }
    }

    private void RepositionFragment(GameObject fragment)
    {
        FragmentPoint randFragmentPoint = GetRandomFragmentPos(GetRandomDimension());
        fragment.transform.position = randFragmentPoint.transform.position;
        fragment.gameObject.SetActive(true);
        randFragmentPoint.gameObject.SetActive(false);
    }

    private GridDimension GetRandomDimension()
    {
        return gridDimensions[Random.Range(0, gridDimensions.Count)];
    }

    private FragmentPoint GetRandomFragmentPos(GridDimension gridDimension)
    {
        // Filter out all inactive FragmentPoints
        var activeFragmentPoints = gridDimension.FragmentPoints.Where(fp => fp.gameObject.activeSelf).ToList();

        // Return a random active FragmentPoint
        return activeFragmentPoints[Random.Range(0, activeFragmentPoints.Count)];
    }





}
