using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float rayCastDistance = 5.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private Vector2 rayOffset = Vector2.zero;
    [SerializeField] private float detectColiderDistance = 5f; 


    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }


    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + rayOffset, Vector2.down, rayCastDistance, groundLayer);
        float distance = ((Vector2)transform.position - hit.point).magnitude;
        Debug.Log(distance);

        if (hit.collider != null && distance < detectColiderDistance)
        {
            // Calculate the angle to match the ground normal
            Vector2 groundNormal = hit.normal;
            float targetAngle = Vector2.SignedAngle(Vector2.up, groundNormal);

            // Get current rotation
            float currentAngle = transform.eulerAngles.z;
            if (currentAngle > 180) currentAngle -= 360;

            // Smoothly interpolate to the target angle
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

            // Apply the rotation
            if (rb != null)
            {
                // If using Rigidbody2D, use MoveRotation for physics-based rotation
                rb.MoveRotation(newAngle);
            }
            else
            {
                // If no Rigidbody2D, use transform directly
                transform.rotation = Quaternion.Euler(0, 0, newAngle);
            }

        }



    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            (Vector2)transform.position + rayOffset,
            (Vector2)transform.position + rayOffset + Vector2.down * rayCastDistance
        );
    }
}
