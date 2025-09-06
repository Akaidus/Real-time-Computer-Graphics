using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RayCastManipulation : MonoBehaviour
{
    [SerializeField] bool raycastPierce;
    [SerializeField] int maxRaycasts = 10;
    [SerializeField][Range(0,0.01f)] float regenerationRate = 0.001f;
    [SerializeField] CreateGrid createGrid;
    List<GameObject> hitCubes = new();
    Stopwatch purgeTime;
    bool isRegenerating = false;

    void Update()
    {
        if (hitCubes.Count >= createGrid.grid.Count)
        {
            purgeTime.Stop();
            print($"All cubes hit! Time taken: {purgeTime.ElapsedMilliseconds} ms");
            StartCoroutine(RegenerateGrid());
        }

        var gridCenter = createGrid.GetCenter();
        if (isRegenerating) return;
        if (raycastPierce)
            RaycastPierce(gridCenter);
        else
            RaycastSingle(gridCenter);
    }

    void RaycastPierce(Vector3 gridCenter)
    {
        for (int i = 0; i < maxRaycasts; i++)
        {
            if (isRegenerating) break;
            RaycastHit[] hits;
            hits = Physics.RaycastAll(gridCenter, Random.onUnitSphere, Mathf.Infinity);
            foreach (var hit in hits)
            {
                if (hitCubes.Count == 0 || purgeTime.ElapsedMilliseconds == 0)
                {
                    purgeTime = Stopwatch.StartNew();
                }
                if(hitCubes.Count >= createGrid.grid.Count) break;
                hitCubes.Add(hit.collider.gameObject);
                hit.collider.gameObject.SetActive(false);
            }
        }
    }

    void RaycastSingle(Vector3 gridCenter)
    {
        for (int i = 0; i < maxRaycasts; i++)
        {
            if (isRegenerating) break;
            RaycastHit hit;
            if (Physics.Raycast(gridCenter, Random.onUnitSphere, out hit, Mathf.Infinity))
            {
                if (hitCubes.Count == 0 || purgeTime.ElapsedMilliseconds == 0)
                {
                    purgeTime = Stopwatch.StartNew();
                }
                if(hitCubes.Count >= createGrid.grid.Count) break;
                hitCubes.Add(hit.collider.gameObject);
                hit.collider.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator RegenerateGrid()
    {
        if (isRegenerating) yield break; // add bool and break here to counter collection modification error
        isRegenerating = true;
        yield return new WaitForSeconds(0.2f);  
        foreach (var cube in hitCubes)
        {
            yield return new WaitForSeconds(regenerationRate);
            cube.SetActive(true);
        }
        yield return new WaitForSeconds(0.2f);
        hitCubes.Clear();
        isRegenerating = false;
    }

    void OnDrawGizmos()
    {
        if (createGrid == null) return;
        var gridCenter = createGrid.GetCenter();
        for (int i = 0; i < maxRaycasts; i++)
        {
            var direction = Random.onUnitSphere;
            var ray = new Ray(gridCenter, direction);
            Gizmos.DrawRay(ray.origin, ray.direction * 100);
        }
    }
}
