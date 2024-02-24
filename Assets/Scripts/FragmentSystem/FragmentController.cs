using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class FragmentController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
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
    [SerializeField] private float dynamicChanceToSelectDimensionWithFragment;
    [SerializeField] private int consecutiveSelectionsWithoutFragments = 0;
    [SerializeField] private bool randDimensionEffect = true;
    [SerializeField] private List<UnityEvent> onDimensionChangeRandEffect;

    [Header("Actions")]
    public UnityEvent onBeforePlayerShifting;
    public UnityEvent onAfterPlayerShifting;
    public UnityEvent onResetDimensionEffects;
    public UnityEvent onFragmentShifting;
    public UnityEvent onGameOverVictory;

    public int GetAmountOfFragments() => fragmentAmount;
    public float FragmentShiftTimer { get => fragmentShiftTimer; set => fragmentShiftTimer = value; }
    public float FragmentShiftTimerCooldown { get => fragmentShiftTimerCooldown; set => fragmentShiftTimerCooldown = value; }

    private void OnValidate()
    {
        GetGameManager();
    }

    private void Awake()
    {
        GetGameManager();

        notCollectedFragments = new List<GameObject>();
        collectedFragments = new List<GameObject>();

        for (int i = 0; i < fragmentAmount; i++)
        {
            GameObject fragment = Instantiate(fragmentPrefab);
            fragment.transform.parent = transform;
            fragment.SetActive(false);
            fragment.GetComponent<Fragment>().FragmentControllerParent = this;
            notCollectedFragments.Add(fragment);
        }
    }


    private void Start()
    {
        //Make sure only one / first dimension is the only one active
        //foreach (var gridDimension in gridDimensions)
        //{
        //    if (gridDimension.gameObject.activeSelf) gridDimension.onDimensionChange?.Invoke();
        //}
    }


    private void Update()
    {
        if (notCollectedFragments.Count <= 0)
        {
            //Debug.Log("No more Fragments to be collected");
            return;
        }

        if (gridDimensions.Count <= 0)
        {
            Debug.Log("No GridDimensions");
            return;
        }

        FragmentShiftTimer += Time.deltaTime;
        if (FragmentShiftTimer >= FragmentShiftTimerCooldown)
        {
            RepositionFragments();
            FragmentShiftTimer = 0;
        }
    }

    private void GetGameManager()
    {
        gameManager = GameManager.Instance;
    }

    private void RepositionFragments()
    {

        //Clear FragmentPoints --> So they can be taken
        foreach (var gridDimension in gridDimensions)
        {
            gridDimension.ResetFragmenPoints();
            gridDimension.ResetFragmentsInDimension();
        }

        //Effect when shifting
        onFragmentShifting?.Invoke();

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

        //Vector3 tempEffectPos = fragment.transform.position;

        fragment.transform.position = randFragmentPoint.transform.position;
        fragment.gameObject.SetActive(randGridDimension.gameObject.activeSelf);
        Fragment fragmentScript = fragment.GetComponent<Fragment>();
        //fragmentScript.FragmentShiftEffect.transform.position = tempEffectPos;
        //fragmentScript.FragmentShiftEffect.SetActive( randGridDimension.gameObject.activeSelf);
        fragmentScript.FragmentShiftEffect.GetComponent<Animator>().StartPlayback();
        randGridDimension.CurrentFragmentsInDimension.Add(fragmentScript);
        fragmentScript.CurrentDimension = randGridDimension;
        randFragmentPoint.gameObject.SetActive(false);



    }

    public void SetFragmentCollected(Fragment fragment)
    {
        notCollectedFragments.Remove(fragment.gameObject);
        collectedFragments.Add(fragment.gameObject);
        //if all collected --> game over victory
        if(notCollectedFragments.Count <= 0 || collectedFragments.Count >= fragmentAmount)
        {
            //Game Over Victory
            onGameOverVictory?.Invoke();
        }
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

        if(gridDimensions.Count <= 1)
        {
            Debug.Log("Only One Dimension. Can't Travel to other");
            return;
        }

        // Adjust chance based on previous outcomes
        dynamicChanceToSelectDimensionWithFragment = Mathf.Clamp(chanceToSelectDimensionWithFragment + (0.1f * consecutiveSelectionsWithoutFragments), 0f, 1f);

        // Separate the dimensions into those with and without fragments
        List<GridDimension> dimensionsWithFragments = gridDimensions
            .Where(gd => gd.CurrentFragmentsInDimension.Count > 0 && gd != excludeDimension).ToList();
        List<GridDimension> dimensionsWithoutFragments = gridDimensions
            .Where(gd => gd.CurrentFragmentsInDimension.Count == 0 && gd != excludeDimension).ToList();

        List<GridDimension> chosenList;
        if (Random.Range(0f, 1f) <= dynamicChanceToSelectDimensionWithFragment)
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

        //before travelling - change volumes
        //Camera.main.GetUniversalAdditionalCameraData().volumeTrigger = GameManager.Instance.GetPlayerControllerRef.transform;

        //before travelling - make only the traveldimension visible
        foreach (var gridDimension in gridDimensions)
        {
            gridDimension.gameObject.SetActive(false);
        }

        gridTravelDimension.gameObject.SetActive(true);

        //before travelling - actions
        onBeforePlayerShifting?.Invoke();

        // Reset Portals before traveling
        excludeDimension.ResetPortals();
        DimensionPortal portal = GetRandomPortal(gridTravelDimension);
        if (collision != null)
        {
            collision.transform.position = portal.transform.position;
            portal.SetHasBeenTraveledThrough();
        }

 

        //set fragments
        foreach (var fragment in notCollectedFragments)
        {
            fragment.SetActive(fragment.GetComponent<Fragment>().CurrentDimension.gameObject.activeSelf);
        }

        //after travelling - actions
        onAfterPlayerShifting?.Invoke();

        //reset other dimension events
        onResetDimensionEffects?.Invoke();

        //trigger dimension event
        if (randDimensionEffect)
        {
            RandDimensionEffect()?.Invoke();
        }
        else
        {
            gridTravelDimension.onDimensionChange?.Invoke();
        }

    }

    private UnityEvent RandDimensionEffect()
    {
        return onDimensionChangeRandEffect[Random.Range(0, onDimensionChangeRandEffect.Count)];
    }

    private DimensionPortal GetRandomPortal(GridDimension gridDimension)
    {
        return gridDimension.Portals[Random.Range(0, gridDimension.Portals.Count)];
    }


}
