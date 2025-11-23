using UnityEngine;
using System.Collections;

public class RandomSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject prefab;
    public float spawnInterval = 2f;   // Seconds between spawns
    public int maxCount = 0;           // 0 = unlimited

    [Header("Spawn Area (World Coordinates)")]
    public Vector3 center = Vector3.zero;
    public Vector2 size = new Vector2(10f, 10f); // X-Z area (width x depth)
    public float spawnHeight = 1f;

    private int spawnedCount = 0;

    // ✅ Define the same 5 colors as used in PlayerController
    private Color[] allowedColors = {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        new Color(0.6f, 0.3f, 0f) // Brown
    };

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (maxCount == 0 || spawnedCount < maxCount)
        {
            SpawnRandomObject();
            spawnedCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnRandomObject()
    {
        // ✅ Generate random position within defined area
        float x = center.x + Random.Range(-size.x / 2f, size.x / 2f);
        float z = center.z + Random.Range(-size.y / 2f, size.y / 2f);
        Vector3 pos = new Vector3(x, spawnHeight, z);

        GameObject go = Instantiate(prefab, pos, Quaternion.identity);

        // ✅ Assign one of the five allowed colors randomly
        var renderer = go.GetComponent<Renderer>();
        if (renderer != null)
        {
            int randomIndex = Random.Range(0, allowedColors.Length);
            renderer.material.color = allowedColors[randomIndex];
        }
    }

    void OnDrawGizmosSelected()
    {
        // ✅ Draw the spawn area in editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(center.x, spawnHeight, center.z),
                            new Vector3(size.x, 0.1f, size.y));
    }
}


/*
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int count = 5;
    // public color z;

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(i * 2f, 1f, 0);
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);
            var renderer = go.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(Random.value, Random.value, Random.value);
                // renderer.material.color = z;
            }
        }
    }
}
*/