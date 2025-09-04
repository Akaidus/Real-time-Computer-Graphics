using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CubeGrid : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] List<GameObject> grid = new();
    [SerializeField] int gridSize = 10;
    [SerializeField] [Range(0,2)] float[] maxSize, minSize;
    [SerializeField] [Range(0,10)] float timeScale = 1;
    [SerializeField] float cubeSpacing = 2.0f;
    [SerializeField] GameObject cubePrefab;

    void Awake()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    CreateCubeInGrid(x - (gridSize / 2), y - (gridSize / 2), z - (gridSize / 2));
                }
            }
        }
    }

    void CreateCubeInGrid(float x, float y, float z)
    {
        Vector3 position = new Vector3(x * cubeSpacing, y * cubeSpacing, z * cubeSpacing);
        GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity, transform);
        grid.Add(cube);
        SetColor(cube, x, y, z);
    }

    void SetColor(GameObject gameobject, float x, float y, float z)
    {
        float r = x / gridSize;
        float g = y / gridSize;
        float b = z / gridSize;
        Color color = new Color(r, g, b);
        gameobject.GetComponent<Renderer>().material.color = color;
    }

    void Update()
    {
        RotateGrid();
        float time = Time.time * timeScale;
        for (int i = 0; i < grid.Count; i++)
        {
            GameObject cube = grid[i];
            Vector3 position = cube.transform.position;

            float scaleX = maxSize[0] + minSize[0] * Mathf.Sin(time + position.x);
            float scaleY = maxSize[1] + minSize[1] * Mathf.Sin(time + position.y);
            float scaleZ = maxSize[2] + minSize[2] * Mathf.Sin(time + position.z);

            cube.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }
    }

    void RotateGrid()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }
}
