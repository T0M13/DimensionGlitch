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

    public FragmentController FragmentControllerParent { get => fragmentControllerParent; set => fragmentControllerParent = value; }
    public GridDimension CurrentDimension { get => currentDimension; set => currentDimension = value; }

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

    private void Collect(Collider2D collision)
    {
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
