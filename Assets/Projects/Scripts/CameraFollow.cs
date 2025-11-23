using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    public Vector3 offset;

    private void Awake()
    {
       if(player == null)
        {
            Debug.LogError("No Player Found");
        }
    }

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
