using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Chase Settings")]
    public float speed = 4f;              // How fast the enemy moves
    public float detectionRadius = 10f;   // Start chasing when player is within this range
    public float stopDistance = 1.5f;     // Stop chasing when this close

    [Header("Axis Lock")]
    public bool lockY = true;             // Keep movement on a flat plane (no vertical motion)

    private Transform player;
    private Vector3 startPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        startPosition = transform.position;

        if (player == null)
            Debug.LogWarning("⚠️ EnemyAI: No object tagged 'Player' found in scene!");
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius && distance > stopDistance)
        {
            // Direction toward player (ignore Y for flat movement)
            Vector3 direction = (player.position - transform.position).normalized;
            if (lockY) direction.y = 0f;

            // Move toward player
            transform.position += direction * speed * Time.deltaTime;

            // Rotate to face player
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.2f);
            }
        }
        else if (distance > detectionRadius)
        {
            // Optional: return to start position when player leaves range
            Vector3 backDirection = (startPosition - transform.position).normalized;
            if (lockY) backDirection.y = 0f;

            if (Vector3.Distance(transform.position, startPosition) > 0.1f)
                transform.position += backDirection * speed * Time.deltaTime;
        }
    }

    // Optional: visualize detection range in scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
