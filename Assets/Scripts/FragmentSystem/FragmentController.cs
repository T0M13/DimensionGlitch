using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FragmentController fragmentController;
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

    private void Start()
    {


    }

    private void Update()
    {
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
            gridDimension.NotTakenFragmentPoints = new List<FragmentPoint>();

            foreach (var fragmentpoint in gridDimension.TakenFragmentPoints)
            {
              
            }
        }

        //Reposition Every Fragment
        foreach (var fragment in notCollectedFragments)
        {
            RepositionFragment(fragment);
        }
    }

    private void RepositionFragment(GameObject fragment)
    {
        GridDimension randGridDimension = GetRandomDimension();
        FragmentPoint randFragmentPoint = GetRandomFragmentPos(randGridDimension);

        if(randFragmentPoint.FragmentPointType == FragmentPointType.Taken)

        fragment.transform.position = randFragmentPoint;

    }

    private GridDimension GetRandomDimension()
    {
        return gridDimensions[Random.Range(0, gridDimensions.Count)];
    }

    private FragmentPoint GetRandomFragmentPos(GridDimension gridDimension)
    {
        return gridDimension.FragmentPoints[Random.Range(0, gridDimension.FragmentPoints.Count)];
    }





}
