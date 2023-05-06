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
    public float[] platformDelays;
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
        //if (active)
        //{
        //    for (int i = 0; i < platformPos.Count; i++)
        //    {
        //        float delay = i < platformDelays.Length ? platformDelays[i] : 0f;
        //        platformPos[i].DOMove(platformTargetVector[i], moveTime).SetDelay(delay);
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < platformPos.Count; i++)
        //    {
        //        platformPos[i].DOMove(platformStartVector[i], moveTime);
        //    }
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ObjectGrabable>(out var key))
        {
            if (!active)
            {
                active = true;
                key.LockOnSocket(this);
                key.Drop();
                for (int i = 0; i < platformPos.Count; i++)
                {
                    float delay = i < platformDelays.Length ? platformDelays[i] : 0f;
                    platformPos[i].DOMove(platformTargetVector[i], moveTime).SetDelay(delay);
                }
            }
        }
    }
    public void DeActivate()
    {
        for (int i = 0; i < platformPos.Count; i++)
        {
            float delay = i < platformDelays.Length ? platformDelays[i] : 0f;
            platformPos[i].DOMove(platformStartVector[i], moveTime).SetDelay(delay);
        }
        StartCoroutine(Delay());

    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("exit");
        active = false;
    }
}
