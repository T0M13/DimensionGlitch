using System;
using System.Collections;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField][ShowOnly] private CapsuleCollider2D capsuleCollider;
    [Header("Crop Settings")]
    [SerializeField] private int dropAmount;
    [SerializeField] private Item dropItem;
    [SerializeField] private PickupItem initializeItem;
    [SerializeField] private Stats cropStats;
    [SerializeField][ShowOnly] private ResourceType resourceType = ResourceType.Crop;
    [Header("Crop States")]
    [SerializeField][ShowOnly] private CropState currentState = CropState.Seed;
    [SerializeField] private CropGrowthState[] states;
    [SerializeField][ShowOnly] private int currentStateIndex = 0;
    [SerializeField][ShowOnly] private float stateTimer = 0;

    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
    public ResourceType ResourceType { get => resourceType; set => resourceType = value; }
    public CropState CurrentState { get => currentState; set => currentState = value; }

    private void OnValidate()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        if (states.Length == 0)
        {
            Debug.LogError("No states defined for the crop.");
            return;
        }

        if (!SpriteRenderer)
        {
            Debug.LogError("SpriteRenderer not assigned.");
            return;
        }

        cropStats.OnDeath += Harvest;

        UpdateSprite();
        StartCoroutine(UpdateStateCoroutine());
    }

    private void UpdateSprite()
    {
        SpriteRenderer.sprite = states[currentStateIndex].sprite;
        CurrentState = states[currentStateIndex].growthStage;

        capsuleCollider.enabled = states[currentStateIndex].colliderActive;
        capsuleCollider.size = states[currentStateIndex].colliderSize;
        capsuleCollider.offset = states[currentStateIndex].colliderOffset;
    }

    private IEnumerator UpdateStateCoroutine()
    {
        while (currentStateIndex < states.Length - 1)
        {
            stateTimer += Time.deltaTime;

            if (stateTimer >= states[currentStateIndex].duration)
            {
                currentStateIndex++;
                stateTimer = 0;

                UpdateSprite();

                if (currentStateIndex == states.Length - 1)
                {
                    OnFullyGrown();
                    yield break;
                }
            }

            yield return null;
        }
    }


    public bool TryHarvesting()
    {
        return cropStats.RecieveDmg();
    }

    private void Harvest()
    {
        //Cooler if more, with distance, and animation for++
        if (currentState == CropState.Mature)
        {
            var instance = Instantiate(initializeItem, transform.position, Quaternion.identity, null);
            instance.InitializeItemPickup(dropItem, dropAmount);
        }
        Destroy(gameObject);
    }

    private void OnFullyGrown()
    {
        UpdateSprite();
        PlacementSystem.Instance.AddFarmableTile(transform.position, this.gameObject);
        Debug.Log("Crop is fully grown.");
    }
}
