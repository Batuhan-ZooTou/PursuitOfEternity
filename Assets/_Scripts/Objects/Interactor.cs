using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class Interactor : MonoBehaviour
{
    public Transform cam;
    [SerializeField] private Transform objectGrabPointTransform;
    public Collider player;
    [SerializeField] private GooGun gooGun;

    [SerializeField] private float grabPointSpeed;
    [SerializeField] private float interactDistance;
    [SerializeField] private float holdDistance;
    [SerializeField] private float throwForce;

    [SerializeField] private LayerMask interactables;
    [SerializeField] private Interactable Interactable;
    [SerializeField] private LayerMask grabables;
    [SerializeField] public ObjectGrabable grabbedObject;
    [SerializeField] private LayerMask solidLayerMask;
    [SerializeField] public LayerMask barrier;

    private Vector3 defaultGrabPoint;
    float mouseScrollY;
    Ray ray;
    RaycastHit hit;


    UnityEvent onInteract;
    UnityEvent onDisInteract;

    //Inputs
    #region 
    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        mouseScrollY = context.ReadValue<Vector2>().y;
        gooGun.SwitchType(mouseScrollY);
    }
    public void OnLMB(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (gooGun!=null && gooGun.gameObject.active)
            {
                gooGun.Shoot();
            }
            ////dropping with lmb
            //if (grabbedObject != null)
            //{
            //    Physics.IgnoreCollision(player, grabbedObject.GetComponent<Collider>(), false);
            //    grabbedObject.Drop();
            //    grabbedObject = null;
            //    //playerlayer.whatIsGround |= (1 << 3);
            //}
            //else if (Interactable != null)
            //{
            //    if (Physics.Raycast(ray, interactDistance, barrier))
            //    {
            //        return;
            //    }
            //    //if hits grabable object
            //    if (Physics.Raycast(ray, out hit, interactDistance, grabables))
            //    {
            //        hit.transform.TryGetComponent(out grabbedObject);
            //        Physics.IgnoreCollision(player, grabbedObject.GetComponent<Collider>(), true);
            //        objectGrabPointTransform.position = defaultGrabPoint;
            //        grabbedObject.Grab(objectGrabPointTransform,this);
            //        //playerlayer.whatIsGround &= ~(1 << 3);
            //    }
            //
            //}

        }
    }
    public void OnRMB(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //throwing with rmb
            if (grabbedObject != null)
            {
                grabbedObject.ThrowObject(cam, throwForce);
                Physics.IgnoreCollision(player, grabbedObject.GetComponent<Collider>(), false);
                grabbedObject.Drop();
                grabbedObject = null;
                //playerlayer.whatIsGround |= (1 << 3);
            }

        }
    }
    public void GetBrain(int brain)
    {
        GameManager.Instance.brains[brain - 1] = true;
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //dropping with lmb
            if (grabbedObject != null)
            {
                Physics.IgnoreCollision(player, grabbedObject.GetComponent<Collider>(), false);
                grabbedObject.Drop();
                grabbedObject = null;
                //playerlayer.whatIsGround |= (1 << 3);
            }
            else if (Interactable != null)
            {
                if (Physics.Raycast(ray, interactDistance, barrier))
                {
                    return;
                }
                //if hits grabable object
                if (Physics.Raycast(ray, out hit, interactDistance, grabables))
                {
                    hit.transform.TryGetComponent(out grabbedObject);
                    Physics.IgnoreCollision(player, grabbedObject.GetComponent<Collider>(), true);
                    objectGrabPointTransform.position = defaultGrabPoint;
                    grabbedObject.Grab(objectGrabPointTransform, this);
                    //playerlayer.whatIsGround &= ~(1 << 3);
                    return;
                }
                //if players hand is empty
                onInteract = Interactable.onInteract;
                onInteract.Invoke();
            }
            

        }
    }
    #endregion

    void Update()
    {
        AdjuctGrabPoint();
        CheckForInteractables();

        defaultGrabPoint = cam.position + cam.forward * holdDistance;
    }
    public void AdjuctGrabPoint()
    {
        //adjusting the distance wiht mouse wheel
        if (grabbedObject != null)
        {
            //if (mouseScrollY > 0f && Vector3.Distance(objectGrabPointTransform.position, cam.position) < interactDistance) // forward
            //{
            //    objectGrabPointTransform.position += cam.forward * Time.deltaTime * grabPointSpeed;
            //}
            //else if (mouseScrollY < 0f && Vector3.Distance(objectGrabPointTransform.position, cam.position) > holdDistance) // backwards
            //{
            //    objectGrabPointTransform.position -= cam.forward * Time.deltaTime * grabPointSpeed;
            //}
            //adjusting the point while near walls
            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit raycastHit, Vector3.Distance(cam.position, objectGrabPointTransform.position), solidLayerMask))
            {
                grabbedObject.snapped = true;
                objectGrabPointTransform.position = raycastHit.point + (cam.forward * -0.1f);

            }
            else if (grabbedObject.snapped)
            {
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit raycastHit1, holdDistance, solidLayerMask))
                {
                    objectGrabPointTransform.position = raycastHit1.point + (cam.forward * -0.1f);
                }
                else
                {
                    grabbedObject.snapped = false;
                    objectGrabPointTransform.position = defaultGrabPoint;
                }
            }
        }
    }
    void CheckForInteractables()
    {
        if (grabbedObject != null)
        {
            Interactable pastInteractable = Interactable.GetComponent<Interactable>();
            Interactable = grabbedObject.GetComponent<Interactable>();
            if (pastInteractable != Interactable)
            {
                pastInteractable.transform.GetComponent<MeshRenderer>().material.SetFloat("_Scale", 0f);
            }
            Interactable.transform.GetComponent<MeshRenderer>().material.SetFloat("_Scale", 0f);
        }
        else
        {
            ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out hit, interactDistance, interactables))
            {
                Interactable pastInteractable;
                //There is Interactable object
                if (Interactable != null)
                {
                    //check if 2 object is not highlighted at the same time
                    pastInteractable = Interactable;
                    hit.transform.TryGetComponent(out Interactable);
                    if (pastInteractable != Interactable)
                    {
                        pastInteractable.transform.GetComponent<MeshRenderer>().material.SetFloat("_Scale", 0f);
                        Interactable.transform.GetComponent<MeshRenderer>().material.SetFloat("_Scale", 1.03f);
                    }
                    else if (pastInteractable == Interactable)
                    {
                        Interactable.transform.GetComponent<MeshRenderer>().material.SetFloat("_Scale", 1.03f);
                    }
                    return;
                }
                hit.transform.TryGetComponent(out Interactable);
                //Highlight it
                Interactable.transform.GetComponent<MeshRenderer>().material.SetFloat("_Scale", 1.03f);
                //UI

            }
            else if (Interactable != null)
            {
                Interactable.transform.GetComponent<MeshRenderer>().material.SetFloat("_Scale", 0f);
                Interactable = null;
            }
        }
    }
    private void OnDrawGizmos()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(objectGrabPointTransform.position, 0.05f);
    }
}
