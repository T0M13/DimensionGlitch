using System.Collections;
using UnityEngine;

public class SpriteFXPool : ObjectPool<SpriteRenderer>
{
    [SerializeField] private float TimeUntilFXObjectReturnsToPool = 1.0f;
    public override SpriteRenderer GetObjectFromPool(Vector2 InitialPosition)
    {
        SpriteRenderer SpriteRenderer = GetAvailableObject();
        SpriteRenderer.transform.position = InitialPosition;
        SpriteRenderer.gameObject.SetActive(true);
        return SpriteRenderer;
    }

    public void SpawnSpriteObjectAtPosition(Vector2 Position)
    {
        SpriteRenderer spriteRenderer = GetObjectFromPool(Position);

        StartCoroutine(DeactivationTimer(spriteRenderer.gameObject));
    }

    IEnumerator DeactivationTimer(GameObject GameObject)
    {
        yield return new WaitForSeconds(TimeUntilFXObjectReturnsToPool);
        
        GameObject.SetActive(false);
    }
}
