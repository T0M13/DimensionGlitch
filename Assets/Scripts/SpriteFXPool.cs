using System.Collections;
using UnityEditor;
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

#if UNITY_EDITOR
    protected override void InitPool()
    {
        if(!ObjectPrefab) return;
      
        for (int i = 0; i < NumberOfObjectsToPool; i++)
        {
            SpriteRenderer SpawnedObject = (SpriteRenderer)PrefabUtility.InstantiatePrefab(ObjectPrefab, transform);
            PooledObjects.Add(SpawnedObject);
            SpawnedObject.transform.SetParent(transform);
            SpawnedObject.transform.rotation = Quaternion.identity;
            SpawnedObject.gameObject.SetActive(false);
        }
    }
#endif
}
