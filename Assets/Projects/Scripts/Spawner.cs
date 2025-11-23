using UnityEngine;
public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawnPoint;

    // Boolean flag to check if object is already spawned
    private bool isSpawned = false;

    void Update()
    {
        // CHeck if Space is pressed and no object has been spawned yet
        if (Input.GetKeyDown(KeyCode.Space) && !isSpawned)
        {
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            isSpawned = true; // Set flag to true after spawning
        }
    }
}
