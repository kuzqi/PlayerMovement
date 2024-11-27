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
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.72f;

    private Vector3 velocity;
    private CharacterController playerController;

    [Header("Ground")]
    [SerializeField] private float groundDistance = 0.5f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform groundChecker;

    private bool isGrounded;

    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        // Input handling moved here for better responsiveness
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundLayerMask);

        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f; // Minor downward force to stick to the ground
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 movement = (transform.right * x + transform.forward * z).normalized;
        
        // Check if the player is trying to move
        if (movement.magnitude > 0)
        {
            float currentSpeed = GetCurrentSpeed(z);
            playerController.Move(movement * currentSpeed * Time.fixedDeltaTime);
        }

        ApplyGravity();
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void ApplyGravity()
    {
        // Update vertical velocity
        velocity.y += gravity * Time.fixedDeltaTime;
        playerController.Move(velocity * Time.fixedDeltaTime);
    }

    private float GetCurrentSpeed(float verticalInput)
    {
        float baseSpeed = isGrounded ? speed : inAirSpeed;

        // Select current speed based on player's actions
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            return sprintSpeed;
        }
        if (Input.GetKey(KeyCode.C))
        {
            return crouchSpeed;
        }
        if (verticalInput < 0) 
        {
            return baseSpeed * 0.65f; // Reduce the base speed when player going backwards
        }

        return baseSpeed;
    }
}
