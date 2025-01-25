using UnityEngine;

public class RostamMovement : MonoBehaviour
{
    // Movement parameters - exposed in Unity Inspector
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 360f;
    
    // Internal references
    private Rigidbody rb;
    private Vector3 movement;
    private Camera mainCamera;

    private void Start()
    {
        // Get component references
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        
        // Ensure we don't fall over
        rb.freezeRotation = true;
        
        // Lock cursor for game-feel testing
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        // Get input axes
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Create movement vector relative to camera
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        
        // Remove vertical component for ground movement
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Calculate movement direction
        movement = forward * verticalInput + right * horizontalInput;
        movement = movement.normalized;

        // Handle rotation if we're moving
        if (movement != Vector3.zero)
        {
            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            
            // Smoothly rotate towards movement direction
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }
    }

    private void FixedUpdate()
    {
        // Apply movement in physics update
        if (movement != Vector3.zero)
        {
            // Move using rigidbody for proper physics interactions
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // Helper method to check if character is moving
    public bool IsMoving()
    {
        return movement != Vector3.zero;
    }
}
