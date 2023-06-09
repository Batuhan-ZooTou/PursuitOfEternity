using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Socket : MonoBehaviour
{
    public List<Transform> platformPos = new List<Transform>();
    public List<Transform> platformTargetPos = new List<Transform>();
    private List<Vector3> platformTargetVector = new List<Vector3>();
    private List<Vector3> platformStartVector = new List<Vector3>();
    public float moveTime;
    public bool active;
    private void Start()
    {
        for (int i = 0; i < platformPos.Count; i++)
        {
            platformTargetVector.Add(platformTargetPos[i].position);
            platformStartVector.Add(platformPos[i].position);
        }
    }
    private void FixedUpdate()
    {
        if (active)
        {
            for (int i = 0; i < platformPos.Count; i++)
            {
                platformPos[i].DOMove(platformTargetVector[i], moveTime);
            }
        }
        if (!active)
        {
            for (int i = 0; i < platformPos.Count; i++)
            {
                platformPos[i].DOMove(platformStartVector[i], moveTime);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ObjectGrabable>(out var key))
        {
            if (!active)
            {
                key.LockOnSocket(this);
                active = true;
                Debug.Log("inside");
                //move to target position
                
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ObjectGrabable>(out var key))
        {
            if (active)
            {
                Debug.Log("outside");
                //move to target position
                StartCoroutine(Delay());

            }
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("exit");
        active = false;

    }
}
