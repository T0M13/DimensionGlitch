using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Volume volume;
    [Header("References")]
    [SerializeField] private Bloom bloom;
    [SerializeField] private ChromaticAberration chromaticAberration;
    [SerializeField] private LensDistortion lensDistortion;

    [Header("Bloom Settings")]
    [SerializeField] private bool doBloom = true;
    [SerializeField] private float baseBloomIntensity = 1f;
    [SerializeField] private float maxBloomIntensity = 3f;
    [SerializeField] private float bloomTransitionSpeed = .1f;

    [Header("ChromaticAberration Settings")]
    [SerializeField] private bool doChromaticAberration = true;
    [SerializeField] private float baseChromaticIntensity = 0.2f; 
    [SerializeField] private float maxChromaticIntensity = 0.5f; 
    [SerializeField] private float chromaticTransitionSpeed = .1f;

    [Header("Transition Settings")]
    [SerializeField] private bool doTransition = true;
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private float transitionWaitTravelDuration = 1f;
    [SerializeField] private float transitionWaitBackDuration = 1f;

    private void OnValidate()
    {
        GetGameManager();
        GetVolume();
        GetReferences();
            
    }

    private void Awake()
    {
        GetGameManager();
        GetVolume();
    }

    private void Start()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        //Set Default Values

        if (volume.profile.TryGet<Bloom>(out bloom))
        {
            bloom.intensity.value = baseBloomIntensity;
        }

        if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        {
            chromaticAberration.intensity.value = baseChromaticIntensity;
        }

        if (volume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            lensDistortion.intensity.value = 0f;
            lensDistortion.scale.value = 1f;
        }
    }

    private void GetGameManager()
    {
        gameManager = GameManager.Instance;
    }

    private void GetVolume()
    {
        volume = GetComponent<Volume>();
    }

    private void Update()
    {
        ChromaticAberrationChanger();
        BloomChanger();
    }

    public void ChromaticAberrationChanger()
    {
        if (!doChromaticAberration) return;

        // Calculate the range of the ping pong effect
        float range = maxChromaticIntensity - baseChromaticIntensity;

        // Apply PingPong based on time and range, then add the base intensity to start from it
        chromaticAberration.intensity.value = baseChromaticIntensity + Mathf.PingPong(Time.time * chromaticTransitionSpeed, range);
    }


    public void BloomChanger()
    {
        if (!doBloom) return;

        float range = maxBloomIntensity - baseBloomIntensity;
        bloom.intensity.value = baseBloomIntensity + Mathf.PingPong(Time.time * bloomTransitionSpeed, range);
    }

    public void StartTransitionToDifferentDimension()
    {
        StartCoroutine(TransitionToDifferentDimension());
    }

    public void StartTransitionBack()
    {
        StartCoroutine(TransitionBack());
    }

    private IEnumerator TransitionToDifferentDimension()
    {
        float elapsedTime = 0f;
        float startIntensity = lensDistortion.intensity.value;
        float startScale = lensDistortion.scale.value;

        while (elapsedTime < transitionDuration)
        {
            // Lerp intensity and scale to desired values
            lensDistortion.intensity.value = Mathf.Lerp(startIntensity, -1f, elapsedTime / transitionDuration);
            lensDistortion.scale.value = Mathf.Lerp(startScale, 0.5f, elapsedTime / transitionDuration); // Assuming 0.5 is the minimum scale value
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lensDistortion.intensity.value = -1f; // Ensure final value is set
        lensDistortion.scale.value = 0.5f; // Ensure final scale value is set

        StartTransitionBack();
    }

    private IEnumerator TransitionBack()
    {
        float elapsedTime = 0f;
        float startIntensity = lensDistortion.intensity.value;
        float startScale = lensDistortion.scale.value;

        while (elapsedTime < transitionDuration)
        {
            // Lerp intensity and scale back to default values
            lensDistortion.intensity.value = Mathf.Lerp(startIntensity, 0f, elapsedTime / transitionDuration);
            lensDistortion.scale.value = Mathf.Lerp(startScale, 1f, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lensDistortion.intensity.value = 0f; // Reset to default value
        lensDistortion.scale.value = 1f; // Reset scale to default
    }
}
