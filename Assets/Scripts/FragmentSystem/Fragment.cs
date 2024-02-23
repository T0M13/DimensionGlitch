using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    [Header("Fragment Settings")]
    [SerializeField] private FragmentController fragmentControllerParent;
    [SerializeField] private GridDimension currentDimension;
    [SerializeField] private FragmentType fragmentType = FragmentType.NotCollected;
    [Header("Collider Settings")]
    [SerializeField] private CapsuleCollider2D fragmentCollider;
    [Header("Effect Settings")]
    [SerializeField] private GameObject fragmentShiftEffect;


    public FragmentController FragmentControllerParent { get => fragmentControllerParent; set => fragmentControllerParent = value; }
    public GridDimension CurrentDimension { get => currentDimension; set => currentDimension = value; }
    public GameObject FragmentShiftEffect { get => fragmentShiftEffect; set => fragmentShiftEffect = value; }

    private void OnValidate()
    {
        GetFragmentController();
        SetCollider();
    }

    private void Awake()
    {
        GetFragmentController();
        SetCollider();
        fragmentType = FragmentType.NotCollected;
    }

    private void GetFragmentController()
    {
        if (fragmentControllerParent == null)
            fragmentControllerParent = FindAnyObjectByType<FragmentController>();
    }

    private void SetCollider()
    {
        fragmentCollider = GetComponent<CapsuleCollider2D>();
    }

    public void Shift()
    {
        if (EffectSpawner.Instance != null)
            EffectSpawner.Instance.SpawnFragmentShiftEffect(transform);
    }

    private void Collect(Collider2D collision)
    {
        //effect
        if (EffectSpawner.Instance != null)
            EffectSpawner.Instance.SpawnItemPickUpEffect(transform);

        fragmentType = FragmentType.Collected;
        //Remove from list 
        fragmentControllerParent.SetFragmentCollected(this);
        //Maybe add to list where its been collected
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() && fragmentType != FragmentType.Collected)
            Collect(collision);

    }

}
