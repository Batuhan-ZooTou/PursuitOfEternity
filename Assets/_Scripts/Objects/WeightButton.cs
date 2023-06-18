using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class WeightButton : MonoBehaviour
{
    public List<Transform> platformPos = new List<Transform>();
    public List<Transform> platformTargetPos = new List<Transform>();
    private List<Vector3> platformTargetVector = new List<Vector3>();
    private List<Vector3> platformStartVector = new List<Vector3>();
    public float moveTime;
    public bool isActive = false;
    public UnityEvent OnPressed;
    public bool isDouble;
    public WeightButton twin;
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
            if (isDouble)
            {
                if (twin.isActive)
                {
                    OnPressed.Invoke();
                    isActive = true;
                    if (platformPos == null)
                    {
                        return;
                    }
                    //move to target position
                    if (isActive)
                    {
                        for (int i = 0; i < platformPos.Count; i++)
                        {
                            platformPos[i].DOMove(platformTargetVector[i], moveTime);
                        }
                    }
                }
                return;
            }
            OnPressed.Invoke();
            if (platformPos == null)
            {
                return;
            }
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
            OnPressed.Invoke();
            isActive = false;
            if (platformPos == null)
            {
                return;
            }
            //back to start position
            if (!isActive)
            {
                for (int i = 0; i < platformPos.Count; i++)
                {
                    platformPos[i].DOMove(platformStartVector[i], moveTime);
                }
            }
        }
    }
}
