using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TransformGrid : MonoBehaviour
{
    [SerializeField] CreateGrid createGrid;
    [Header("Position")]
    [SerializeField] float moveFrequency = 1.0f;
    [SerializeField] float moveAmplitude = 1.0f;
    [Header("Rotation")]
    [SerializeField] Vector3 rotationSpeed = new(0.1f, 0.1f, 0.1f);
    [Header("Scale")]
    [SerializeField] bool useCosForScale = false;
    [SerializeField] Vector3 minScale = new(0.5f, 0.5f, 0.5f);
    [SerializeField] Vector3 maxScale = new(1.5f, 1.5f, 1.5f);
    [SerializeField] float scaleFrequency = 1.0f;
    [SerializeField] float scaleAmplitude = 1.0f;
    float scaleX;
    float scaleY;
    float scaleZ;
    [Header("Color")]
    [SerializeField] bool randomColor = false;
    [SerializeField] float colorChangeInterval = 1.0f;
    
    //float colorChangeDuration;
    //float colorChangeTimer = 0.0f;

    void Update()
    {
        for (int i = 0; i < createGrid.grid.Count; i++)
        {
            var cube = createGrid.grid[i];
            TransformPostion(cube, i);
            TransformRotation(cube);
            TransformScale(cube);
            if (randomColor) 
                LerpToRandomColor(cube);
        }
    }

    void TransformPostion(GameObject gameObject, int i)
    {
        gameObject.transform.localPosition = createGrid.startPosition[i] + Vector3.up * Mathf.Sin(Time.time * moveFrequency) * moveAmplitude;
    }

    void TransformRotation(GameObject gameObject)
    {
        gameObject.transform.Rotate(rotationSpeed);
    }

    void TransformScale(GameObject gameObject)
    {
        var position = gameObject.transform.position;
        var scaleFreq = Time.time * scaleFrequency;

        if (useCosForScale)
        {
            scaleX = maxScale[0] + minScale[0] * Mathf.Cos(scaleFreq + position.x) * scaleAmplitude;
            scaleY = maxScale[1] + minScale[1] * Mathf.Cos(scaleFreq + position.y) * scaleAmplitude;
            scaleZ = maxScale[2] + minScale[2] * Mathf.Cos(scaleFreq + position.z) * scaleAmplitude;
        }
        else
        {
            scaleX = maxScale[0] + minScale[0] * Mathf.Sin(scaleFreq + position.x) * scaleAmplitude;
            scaleY = maxScale[1] + minScale[1] * Mathf.Sin(scaleFreq + position.y) * scaleAmplitude;
            scaleZ = maxScale[2] + minScale[2] * Mathf.Sin(scaleFreq + position.z) * scaleAmplitude;
        }

        gameObject.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    // With a little help from AI
    void LerpToRandomColor(GameObject gameObject)
    {
        if (!createGrid.lerpStates.TryGetValue(gameObject, out var state)) return;
        state.timer += Time.deltaTime;
        var t = Mathf.Clamp01(state.timer / colorChangeInterval);
        gameObject.GetComponent<Renderer>().material.color = Color.Lerp(state.startColor, state.targetColor, t);
        if (t >= 1.0f)
        {
            state.startColor = state.targetColor;
            state.targetColor = new Color(Random.value, Random.value, Random.value);
            state.timer = 0f;
        }

        /* 
        Previous version before using Dictionary to store state per cube
        
        colorChangeTimer += Time.deltaTime;
        if (colorChangeTimer !>= colorChangeInterval) return;
        var randomColor = new Color(Random.value, Random.value, Random.value);
        var currentColor = gameObject.GetComponent<Renderer>().material.color;
        colorChangeDuration = Mathf.Clamp01(colorChangeInterval / Time.deltaTime);
        gameObject.GetComponent<Renderer>().material.color = Color.Lerp(currentColor, randomColor, colorChangeDuration);
        if (colorChangeDuration >= 1)
            colorChangeTimer = 0.0f;
        */
    }
}
