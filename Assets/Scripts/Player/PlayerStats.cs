using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : Stats
{
    [SerializeField, Min(0.1f)] float InvincibilityTime = 0.2f;
    [SerializeField, Min(0), Tooltip("Needs to be a multiple of two")] private int TimesToBlink = 4;
    [SerializeField] SpriteRenderer SpriteRenderer;
    
    Color DefaultColor;
    private void OnValidate()
    {
        if (TimesToBlink % 2 != 0)
        {
            Debug.LogError("You didnt set a multiple of two");
        }
    }

    private void Start()
    {
        DefaultColor = SpriteRenderer.color;
    }

    public override bool RecieveDmg()
    {
        StartCoroutine(Invincibility());

        return base.RecieveDmg();
    }

    IEnumerator Invincibility()
    {
        SetInvincibility(true);
        float PassedTime = 0.0f;
        float BlinkTimer = 0.0f;
        float BlinkStep = InvincibilityTime / TimesToBlink;

        bool ZeroAlpha = true;

        while (PassedTime <= InvincibilityTime)
        {
            float DeltaTime = Time.deltaTime;
            PassedTime += DeltaTime;
            BlinkTimer += DeltaTime;

            if (BlinkTimer >= BlinkStep)
            {
                SpriteRenderer.color = ZeroAlpha ? Color.clear : DefaultColor;
                BlinkTimer -= BlinkStep;
                ZeroAlpha = !ZeroAlpha;
            }

            yield return null;
        }

        SetInvincibility(false);
    }
}
