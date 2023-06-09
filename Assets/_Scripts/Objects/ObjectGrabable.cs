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
            //socket.Close();

        }
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;

    }
    public void Drop()
    {
        //Physics.IgnoreLayerCollision(3, 6, false);
        player.grabbedObject = null;
        player = null;
        this.objectGrabPointTransform = null;
        if (snapped)
        {
            objectRigidbody.velocity = Vector3.zero;
        }
        objectRigidbody.useGravity = true;
    }
    public void LockOnSocket(Socket _socket)
    {
        objectRigidbody.velocity = Vector3.zero;
        socket = _socket;
        insideSocket = true;
        this.objectGrabPointTransform = null;
        objectRigidbody.isKinematic = true;
    }
    public void ThrowObject(Transform playerCameraTransform, float throwForce)
    {
        objectRigidbody.AddForce(playerCameraTransform.forward * throwForce);
    }
    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            // Simple
            // float lerpSpeed=50f;
            // Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            // objectRigidbody.MovePosition(newPosition);

            //physic based
            Vector3 DirectionToPoint = objectGrabPointTransform.position - transform.position;
            float DistanceToPoint = DirectionToPoint.magnitude;

            objectRigidbody.velocity = DirectionToPoint.normalized * moveSpeed * DistanceToPoint*Time.fixedDeltaTime;
        }
        if (insideSocket)
        {
            transform.DOMove(socket.transform.position, socketspeed);
            transform.DORotate(socket.transform.eulerAngles, socketspeed);
        }

    }
}
