using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private CharacterController playerController;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float inAirSpeed = 4f;
    [SerializeField] private float crouchSpeed = 3f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float jumpHeight = 1.5f;
    private Vector3 velocity;
    private float gravity = -25f;

    [Header("Ground")]
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundChecker;
    private bool isGrounded;

    [Header("Camera")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float lookXLimit = 45f;
    private float rotationX = 0;

    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        HandleInput();
        HandleCamera();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleCamera()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        
    }

    private void HandleInput()
    {

        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f; 
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
        velocity.y += gravity * Time.fixedDeltaTime;
        playerController.Move(velocity * Time.fixedDeltaTime);
    }

    private float GetCurrentSpeed(float verticalInput)
    {
        float baseSpeed = isGrounded ? speed : inAirSpeed;

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
            return baseSpeed * 0.65f;
        }

        return baseSpeed;
    }
}
