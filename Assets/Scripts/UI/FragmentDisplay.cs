using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FragmentDisplay : MonoBehaviour, ITweenable<Color>, ITweenable<TweenTransform>, ITweenable<Vector3>
{
    [SerializeField] Image FragmentFillImage;
    [SerializeField] RectTransform Transform;
    [SerializeField] TweenRequest<Color> ColorTween;
    [SerializeField] TweenRequest<TweenTransform> TransformTween;
    [SerializeField] TweenRequest<Vector3> RotationTween;

    TweenRequest<TweenTransform> ReverseTween;
    
    private void Start()
    {
        FragmentFillImage.enabled = false;
        ColorTween.ToTween = this;
        TransformTween.ToTween = this;
        RotationTween.ToTween = this;

        ReverseTween = TransformTween;
        
        TransformTween.OnTweenFinished = () =>
        {
            TweenManager.Instance.StartTweenTransform(ReverseTween, true);
        };
    }

    public void ActivateFragmentFillImage()
    {
        FragmentFillImage.enabled = true;
        
        TweenManager.Instance.StartTweenColor(ColorTween);
        TweenManager.Instance.StartTweenTransform(TransformTween);
        TweenManager.Instance.StartTweenRotation(RotationTween);
    }
    public void SetTween(Color NewValue)
    {

        FragmentFillImage.color = NewValue;
    }
    public void SetTween(TweenTransform NewValue)
    {
        Transform.localScale = NewValue.Scale;
    }
    public void SetTween(Vector3 NewValue)
    {
        Transform.rotation = Quaternion.Euler(NewValue);
    }
}
