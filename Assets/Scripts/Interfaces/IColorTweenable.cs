using UnityEngine;

public interface ITweenable<T>
{
    public void SetTween(T NewValue);
}
