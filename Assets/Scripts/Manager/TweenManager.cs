using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct TweenTransform
{
    [SerializeField] public Vector2 Position;
    [SerializeField] public Vector3 EulerRotation;
    [SerializeField] public Vector3 Scale;

    
    public TweenTransform(Vector2 Position, Vector3 EulerRotation, Vector3 Scale)
    {
        this.Position = Position;
        this.EulerRotation = EulerRotation;
        this.Scale = Scale;
    }
}

[Serializable]
public struct TweenRequest<T>
{
    public TweenRequest(Action OnTweenFinished, ITweenable<T> ToTween, T From, T To, float AnimationTime, float AnimationSpeed = 1.0f)
    {
        this.OnTweenFinished = OnTweenFinished;
        this.ToTween = ToTween;
        this.From = From;
        this.To = To;
        this.AnimationTime = AnimationTime;
        this.AnimationSpeed = AnimationSpeed;
        EaseCurve = new AnimationCurve();
    }

    //implement on tween finished
    public Action OnTweenFinished;
    [SerializeField, Tooltip("The end and start values should always be greater then zero")]
    public AnimationCurve EaseCurve;
    
    public ITweenable<T> ToTween;
    public T From;
    public T To;
    public float AnimationTime;
    public float AnimationSpeed;
}

public class TweenManager : BaseSingleton<TweenManager>
{
    public void StartTweenColor(TweenRequest<Color> TweenRequest, bool ReverseTween = false)
    {
        StartCoroutine(TweenColor(TweenRequest, ReverseTween));
    }

    public void StartTweenTransform(TweenRequest<TweenTransform> TweenRequest, bool ReverseTween = false)
    {
        StartCoroutine(TweenTransform(TweenRequest, ReverseTween));
    }

    public void StartTweenRotation(TweenRequest<Vector3> TweenRequest, bool ReverseTween = false)
    {
        StartCoroutine(TweenRotation(TweenRequest, ReverseTween));
    }
    
    IEnumerator TweenColor(TweenRequest<Color> TweenRequest, bool ReverseTween = false)
    {
        float CurrentAnimTime = ReverseTween ? TweenRequest.AnimationTime : 0.0f;
        float AnimationDuration = TweenRequest.AnimationTime;
        
        while ((!ReverseTween && CurrentAnimTime < AnimationDuration) || (ReverseTween && CurrentAnimTime > 0.0f))
        {
            float InterpValue = CurrentAnimTime / AnimationDuration;
            CurrentAnimTime += GetAnimTimeIncrement(TweenRequest, InterpValue, ReverseTween);
            InterpValue = Mathf.Clamp01(InterpValue);
            
            Color NewColor = Color.Lerp(TweenRequest.From, TweenRequest.To, InterpValue);
            TweenRequest.ToTween.SetTween(NewColor);
            yield return null;
        }
        
        TweenRequest.OnTweenFinished?.Invoke();
    }

    IEnumerator TweenTransform(TweenRequest<TweenTransform> TweenRequest, bool ReverseTween = false)
    {
        float CurrentAnimTime = ReverseTween ? TweenRequest.AnimationTime : 0.0f;
        float AnimationDuration = TweenRequest.AnimationTime;
        
        while ((!ReverseTween && CurrentAnimTime < AnimationDuration) || (ReverseTween && CurrentAnimTime > 0.0f))
        {
            float InterpValue = CurrentAnimTime / AnimationDuration;
            CurrentAnimTime += GetAnimTimeIncrement(TweenRequest, InterpValue, ReverseTween);
            Debug.Log(CurrentAnimTime);
            InterpValue = Mathf.Clamp01(InterpValue);
            
            Vector2 NewPosition = Vector2.Lerp(TweenRequest.From.Position, TweenRequest.To.Position, InterpValue);
            Vector3 NewRotation = Vector3.Lerp(TweenRequest.From.EulerRotation, TweenRequest.To.EulerRotation, InterpValue);
            Vector3 NewScale = Vector3.Lerp(TweenRequest.From.Scale, TweenRequest.To.Scale, InterpValue);
            
            TweenTransform NewTweenTransform = new TweenTransform(NewPosition, NewRotation, NewScale);
            
            TweenRequest.ToTween.SetTween(NewTweenTransform);
            yield return null;
        }
        
        TweenRequest.OnTweenFinished?.Invoke();
    }
    
    IEnumerator TweenRotation(TweenRequest<Vector3> TweenRequest, bool ReverseTween = false)
    {
        float CurrentAnimTime = ReverseTween ? TweenRequest.AnimationTime : 0.0f;
        float AnimationDuration = TweenRequest.AnimationTime;
       
        while ((!ReverseTween && CurrentAnimTime < AnimationDuration) || (ReverseTween && CurrentAnimTime > 0.0f))
        {
            float InterpValue = CurrentAnimTime / AnimationDuration;
            CurrentAnimTime += GetAnimTimeIncrement(TweenRequest, InterpValue, ReverseTween);
            InterpValue = Mathf.Clamp01(InterpValue);
            
            Vector3 NewRotation = Vector3.Lerp(TweenRequest.From, TweenRequest.To, InterpValue);
           
            TweenRequest.ToTween.SetTween(NewRotation);
            yield return null;
        }
        
        TweenRequest.OnTweenFinished?.Invoke();
    }

    float GetAnimTimeIncrement<T>(TweenRequest<T> TweenRequest, float CurrentInterpValue, bool ReverseTween)
    {
        float Increment = ReverseTween ? -Time.deltaTime * TweenRequest.EaseCurve.Evaluate(CurrentInterpValue) * TweenRequest.AnimationSpeed : Time.deltaTime * TweenRequest.EaseCurve.Evaluate(CurrentInterpValue) * TweenRequest.AnimationSpeed;
        
        return Increment;
    }
  
}
