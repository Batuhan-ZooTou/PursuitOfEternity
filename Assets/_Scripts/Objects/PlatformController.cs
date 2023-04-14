
using UnityEngine;
using DG.Tweening;

public class PlatformController : MonoBehaviour
{

    public Transform platformPos;
    public Transform platformTargetPos;
    public Vector3 platformTargetVector;
    public float moveTime;

    private void Start()
    {
        platformTargetVector = platformTargetPos.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            platformPos.DOMove(platformTargetVector, moveTime);
        }
        if (other.gameObject.CompareTag("interactableCube"))
        {
            platformPos.DOMove(platformTargetVector, moveTime);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            platformPos.DOMove(platformTargetVector, moveTime);
        }
        if (other.gameObject.CompareTag("interactableCube"))
        {
            platformPos.DOMove(platformTargetVector, moveTime);
        }
    }


}
