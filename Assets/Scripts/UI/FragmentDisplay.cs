using System;
using UnityEngine;
using UnityEngine.UI;

public class FragmentDisplay : MonoBehaviour
{
    [SerializeField] Image FragmentFillImage;

    private void Start()
    {
        FragmentFillImage.enabled = false;
    }

    public void ActivateFragmentFillImage()
    {
        FragmentFillImage.enabled = true;
    }
}
