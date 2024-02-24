using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPool : ObjectPool<AudioSource>
{
    public override AudioSource GetObjectFromPool(Vector2 InitialPosition)
    {
        AudioSource Source = GetAvailableObject();
        Source.gameObject.SetActive(true);
        Source.transform.position = InitialPosition;

        return Source;
    }
}
