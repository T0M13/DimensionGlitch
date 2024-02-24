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
    [SerializeField] private ColorAdjustments colorAdjustments;

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

    [Header("Color Adjustments Settings")]
    [SerializeField] private bool doColorAdjustments = true;
    [SerializeField] private float baseColorAdjustmentHue = 0.2f;
    [SerializeField] private float baseColorAdjustmentSaturation = 0.2f;

    [Header("LensDistortion Settings")]
    [SerializeField] private float baseLensDistortionValue = 1f;
    [SerializeField] private float minLensDistrtionValue = -.7f;
    [SerializeField] private float maxLensDistrtionValue = .5f;


    [Header("Transition Settings")]
    [SerializeField] private bool doTransition = true;
    [SerializeField] private float transitionDuration = .3f;

    [SerializeField] private float transitionIntensityMax = -1f;
    [SerializeField] private float transitionIntensityBase = 0f;
    [SerializeField] private float transitionScaleMax = 0.5f;
    [SerializeField] private float transitionScaleBase = 1f;

    [SerializeField] private float transitionWaitTravelDuration = 1f;
    [SerializeField] private float transitionWaitBackDuration = 1f;

    [SerializeField] private float fragmentShiftDuration = .2f;
    [SerializeField] private float fragmentShiftIntensityMax = 1f;
    [SerializeField] private float fragmentShiftScaleMax = 1f;


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

        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.hueShift.value = baseColorAdjustmentHue;
            colorAdjustments.saturation.value = baseColorAdjustmentSaturation;
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
        StartCoroutine(TransitionToDifferentDimension(transitionDuration, transitionIntensityMax, transitionScaleMax));
    }

    public void StartTransitionBack()
    {
        StartCoroutine(TransitionBack(transitionDuration, transitionIntensityBase, transitionScaleBase));
    }

    public void StartTransitionToDifferentDimension(float transitionDuration, float transitionIntensityMax, float transitionScaleMax)
    {
        StartCoroutine(TransitionToDifferentDimension(transitionDuration, transitionIntensityMax, transitionScaleMax));
    }

    public void StartTransitionBack(float transitionDuration, float transitionIntensityBase, float transitionScaleBase)
    {
        StartCoroutine(TransitionBack(transitionDuration, transitionIntensityBase, transitionScaleBase));
    }

    private IEnumerator TransitionToDifferentDimension(float transitionDuration, float transitionIntensityMax, float transitionScaleMax)
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

        lensDistortion.intensity.value = transitionIntensityMax; // Ensure final value is set
        lensDistortion.scale.value = transitionScaleMax; // Ensure final scale value is set

        StartTransitionBack();
    }

    private IEnumerator TransitionBack(float transitionDuration, float transitionIntensityBase, float transitionScaleBase)
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

        lensDistortion.intensity.value = transitionIntensityBase; // Reset to default value
        lensDistortion.scale.value = transitionScaleBase; // Reset scale to default
    }


    public void StartFragmentShift()
    {
        StartCoroutine(TransitionToDifferentDimension(fragmentShiftDuration, fragmentShiftIntensityMax, fragmentShiftScaleMax));
    }

    public void SetBlackAndWhite()
    {
        colorAdjustments.saturation.value = colorAdjustments.saturation.min;
    }

    public void SetInvertedColors()
    {
        colorAdjustments.hueShift.value = colorAdjustments.hueShift.min;
    }

    public void SetMaxLensDistortion()
    {
        lensDistortion.intensity.value = maxLensDistrtionValue;
    }

    public void SetMinLensDistortion()
    {
        lensDistortion.intensity.value = minLensDistrtionValue;
    }

    public void ResetEffects()
    {
        colorAdjustments.saturation.value = baseColorAdjustmentSaturation;
        colorAdjustments.hueShift.value = baseColorAdjustmentHue;
        lensDistortion.intensity.value = baseLensDistortionValue;
    }

}
