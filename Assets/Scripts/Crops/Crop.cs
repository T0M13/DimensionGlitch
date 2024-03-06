using System.Collections;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] private CropGrowthState[] states;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField][ShowOnly] private CropState currentState = CropState.Seed;

    [SerializeField][ShowOnly] private int currentStateIndex = 0;
    [SerializeField][ShowOnly] private float stateTimer = 0;

    public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }

    private void OnValidate()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
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

        UpdateSprite();
        StartCoroutine(UpdateStateCoroutine());
    }

    private void UpdateSprite()
    {
        SpriteRenderer.sprite = states[currentStateIndex].sprite;
        currentState = states[currentStateIndex].growthStage;
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


    private void OnFullyGrown()
    {
        // Implement what happens when the crop is fully grown
        Debug.Log("Crop is fully grown.");
    }
}
