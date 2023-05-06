using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonPress : MonoBehaviour
{
    public List<Transform> platformPos = new List<Transform>();
    public List<Transform> platformTargetPos = new List<Transform>();
    private List<Vector3> platformTargetVector = new List<Vector3>();
    private List<Vector3> platformStartVector = new List<Vector3>();
    public float moveTime;
    public bool isActive = false;
    public float[] platformCloseDelays;
    public float[] platformStartDelays;
    public float activeTime;
    private float activeCounter;
    public bool hasTimer;
    private void Start()
    {
        activeCounter = activeTime;
        for (int i = 0; i < platformPos.Count; i++)
        {
            platformTargetVector.Add(platformTargetPos[i].position);
            platformStartVector.Add(platformPos[i].position);
        }
    }
    private void Update()
    {
        if (hasTimer)
        {
            if (isActive)
            {
                activeCounter -= Time.deltaTime;
                if (activeCounter <= 0)
                {
                    isActive = false;
                    activeCounter = activeTime;
                    for (int i = 0; i < platformPos.Count; i++)
                    {
                        float delay = i < platformCloseDelays.Length ? platformCloseDelays[i] : 0f;
                        platformPos[i].DOMove(platformStartVector[i], moveTime).SetDelay(delay);
                    }
                }
            }
        }
    }
    public void Pressed()
    {
        if (!hasTimer)
        {
            isActive = !isActive;
            if (isActive)
            {
                for (int i = 0; i < platformPos.Count; i++)
                {
                    float delay = i < platformStartDelays.Length ? platformStartDelays[i] : 0f;
                    platformPos[i].DOMove(platformTargetVector[i], moveTime).SetDelay(delay);
                }
            }
            else
            {
                for (int i = 0; i < platformPos.Count; i++)
                {
                    float delay = i < platformCloseDelays.Length ? platformCloseDelays[i] : 0f;
                    platformPos[i].DOMove(platformStartVector[i], moveTime).SetDelay(delay);
                }
            }
        }
        else
        {
            if (!isActive)
            {
                isActive = !isActive;
                for (int i = 0; i < platformPos.Count; i++)
                {
                    float delay = i < platformStartDelays.Length ? platformStartDelays[i] : 0f;
                    platformPos[i].DOMove(platformTargetVector[i], moveTime).SetDelay(delay);
                }
            }
        }
        
    }
}
