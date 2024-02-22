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

    [Header("Dimension Settings")]
    [SerializeField] private float chanceToSelectDimensionWithFragment = 0.5f;
    [SerializeField] private int consecutiveSelectionsWithoutFragments = 0;



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
            gridDimension.ResetFragmentsInDimension();
        }

        //Reposition Every Fragment
        foreach (var fragment in notCollectedFragments)
        {
            RepositionFragment(fragment);
        }
    }

    private void RepositionFragment(GameObject fragment)
    {
        FragmentPoint randFragmentPoint = null;
        GridDimension randGridDimension = null;
        bool foundPosition = false;


        var tempGridDimension = gridDimensions;

        // Attempt to find a non-occupied position for the fragment
        while (!foundPosition)
        {
            randGridDimension = GetRandomDimension(tempGridDimension);
            var activeFragmentPoints = GetRandomActiveFragmentPoints(randGridDimension);

            if (activeFragmentPoints.Count > 0)
            {
                randFragmentPoint = activeFragmentPoints[Random.Range(0, activeFragmentPoints.Count)];
                foundPosition = true; // Break the loop if a position is found
            }
            else
            {
                //Remove the gridDimension with no active points to avoid infinite loop
                tempGridDimension.Remove(randGridDimension);
                if (gridDimensions.Count <= 0)
                {
                    Debug.LogError("Ran out of grid dimensions to place fragments.");
                    return; // Or handle this case as needed
                }
            }
        }

        fragment.transform.position = randFragmentPoint.transform.position;
        fragment.gameObject.SetActive(true);
        Fragment fragmentScript = fragment.GetComponent<Fragment>();
        randGridDimension.CurrentFragmentsInDimension.Add(fragmentScript);
        randFragmentPoint.gameObject.SetActive(false);



    }

    private GridDimension GetRandomDimension(List<GridDimension> gridDimensions)
    {
        return gridDimensions[Random.Range(0, gridDimensions.Count)];
    }

    private List<FragmentPoint> GetRandomActiveFragmentPoints(GridDimension gridDimension)
    {
        // Filter out all inactive FragmentPoints
        return gridDimension.FragmentPoints.Where(fp => fp.gameObject.activeSelf).ToList();
    }


    public void TravelToDifferentDimension(Collider2D collision, GridDimension excludeDimension)
    {
        // Adjust chance based on previous outcomes
        float dynamicChance = Mathf.Clamp(chanceToSelectDimensionWithFragment + (0.1f * consecutiveSelectionsWithoutFragments), 0f, 1f);

        // Separate the dimensions into those with and without fragments
        List<GridDimension> dimensionsWithFragments = gridDimensions
            .Where(gd => gd.CurrentFragmentsInDimension.Count > 0 && gd != excludeDimension).ToList();
        List<GridDimension> dimensionsWithoutFragments = gridDimensions
            .Where(gd => gd.CurrentFragmentsInDimension.Count == 0 && gd != excludeDimension).ToList();

        List<GridDimension> chosenList;
        if (Random.Range(0f, 1f) <= dynamicChance)
        {
            chosenList = dimensionsWithFragments.Any() ? dimensionsWithFragments : dimensionsWithoutFragments;
        }
        else
        {
            chosenList = dimensionsWithoutFragments.Any() ? dimensionsWithoutFragments : dimensionsWithFragments;
        }

        GridDimension gridTravelDimension = chosenList[Random.Range(0, chosenList.Count)];

        // Check the selection and adjust the consecutiveSelectionsWithoutFragments counter
        if (gridTravelDimension.CurrentFragmentsInDimension.Count == 0)
        {
            consecutiveSelectionsWithoutFragments++;
        }
        else
        {
            consecutiveSelectionsWithoutFragments = 0; // Reset counter if a dimension with fragments is selected
        }

        // Reset Portals before traveling
        excludeDimension.ResetPortals();
        DimensionPortal portal = GetRandomPortal(gridTravelDimension);
        if (collision != null)
        {
            collision.transform.position = portal.transform.position;
            portal.SetHasBeenTraveledThrough();
        }
    }

    private DimensionPortal GetRandomPortal(GridDimension gridDimension)
    {
        return gridDimension.Portals[Random.Range(0, gridDimension.Portals.Count)];
    }


}
