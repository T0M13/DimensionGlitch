using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip animationClip;
    private float timer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= animationClip.length)
        {
            gameObject.SetActive(false);
            timer = 0;
        }
    }


}
