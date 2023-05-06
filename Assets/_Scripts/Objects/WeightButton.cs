using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeightButton : MonoBehaviour
{
    public List<Transform> platformPos = new List<Transform>();
    public List<Transform> platformTargetPos = new List<Transform>();
    private List<Vector3> platformTargetVector = new List<Vector3>();
    private List<Vector3> platformStartVector = new List<Vector3>();
    public float moveTime;
    public float[] platformDelays;
    public bool isActive = false;

    private void Start()
    {
        for (int i = 0; i < platformPos.Count; i++)
        {
            platformTargetVector.Add(platformTargetPos[i].position);
            platformStartVector.Add(platformPos[i].position);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WeightButton"))
        {
            isActive = true;
            //move to target position
            if (isActive)
            {
                for (int i = 0; i < platformPos.Count; i++)
                {
                    platformPos[i].DOMove(platformTargetVector[i], moveTime);
                }
            }
            //back to start position

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("WeightButton"))
        {
            isActive = false;
            //back to start position
            if (!isActive)
            {
                for (int i = 0; i < platformPos.Count; i++)
                {
                    float delay = i < platformDelays.Length ? platformDelays[i] : 0f;
                    platformPos[i].DOMove(platformStartVector[i], moveTime).SetDelay(delay);
                }
            }
        }
    }

}