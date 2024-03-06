using UnityEngine;

[System.Serializable]
public struct CropGrowthState
{
    public float duration;
    public CropState growthStage;
    public Sprite sprite;
    public bool colliderActive;
    public Vector2 colliderSize; 
    public Vector2 colliderOffset; 
}

