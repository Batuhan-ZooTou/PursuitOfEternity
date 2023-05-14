using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectGrabable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    private Socket socket;
    Interactor player;
    public bool insideSocket;
    public float moveSpeed;
    public bool snapped;
    public float socketspeed;
    private Vector3 spawnPosition;

    Texture2D tex;
    Texture text;
    RenderTexture rText;
    Color maskColor;

    [SerializeField] private float bounceMultiplier;
    [SerializeField] private float decreaseBouncePercentage;
    [SerializeField] private float thresholdVelocity;
    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
        spawnPosition = transform.position;
    }
    public void ResetPosition()
    {
        Debug.Log("tp");
        if (player!=null)
        {
            Drop();
        }
        objectRigidbody.velocity = Vector3.zero;
        transform.position = spawnPosition;
        transform.rotation = Quaternion.Euler(Vector3.zero);

    }
    public void Grab(Transform objectGrabPointTransform, Interactor _player)
    {
        player = _player;
        //Physics.IgnoreLayerCollision(3, 6, true);
        if (insideSocket)
        {
            insideSocket = false;
            objectRigidbody.isKinematic = false;
            socket.DeActivate();

        }
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;

    }
    public void Drop()
    {
        //Physics.IgnoreLayerCollision(3, 6, false);
        if (player!=null)
        {
            player.grabbedObject = null;
            Physics.IgnoreCollision(player.player, GetComponent<Collider>(), false);
            player = null;

        }
        this.objectGrabPointTransform = null;
        if (snapped)
        {
            objectRigidbody.velocity = Vector3.zero;
        }
        objectRigidbody.useGravity = true;
    }
    public void LockOnSocket(Socket _socket)
    {
        socket = _socket;
        insideSocket = true;
        this.objectGrabPointTransform = null;
        objectRigidbody.isKinematic = true;
        transform.DOMove(socket.holeTransform.position, socketspeed);
        transform.DORotate(socket.holeTransform.eulerAngles, socketspeed);
    }
    public void ThrowObject(Transform playerCameraTransform, float throwForce)
    {
        objectRigidbody.AddForce(playerCameraTransform.forward * throwForce);
    }
    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            if (Physics.Raycast(transform.position,(player.cam.transform.position-transform.position).normalized,0.4f,player.barrier))
            {
                Debug.DrawRay(transform.position, (player.cam.transform.position - transform.position).normalized*0.4f, Color.red);
                Drop();
                return;
            }
            
            // Simple
            // float lerpSpeed=50f;
            // Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            // objectRigidbody.MovePosition(newPosition);

            //physic based
            Vector3 DirectionToPoint = objectGrabPointTransform.position - transform.position;
            float DistanceToPoint = DirectionToPoint.magnitude;

            objectRigidbody.velocity = DirectionToPoint.normalized * moveSpeed * DistanceToPoint*Time.fixedDeltaTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Paintable>(out var paintable))
        {
            if (paintable!=null)
            {
                if (objectGrabPointTransform!=null)
                {
                    return;
                }
                Debug.DrawRay(transform.position, (collision.contacts[0].point - transform.position).normalized * transform.localScale.x, Color.red);
                if (Physics.Raycast(transform.position, (collision.contacts[0].point - transform.position).normalized, out RaycastHit raycastHit, transform.localScale.x))
                {
                    rText = null;
                    rText = paintable.getMask();
                    tex = rText.toTexture2D();
                    text = paintable.GetComponent<MeshRenderer>().material.GetTexture("Texture2D_41271c3c5f484ca2a435c65087a81705");
                    maskColor = tex.GetPixel(Mathf.FloorToInt(raycastHit.textureCoord.x * text.width), Mathf.FloorToInt(raycastHit.textureCoord.y * text.height));
                    if (maskColor.r != 0)
                    {
                        if (Mathf.Abs(objectRigidbody.velocity.magnitude) > thresholdVelocity)
                        {
                        Debug.Log(raycastHit.normal);
                            objectRigidbody.AddForce(raycastHit.normal* bounceMultiplier, ForceMode.VelocityChange);
                        }
                        Debug.Log("Red");
                    }
                    else if (maskColor.g != 0)
                    {
                        Debug.Log("green");
                    }
                    else if (maskColor.b != 0)
                    {
                        Debug.Log("blue");
                    }
                    else
                    {
                        Debug.Log("nothing");
                    }
                }
            }
            
        }
    }
}
