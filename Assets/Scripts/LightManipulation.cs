using UnityEngine;

public class LightManipulation : MonoBehaviour
{
    [SerializeField] new Light light;
    [SerializeField] bool cosX, cosY, cosZ;
    [SerializeField] [Tooltip("For figure 8: x: 1, y: 3 (cos).")] Vector3 amplitude = new(1,1,1);
    [SerializeField] [Tooltip("For figure 8: x: 2, y: 1 (cos).")] Vector3 frequency = new(1,1,1);
    [SerializeField] Color[] colors = new Color[] { Color.white, Color.red };
    [SerializeField] float colorChangeSpeed = 1f;
    float x, y, z;

    void Update()
    {
        if (cosX)
            x = Mathf.Cos(Time.time * frequency[0]) * amplitude[0];
        else
            x = Mathf.Sin(Time.time * frequency[0]) * amplitude[0];
        if (cosY)
            y = Mathf.Cos(Time.time * frequency[1]) * amplitude[1];
        else
            y = Mathf.Sin(Time.time * frequency[1]) * amplitude[1];
        if (cosZ)
            z = Mathf.Cos(Time.time * frequency[2]) * amplitude[2];
        else
            z = Mathf.Sin(Time.time * frequency[2]) * amplitude[2];

        light.transform.position = new Vector3(x, y, z);
        float t = Mathf.Clamp01((Mathf.Sin(Time.time * colorChangeSpeed)));
        light.color = Color.Lerp(colors[0], colors[1], t);
    }
}
