using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    [SerializeField] Material rippleMaterial;
    [SerializeField] [Tooltip("How intense should the ripples be (0.01 is good)")] float maxRippleAmplitude;
    [SerializeField] [Tooltip("How many ripples should appear at max (12 is good)")] float maxRippleCount;
    [SerializeField][Tooltip("How fast should ripples appear over time")] float rippleRate;
    [SerializeField] [Tooltip("How long should the ripple last")] float rippleDuration;
    float amplitude;
    float count;
    float duration;
    float repeatRate = 0.1f;
    void Awake()
    {
        rippleMaterial.SetFloat("_RippleAmplitude", 0);
        rippleMaterial.SetFloat("_RippleCount", 0);
    }

    public void BeginRipple()
    {
        amplitude = maxRippleAmplitude;
        duration = rippleDuration;
        count = maxRippleCount;
        rippleMaterial.SetFloat("_RippleCount", count);
        rippleMaterial.SetFloat("_RippleAmplitude", amplitude);
        InvokeRepeating(nameof(DecreaseRipple), 0, repeatRate);
    }

    void DecreaseRipple()
    {
        //if(count < maxRippleCount)
        //{
        //    count += rippleRate;
        //}
        //rippleMaterial.SetFloat("_RippleCount", count);
        
        duration -= repeatRate;
        amplitude = maxRippleAmplitude * (duration / rippleDuration);
        rippleMaterial.SetFloat("_RippleAmplitude", amplitude);
        
        if (duration <= 0f)
        {
            rippleMaterial.SetFloat("_RippleAmplitude", 0);
            CancelInvoke(nameof(DecreaseRipple));
        }
    }
}
