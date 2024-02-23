using System.Collections;
using UnityEngine;

public class SpearTrap : MonoBehaviour
{
    [SerializeField] float OffsetToCenter = 10;
    [SerializeField] float TrapExecutionTime = 2.0f;
    [SerializeField] private float TargetOffsetUpper = 0.0f;
    [SerializeField] private float TargetOffsetLower = 0.0f;
    [SerializeField] TrapTrigger TrapTrigger;
    [SerializeField] private DamageTrigger UpperSpear;
    [SerializeField] private DamageTrigger LowerSpear;
    
    [Header("Animation")] 
    [SerializeField] string AnimationStateName = "Stab";
    [SerializeField] Animator UpperSpearAnimator;
    [SerializeField] Animator LowerSpearAnimator;
    
    private int HashedAnimationStateName = 0;
    private void OnValidate()
    {
        if (UpperSpear && LowerSpear)
        {
            Vector2 UpperOffset = Vector2.up * OffsetToCenter;
            Vector2 LowerOffset = Vector2.down * OffsetToCenter;
        
            UpperSpear.transform.localPosition = UpperOffset;
            LowerSpear.transform.localPosition = LowerOffset;

            if (UpperSpear.GetTriggerCollider() && UpperSpear.GetTriggerCollider())
            {
                TargetOffsetUpper = OffsetToCenter - UpperSpear.GetTriggerCollider().size.y / 2;
                TargetOffsetLower = OffsetToCenter - LowerSpear.GetTriggerCollider().size.y / 2;
            }
        }
    }

    private void Start()
    {
        HashedAnimationStateName = Animator.StringToHash(AnimationStateName);
        TrapTrigger.OnTrapTriggerTriggered += DoStab;
        SetSpearsEnbled(false);
    }

    private void OnDisable()
    {
        TrapTrigger.OnTrapTriggerTriggered -= DoStab;
    }

    void SetSpearsEnbled(bool Enabled)
    {
        UpperSpear.SetTrigger(Enabled);
        LowerSpear.SetTrigger(Enabled);
    }
    void DoStab()
    {
        StopAllCoroutines();
        StartCoroutine(Stab());
    }
    IEnumerator Stab()
    {
        float SpearExtractionTime = TrapExecutionTime * 0.5f;
        
        SetSpearsEnbled(true);
        yield return AnimateSpears(SpearExtractionTime, false);
        
        SetSpearsEnbled(false);
        yield return AnimateSpears(SpearExtractionTime, true);
        
       
    }

    IEnumerator AnimateSpears(float SpearExtractionTime, bool Reverse)
    {
        float PassedTime = 0.0f;
        float Timer = Reverse ? SpearExtractionTime : 0;
        
        while (PassedTime < SpearExtractionTime)
        {
            PassedTime += Time.deltaTime; 
            Timer += Reverse ? -Time.deltaTime : Time.deltaTime;
            float InterpValue = Timer / SpearExtractionTime;
           
            UpperSpear.GetTriggerCollider().offset = new Vector2(0, Mathf.Lerp(0, TargetOffsetUpper, InterpValue));
            LowerSpear.GetTriggerCollider().offset = new Vector2(0, Mathf.Lerp(0, TargetOffsetLower, InterpValue));
            UpperSpearAnimator.Play(HashedAnimationStateName, 0, InterpValue);
            LowerSpearAnimator.Play(HashedAnimationStateName, 0, InterpValue);
            yield return new WaitForEndOfFrame();
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (TrapTrigger)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, TrapTrigger.transform.position);
        }
    }
}
