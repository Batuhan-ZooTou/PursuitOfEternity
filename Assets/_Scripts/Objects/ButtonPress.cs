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

    private void Start()
    {
        for (int i = 0; i < platformPos.Count; i++)
        {
            platformTargetVector.Add(platformTargetPos[i].position);
            platformStartVector.Add(platformPos[i].position);
        }
    }

    public void Pressed()
    {
        isActive = !isActive;
        if (isActive)
        {
            for (int i = 0; i < platformPos.Count; i++)
            {
                platformPos[i].DOMove(platformTargetVector[i], moveTime);
            }
        }
        else
        {
            for (int i = 0; i < platformPos.Count; i++)
            {
                platformPos[i].DOMove(platformStartVector[i], moveTime);
            }
        }
    }
}
