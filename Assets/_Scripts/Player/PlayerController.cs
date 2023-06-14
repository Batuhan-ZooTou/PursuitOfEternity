using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float moveX, moveZ;
    private Rigidbody rigidbody;
    [HideInInspector] public Transform checkPoint;
    [SerializeField] public float moveSpeed;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool onGoo;
    public bool isCrouching = false;
    public bool isMoving;
    MouseLook look;
    public Interactor interactor;
    private Vector2 move;
    RaycastHit[] groundHits = new RaycastHit[2];
    public ForceMode force;
    bool onJumpPad;

    [SerializeField] float airTime;

    Texture2D tex;
    Texture text;
    RenderTexture rText;
    Color maskColor;
    private bool readyToJump = true;
    [SerializeField] private float jumpCooldown = 0.2f;
    [SerializeField] private float bounceMultiplier;
    [SerializeField] private float decreaseBouncePercentage;
    [SerializeField] private float thresholdYVelocity;
    private bool isStuck;
    private bool isJumping;

    public PlayerController Instance;
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
        UpdateJump();
        Crouch();
        GetYVelocityMultiplier();

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
                    int hitCount = Physics.RaycastNonAlloc(groundCheck.position, Vector3.down, groundHits, groundDistance, groundMask);
                    if (hitCount >= 2)
                    {
                        isGrounded = true;
                        onJumpPad = false;

                    }
                    else
                    {
                        isGrounded = false;
                    }
                }
                else
                {
                    isGrounded = true;
                    onJumpPad = false;

                }
            }
            else
            {
                onJumpPad = false;
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
            if (hit.collider.GetComponent<Paintable>() != null)
            {
                rText = null;
                rText = hit.collider.GetComponent<Paintable>().getMask();
                tex = rText.toTexture2D();
                text = hit.collider.GetComponent<MeshRenderer>().material.GetTexture("Texture2D_41271c3c5f484ca2a435c65087a81705");
                maskColor = tex.GetPixel(Mathf.FloorToInt(hit.textureCoord.x * text.width), Mathf.FloorToInt(hit.textureCoord.y * text.height));
                if (maskColor.r != 0)
                {
                    onGoo = true;
                    if (airTime < 0.15f)
                    {
                        return;
                    }
                    rigidbody.AddForce(hit.normal * bounceMultiplier * airTime, force);

                    Debug.Log("Red");
                }
                else if (maskColor.g != 0)
                {
                    onGoo = true;
                    Debug.Log("green");
                }
                else if (maskColor.b != 0)
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Paintable>(out var paintable))
        {
            if (paintable != null)
            {
                if (Physics.Raycast(transform.position, (collision.contacts[0].point - transform.position).normalized, out RaycastHit raycastHit, transform.localScale.x))
                {
                    rText = null;
                    rText = paintable.getMask();
                    tex = rText.toTexture2D();
                    text = paintable.GetComponent<MeshRenderer>().material.GetTexture("Texture2D_41271c3c5f484ca2a435c65087a81705");
                    maskColor = tex.GetPixel(Mathf.FloorToInt(raycastHit.textureCoord.x * text.width), Mathf.FloorToInt(raycastHit.textureCoord.y * text.height));
                    if (maskColor.r != 0)
                    {
                        //rigidbody.AddForce(raycastHit.normal * bounceMultiplier * airTime, force);

                        Debug.Log("TouchRed");
                    }
                    else if (maskColor.g != 0 && !isJumping)
                    {
                        transform.SetParent(collision.transform.parent);
                        rigidbody.constraints = RigidbodyConstraints.FreezePosition;
                        isStuck = true;
                        Debug.Log("Touchgreen");
                    }
                    else if (maskColor.b != 0)
                    {
                        Debug.Log("Touchblue");
                    }
                    else
                    {
                        Debug.Log("Touchnothing");
                    }
                }
            }

        }
    }
    public void GetYVelocityMultiplier()
    {
        if (!isGrounded && rigidbody.velocity.y < 0)
        {
            airTime += Time.deltaTime;
            airTime = Mathf.Clamp(airTime, 0, 2f);
        }
        else if (isGrounded)
        {
            airTime = 0;
        }
        if (isStuck)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.SetParent(null);
                rigidbody.constraints = RigidbodyConstraints.None;
                rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                isJumping = true;
                isStuck = false;
                rigidbody.isKinematic = false;
            }
        }
        else
        {
            isJumping = false;
        }
    }
    private void FixedUpdate()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        PlayerMovement();
        //FixedJump();
        CheckForGoo();
        CheckForGround();
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
        if (onJumpPad)
        {
            return;
        }
        Vector3 moveVector = new Vector3(move.x * moveSpeed * Time.fixedDeltaTime, rigidbody.velocity.y, move.y * moveSpeed * Time.fixedDeltaTime);
        moveVector = transform.TransformDirection(moveVector);
        rigidbody.velocity = moveVector;

    }
    private void UpdateJump()
    {
        if (isCrouching) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded && readyToJump)
            {
                readyToJump = false;
                Invoke(nameof(ResetJump), jumpCooldown);
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
                isGrounded = false;
            }

        }
    }
    private void ResetJump()
    {
        readyToJump = true;
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out JumpPad pad))
        {
            onJumpPad = true;
        }
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundDistance, Color.green);
    }
}
