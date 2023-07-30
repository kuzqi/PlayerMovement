using UnityEngine;

public class PlayerControls : MonoBehaviour {

    private Vector3 moveDirection;

    private Animator anim;

    private CharacterController controller;

    private CameraControls cam;

    public float jump = 10;

    public float gravity = 20;

    private bool onGround = true;

    private float ConstantForce = -10;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraControls>();
    }

    void Update()
    {
        onGround = controller.isGrounded;

        if (anim.GetBool("Ground") != onGround)
        {
            anim.SetBool("Ground", onGround);
            if (onGround)
            {
                ConstantForce = -10;
            }
            else if (ConstantForce == -10) ConstantForce = 0;
        }
        if (onGround)
        {
                if (Input.GetKey(KeyCode.Space)) 
            {
                anim.SetBool("Jump", true);
                ConstantForce = jump;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                anim.SetBool("Jump", false);
                ConstantForce = jump;
            }

            if (Input.GetKey(KeyCode.K))
            {
                anim.SetBool("Dance", true);
            }

            if (Input.GetKeyUp(KeyCode.K))
            {
                anim.SetBool("Dance", false);
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space))
            {
                if (anim.GetBool("Dance"))
                {
                    anim.SetBool("Dance", false);
                }
            }

        }
        else ConstantForce -= gravity * Time.deltaTime;

        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        anim.SetFloat("Horizontal", moveDirection.x, 0.1f, (2.5f - moveDirection.magnitude) * Time.deltaTime);
        anim.SetFloat("Vertical", moveDirection.z, 0.1f, (2.5f - moveDirection.magnitude) * Time.deltaTime);
        
        Vector3 translate = moveDirection.normalized * 5; 
        if (translate.z < 0) translate *= 0.75f;
        translate.y = ConstantForce;
        controller.Move(transform.rotation * translate * Time.deltaTime);
    }

    public void SetRotation(float rot)
    {
        if (moveDirection.magnitude > 0)
            transform.rotation = Quaternion.Euler(0, rot, 0);
    }

}
