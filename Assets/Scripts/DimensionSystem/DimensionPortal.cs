using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionPortal : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] private float interactionRange = .6f;
    [Header("Collider Settings")]
    [SerializeField] private CircleCollider2D portalCollider;
    [Header("Gizmos Settings")]
    [SerializeField] private bool showGizmos;

    private void OnValidate()
    {
        SetCollider();

    }

    private void Awake()
    {
        SetCollider();
    }

    private void SetCollider()
    {
        portalCollider = GetComponent<CircleCollider2D>();

        portalCollider.radius = interactionRange;
    }

    private void TravelDimension(Collider2D collision)
    {

        //Travel Dimension
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if its the player
        TravelDimension(collision);

    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
