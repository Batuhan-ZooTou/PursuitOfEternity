using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ObjectGrabable>(out var objectGrabbable))
        {
            objectGrabbable.ResetPosition();
        }
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            Debug.Log("Saved Progression");
            player.checkPoint = transform;
        }
    }
}
