using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionPortal : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FragmentController fragmentController;
    [SerializeField] private GridDimension gridDimensionParent;
    [SerializeField] private SpriteRenderer sprite;

    [Header("Portal Settings")]
    [SerializeField] private bool hasBeenTraveledThrough;

    [SerializeField] private float interactionRange = .6f;
    [Header("Collider Settings")]
    [SerializeField] private CircleCollider2D portalCollider;
    [Header("Gizmos Settings")]
    [SerializeField] private bool showGizmos;

    private void OnValidate()
    {
        GetSprite();
        SetCollider();
        GetGridDimensionParent();
        GetFragmentController();
    }

    private void Awake()
    {
        GetSprite();
        SetCollider();
        GetGridDimensionParent();
        GetFragmentController();
        hasBeenTraveledThrough = false;
    }

    private void GetGridDimensionParent()
    {
        gridDimensionParent = transform.root.gameObject.GetComponent<GridDimension>();
    }

    private void GetFragmentController()
    {
        fragmentController = FindAnyObjectByType<FragmentController>();
    }
    private void GetSprite()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetHasBeenTraveledThrough()
    {
        sprite.color = new Color(1f, 1f, 1f, .5f);
        hasBeenTraveledThrough = true;
    }

    public void ResetTraveledThrough()
    {
        sprite.color = new Color(1f, 1f, 1f, 1f);
        hasBeenTraveledThrough = false;
    }

    private void SetCollider()
    {
        portalCollider = GetComponent<CircleCollider2D>();

        portalCollider.radius = interactionRange;
    }

    private void TravelDimension(Collider2D collision)
    {
        fragmentController.TravelToDifferentDimension(collision, gridDimensionParent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBeenTraveledThrough) return;

        //Check if its the player
        if (collision.gameObject.GetComponent<PlayerController>())
            TravelDimension(collision);

    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
