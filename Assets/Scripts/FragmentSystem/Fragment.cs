using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    [Header("Fragment Settings")]
    [SerializeField] private float interactionRange = .5f;
    [SerializeField] private FragmentType fragmentType = FragmentType.NotCollected;
    [Header("Gizmos Settings")]
    [SerializeField] private bool showGizmos;


    private void Awake()
    {
        fragmentType = FragmentType.NotCollected;
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
