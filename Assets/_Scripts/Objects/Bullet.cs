using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float knockbackForce;


    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetDirection = (target.position - transform.position).normalized;
        transform.Translate(transform.forward * bulletSpeed * Time.deltaTime, Space.World);

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.AddForce(Vector3.forward * knockbackForce);

            }
        }
        Destroy(gameObject, 2f);
    }
}
