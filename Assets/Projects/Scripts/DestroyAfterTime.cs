using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawnPoint;

    private GameObject spawnedObject;
    private bool isSpawned = false;

    void Update()
    {
        // If space is pressed and no object currently exists
        if (Input.GetKeyDown(KeyCode.Space) && !isSpawned)
        {
            spawnedObject = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            Destroy(spawnedObject, 3f);
            isSpawned = true;
        }

        // If the object was spawned but has since been destroyed
        if (isSpawned && spawnedObject == null)
        {
            isSpawned = false;
        }
    }
}
