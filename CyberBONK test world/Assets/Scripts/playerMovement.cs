using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public static Transform player;
    public static GameObject playerCameraObject;
    public float speed = 12f;
    //public float gravity = -9.81f;
    public float jumpForce = 100f;
    public float maxVelocityChange = 10.0f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Rigidbody rb;

    Vector3 velocity;
    Vector3 targetVelocity;
    bool isGrounded;    
    bool jumping;
    public Animator animator;


    private void Start() {
        player = this.transform;
        animator = GetComponentInChildren<Animator>();
        playerCameraObject = GetComponentInChildren<Camera>().gameObject;
    }

    private void Update() {
        targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if(Input.GetButtonDown("Jump") && isGrounded && !jumping)
        {
            jumping = true;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && jumping)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0));
            jumping = false;
        }
        //animator.SetBool("IsGrounded", isGrounded);

        // if(isGrounded && velocity.y < 0)
        // {
        //     velocity.y = -2f;
        // }

        //float x = Input.GetAxis("Horizontal");
        //float z = Input.GetAxis("Vertical");

        //animator.SetFloat("InputHorizontal", x);
        //animator.SetFloat("InputVertical",  z);
        //animator.SetFloat("InputMagnitude", new Vector3(x, 0, z).magnitude);

        //Vector3 move = transform.right * x + transform.forward * z;

        //rb.velocity = new Vector3(move.x * speed * Time.deltaTime, rb.velocity.y ,move.z * speed * Time.deltaTime);


        // Calculate how fast we should be moving
        
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        if (Mathf.Abs(rb.velocity.z) > 0.1f && Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        //transform.position = transform.position + (move * speed * Time.deltaTime);
        
        //+ (move * speed * Time.deltaTime);


        //velocity.y += gravity * Time.deltaTime;

        //controller.Move(velocity * Time.deltaTime);
    }
}
