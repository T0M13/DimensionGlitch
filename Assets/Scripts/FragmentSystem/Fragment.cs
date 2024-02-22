using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    [Header("Fragment Settings")]
    [SerializeField] private float interactionRange = .3f;
    [SerializeField] private FragmentType fragmentType = FragmentType.NotCollected;
    [Header("Collider Settings")]
    [SerializeField] private CircleCollider2D fragmentCollider;
    [Header("Gizmos Settings")]
    [SerializeField] private bool showGizmos;


    private void OnValidate()
    {
        SetCollider();

    }

    private void Awake()
    {
        SetCollider();
        fragmentType = FragmentType.NotCollected;
    }

    private void SetCollider()
    {
        fragmentCollider = GetComponent<CircleCollider2D>();

        fragmentCollider.radius = interactionRange * 2;
    }

    private void Collect(Collider2D collision)
    {
        fragmentType = FragmentType.Collected;
        //Remove from list 
        //Maybe add to list where its been collected
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if its the player
        Collect(collision);

    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
