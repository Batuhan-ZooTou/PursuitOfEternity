using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveX, moveZ;
    private Rigidbody rigidbody;
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private bool isGrounded;
    public bool isCrouching = false;
    private bool isMoving;


    float xRot = 0f;
    float yRot = 0f;
    public Transform cameraTransform;
    MouseLook look;



    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        look=FindObjectOfType<MouseLook>();
    }
    private void Update()
    {
        Inputs();
        UpdateJump();
        Crouch();
        //MouseMove();

    }
    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        PlayerMovement();
        FixedJump();
    }
    private void Inputs()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        isMoving = moveX != 0 || moveZ != 0;

    }
    private void PlayerMovement()
    {
        //if (!isGrounded) return;
        Vector3 moveVector = new Vector3(moveX * moveSpeed * Time.fixedDeltaTime, rigidbody.velocity.y, moveZ * moveSpeed * Time.fixedDeltaTime);
        moveVector = transform.TransformDirection(moveVector);
        rigidbody.velocity = moveVector;

    }
    private void UpdateJump()
    {
        if (isCrouching) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
                isGrounded = false;
            }

        }
    }
    private void FixedJump()
    {
        if (!isGrounded) return;
        if (isCrouching) return;

        if (rigidbody.velocity.y < 0)
        {
            rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rigidbody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rigidbody.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }


        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);
    }
    private void Crouch()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isCrouching)
            {
                transform.localScale = new Vector3(.2f, .1f, .2f);
                look.offset=new Vector3(0,0.2f,0);
                isCrouching = true;
            }

            else
            {
                transform.localScale = new Vector3(.2f, .2f, .2f);
                look.offset=new Vector3(0,0.4f,0);
                isCrouching = false;
            }
        }
    }
}
