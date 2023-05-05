using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour
{
    public GameObject door;
    public List<Transform> platformPos = new List<Transform>();
    public List<Transform> platformTargetPos = new List<Transform>();
    private List<Vector3> platformTargetVector = new List<Vector3>();
    private List<Vector3> platformStartVector = new List<Vector3>();
    public float moveTime;
    public bool active;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ObjectGrabable>(out var key))
        {
            if (!active)
            {
                key.LockOnSocket(this);
                door.GetComponent<Animator>().SetBool("Open", true);
                active = true;
            }
        }
    }
    public void Close()
    {
        door.GetComponent<Animator>().SetBool("Open", false);
        StartCoroutine(Delay());

    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("exit");
        active = false;

    }
}
