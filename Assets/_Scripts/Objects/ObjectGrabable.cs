using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;
    //private Socket socket;
    public bool insideSocket;
    public float moveSpeed;
    public bool snapped;
    //public float socketspeed;

    
    private void Awake()
    {
        objectRigidbody = GetComponent<Rigidbody>();
    }
    public void Grab(Transform objectGrabPointTransform)
    {
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
        this.objectGrabPointTransform = null;
        if (snapped)
        {
            objectRigidbody.velocity = Vector3.zero;
        }
        objectRigidbody.useGravity = true;
    }
    //public void LockOnSocket(Socket _socket)
    //{
    //    socket = _socket;
    //    insideSocket = true;
    //    this.objectGrabPointTransform = null;
    //    objectRigidbody.isKinematic = true;
    //}
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

            objectRigidbody.velocity = DirectionToPoint.normalized * moveSpeed * DistanceToPoint;
        }
        if (insideSocket)
        {
            //transform.position = Vector3.Lerp(transform.position, socket.transform.position, socketspeed);
            //transform.rotation = Quaternion.Lerp(transform.rotation, socket.transform.rotation, socketspeed);
        }

    }
}
