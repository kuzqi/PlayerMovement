using UnityEngine;

/*
* @author kuzqi
*/
public class PlayerMovement : MonoBehaviour
{

    [Header("Values")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float inAirSpeed = 5f;
    [SerializeField] private float crouchSpeed = 3f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float gravity = -19.62f;
    [SerializeField] private float jumpHeight = 1.72f;

    // Movement variables
    private Vector3 velocity;
    
    [SerializeField] private CharacterController playerController;
    public Transform groundChecker;
    [SerializeField] private float groundDistance = 0.5f;
    [SerializeField] private LayerMask groundMask;

    private bool isGrounded;

    // Update is called every frame
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Retrieve input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Determine grounded status
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

        // Reset vertical velocity if grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Normalized movement vector
        Vector3 movement = (transform.right * x + transform.forward * z).normalized;

        // Calculate speed based on player state
        float currentSpeed = GetCurrentSpeed(z);
        
        // Move player controller
        playerController.Move(movement * currentSpeed * Time.deltaTime);
        
        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded) {Jump();}

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        
        // Move player based on velocity
        playerController.Move(velocity * Time.deltaTime);
    }


    private void Jump()
    {
        // Edit the velocity of y with Math Sqrt and multiply jumpHeight by -2f and by gravity to force him to back to ground
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
    
    private float GetCurrentSpeed(float verticalInput)
    {
        // Check if the player on ground, true = adjust the speed to normal, false?= put it to air speed.
        float baseSpeed = isGrounded ? speed : inAirSpeed;
        
        // If player pressed the Left Shift and he is on ground ajust the speed to sprint if false adjust the speed to the base speed
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : baseSpeed;

        // If player press 'C' key on the keyboard adjust the current speed to crouch speed
        if (Input.GetKey(KeyCode.C))
        {
            currentSpeed = crouchSpeed;
        }

        // Reduce speed when moving backward
        if (verticalInput < 0)
        {
            // Multiply the current speed by 0.68f to reduce it
            currentSpeed *= 0.68f;
        }
        
        return currentSpeed;
    }
}
