using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FragmentCounter : MonoBehaviour
{
    [SerializeField] float AnimationTime = 1.0f;
    [SerializeField] RectTransform Transform;
    [SerializeField] HorizontalLayoutGroup HorizontalLayoutGroup;
    [SerializeField] FragmentDisplay FragmentDisplayPrefab;
    [SerializeField] OneShotAudioPlayer OneShotAudioPlayer;
   
    List<FragmentDisplay> FragmentDisplays = new List<FragmentDisplay>();

    int CollectedFragmentsCounter = 0;
   
    void Start()
    {
        Init();
    }

    private void OnDisable()
    {
        Fragment.OnFragmentCollected -= UpdateCollectedFragments;
    }

    void Init()
    {
        Fragment.OnFragmentCollected += UpdateCollectedFragments;
        for (int i = 0; i < GameManager.Instance.FragmentController.GetAmountOfFragments(); i++)
        {
            var Instance = Instantiate(FragmentDisplayPrefab, HorizontalLayoutGroup.transform);
            FragmentDisplays.Add(Instance);
        }
    }
    void UpdateCollectedFragments()
    {
        FragmentDisplays[CollectedFragmentsCounter].ActivateFragmentFillImage();
        
        CollectedFragmentsCounter++;
        OneShotAudioPlayer.PlayOneShotAudioClip();
    }
}
