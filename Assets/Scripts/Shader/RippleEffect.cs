using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    [SerializeField] Material rippleMaterial;
    [SerializeField] [Tooltip("How intense should the ripples be (0.01 is good)")] float maxRippleAmplitude;
    [SerializeField] [Tooltip("How many ripples should appear at max (12 is good)")] float maxRippleCount;
    [SerializeField] [Tooltip("How long should the ripple last")] float rippleDuration;
    float amplitude;
    float count;
    float duration;
    float repeatRate = 0.1f;
    Collider col;
    void Awake()
    {
        rippleMaterial.SetFloat("_RippleAmplitude", 0);
        rippleMaterial.SetFloat("_RippleCount", 0);
        col = GetComponent<Collider>();
    }

    public void BeginRipple()
    {
        CancelInvoke(nameof(DecreaseRipple));
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

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Get the closest point on the collider's bounds relative to the player's position
        Vector3 closestPoint = col.ClosestPoint(other.transform.position);

        // Normalize the closest point (X and Z) to the range [-1, 1] based on the collider's bounds
        float normalizedZ = Mathf.InverseLerp(col.bounds.min.z, col.bounds.max.z, closestPoint.z) * 2 - 1;
        float normalizedY = Mathf.InverseLerp(col.bounds.min.y, col.bounds.max.y, closestPoint.y) * 2 - 1;

        rippleMaterial.SetVector("_RippleOrigin", new Vector4(-normalizedZ, 0f, -normalizedY, 0f)); // Use Z for the 2D effect
        BeginRipple();
    }

}
