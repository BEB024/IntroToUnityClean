using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject speedPowerUp;
    public GameObject timePowerUp;
    public GameObject doublePowerUp;

    public float spawnInterval = 2f;   // Seconds between spawns
    public int maxCount = 0;           // 0 = unlimited

    [Header("Spawn Area (World Coordinates)")]
    public Vector3 center = Vector3.zero;
    public Vector2 size = new Vector2(10f, 10f); // X-Z area (width x depth)
    public float spawnHeight = 1f;

    private int spawnedCount = 0;

    private GameObject[] powerUps;

    void Start()
    {
        // Store all power-ups in an array for random selection
        powerUps = new GameObject[] { speedPowerUp, timePowerUp, doublePowerUp };

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (maxCount == 0 || spawnedCount < maxCount)
        {
            SpawnRandomPowerUp();
            spawnedCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnRandomPowerUp()
    {
        // Generate random position within defined area
        float x = center.x + Random.Range(-size.x / 2f, size.x / 2f);
        float z = center.z + Random.Range(-size.y / 2f, size.y / 2f);
        Vector3 pos = new Vector3(x, spawnHeight, z);

        // Randomly choose one of the three power-ups
        int index = Random.Range(0, powerUps.Length);
        GameObject selectedPowerUp = powerUps[index];

        // Spawn the chosen power-up
        Instantiate(selectedPowerUp, pos, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        // Draw the spawn area in the Scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(center.x, spawnHeight, center.z),
                            new Vector3(size.x, 0.1f, size.y));
    }
}
