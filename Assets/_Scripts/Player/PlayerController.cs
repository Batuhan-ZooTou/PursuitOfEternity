using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private bool onGoo;
    public bool isCrouching = false;
    private bool isMoving;
    MouseLook look;
    public Interactor interactor;
    private Vector2 move; 
    RaycastHit[] groundHits = new RaycastHit[2];


    Texture2D tex;
    Texture text;
    RenderTexture rText;
    Color maskColor;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        look = FindObjectOfType<MouseLook>();
         tex = new Texture2D(1024, 1024, TextureFormat.RGB24, false);  
    }
    private void Update()
    {
        Inputs();
        CheckForGround();
        UpdateJump();
        Crouch();
    }

    private void CheckForGround()
    {
        if (Physics.Raycast(groundCheck.position, Vector3.down, out RaycastHit hit, groundDistance, groundMask))
        {
            ObjectGrabable objectUnder;
            if (hit.transform.TryGetComponent(out objectUnder))
            {
                if (objectUnder == interactor.grabbedObject)
                {
                    int hitCount = Physics.RaycastNonAlloc(groundCheck.position, Vector3.down, groundHits,groundDistance, groundMask);
                    if (hitCount >= 2)
                    {
                        isGrounded = true;
        
                    }
                    else
                    {
                        isGrounded = false;
                    }
                }
                else
                {
                    isGrounded = true;
        
                }
            }
            else
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }
    }   
    public void CheckForGoo()
    {
        if (Physics.Raycast(groundCheck.position, Vector3.down, out RaycastHit hit, groundDistance, groundMask))
        {
            if (hit.collider.GetComponent<Paintable>()!=null)
            {
                rText = null;
                rText = hit.collider.GetComponent<Paintable>().getMask();
                tex = rText.toTexture2D();
                text = hit.collider.GetComponent<MeshRenderer>().material.GetTexture("Texture2D_41271c3c5f484ca2a435c65087a81705");
                maskColor = tex.GetPixel(Mathf.FloorToInt(hit.textureCoord.x * text.width), Mathf.FloorToInt(hit.textureCoord.y * text.height));
                if (maskColor.r!=0)
                {
                    onGoo = true;
                    Debug.Log("Red");
                }
                else if (maskColor.g!=0)
                {
                    onGoo = true;
                    Debug.Log("green");
                }
                else if(maskColor.b!=0)
                {
                    onGoo = true;
                    Debug.Log("blue");
                }
                else
                {
                    onGoo = false;
                    Debug.Log("nothing");
                }
            }
        }
        else
        {
            onGoo = false;

        }
    }
    private void FixedUpdate()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        PlayerMovement();
        //FixedJump();
        CheckForGoo();
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
        Vector3 moveVector = new Vector3(move.x * moveSpeed * Time.fixedDeltaTime, rigidbody.velocity.y, move.y * moveSpeed * Time.fixedDeltaTime);
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


        //isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);
    }
    private void Crouch()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isCrouching)
            {
                transform.localScale = new Vector3(.2f, .1f, .2f);
                look.offset = new Vector3(0, 0.2f, 0);
                isCrouching = true;
            }

            else
            {
                transform.localScale = new Vector3(.2f, .2f, .2f);
                look.offset = new Vector3(0, 0.4f, 0);
                isCrouching = false;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down*groundDistance, Color.green);
    }
}
