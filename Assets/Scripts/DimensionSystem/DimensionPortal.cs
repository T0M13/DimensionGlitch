using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionPortal : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FragmentController fragmentController;
    [SerializeField] private GridDimension gridDimensionParent;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color[] colors;

    [Header("Portal Settings")]
    [SerializeField] private bool hasBeenTraveledThrough;

    [SerializeField] private Vector2 interactionRange = new Vector2(0.8f, 1.5f);
    [Header("Collider Settings")]
    [SerializeField] private CapsuleCollider2D portalCollider;

    private void OnValidate()
    {
        GetSprite();
        SetCollider();
        GetGridDimensionParent();
        GetFragmentController();
        SetRandColor();
        //RandRotation();
    }

    private void Awake()
    {
        GetSprite();
        SetCollider();
        GetGridDimensionParent();
        GetFragmentController();
        SetRandColor();
        //RandRotation();
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
        hasBeenTraveledThrough = true;
    }

    public void ResetTraveledThrough()
    {
        SetRandColor();
        hasBeenTraveledThrough = false;
    }

    private void SetRandColor()
    {
        sprite.color = colors[Random.Range(0, colors.Length)];
    }

    private void SetCollider()
    {
        portalCollider = GetComponent<CapsuleCollider2D>();

        portalCollider.size = interactionRange;
    }

    private void RandRotation()
    {
        Quaternion temp = transform.rotation;
        temp.z = Random.Range(0, 360);
        transform.rotation = temp;
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

}
