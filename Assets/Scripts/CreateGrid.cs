using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    [SerializeField] public List<GameObject> grid = new();
    [SerializeField] public List<Vector3> startPosition = new();
    [SerializeField] int gridSize = 10;
    [SerializeField] float cubeSpacing = 2.0f;
    [SerializeField] GameObject cubePrefab;

    //Color stuff (Dictionary help from AI)
    public class LerpState
    {
        public Color startColor;
        public Color targetColor;
        public float timer;
    }
    [HideInInspector] public Dictionary<GameObject, LerpState> lerpStates = new();

    void Awake()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    var centerOffset = gridSize / 2;
                    CreateCubeInGrid(x - centerOffset, y - centerOffset, z - centerOffset);
                }
            }
        }

    }

    void CreateCubeInGrid(float x, float y, float z)
    {
        var position = new Vector3(x * cubeSpacing, y * cubeSpacing, z * cubeSpacing);
        var cube = Instantiate(cubePrefab, position, Quaternion.identity, transform);
        startPosition.Add(position);
        grid.Add(cube);
        SetColor(cube, x, y, z);
        lerpStates[cube] = new LerpState
        {
            startColor = cube.GetComponent<Renderer>().material.color,
            targetColor = cube.GetComponent<Renderer>().material.color,
            timer = 0f
        };
    }

    void SetColor(GameObject gameobject, float x, float y, float z)
    {
        var r = x / gridSize;
        var g = y / gridSize;
        var b = z / gridSize;
        var color = new Color(r, g, b);
        gameobject.GetComponent<Renderer>().material.color = color;
    }

    public Vector3 GetCenter()
    {
        return transform.position;
    }
}
