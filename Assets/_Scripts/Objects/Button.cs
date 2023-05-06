using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Button : MonoBehaviour
{
    public List<Transform> platformPos = new List<Transform>();
    public List<Transform> platformTargetPos = new List<Transform>();
    public Transform pressedPos;
    private Vector3 nonPressedPos;
    private List<Vector3> platformTargetVector = new List<Vector3>();
    private List<Vector3> platformStartVector = new List<Vector3>();
    public float moveTime;
    public bool isActive=false;
    public float[] platformCloseDelays;
    public float[] platformStartDelays;

    private void Start()
    {
        for (int i = 0; i < platformPos.Count; i++)
        {
            platformTargetVector.Add(platformTargetPos[i].position);
            platformStartVector.Add(platformPos[i].position);
        }
        nonPressedPos = transform.position;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            isActive = !isActive;
            if (isActive)
            {
                for (int i = 0; i < platformPos.Count; i++)
                {
                    float delay = i < platformStartDelays.Length ? platformStartDelays[i] : 0f;
                    platformPos[i].DOMove(platformTargetVector[i], moveTime).SetDelay(delay);
                }
                transform.DOMove(pressedPos.position, 0.5f);
            }
            else
            {
                for (int i = 0; i < platformPos.Count; i++)
                {
                    float delay = i < platformCloseDelays.Length ? platformCloseDelays[i] : 0f;
                    platformPos[i].DOMove(platformStartVector[i], moveTime).SetDelay(delay);
                }
                transform.DOMove(nonPressedPos, 0.5f);
            }
        }
    }
}
