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
}
