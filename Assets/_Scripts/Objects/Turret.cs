using UnityEngine;
using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour
{
    public Transform player;
    public float maxRange = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;
    private bool isFiring = false;
    private GameObject activeBullet;
    public Transform body;
    public Vector3 startRot;
    public Vector3 endRot;
    private void Start()
    {
        startRot = new Vector3(body.rotation.x, body.rotation.y, body.rotation.z);
        body.DORotate(endRot,3).SetLoops(-1,LoopType.Yoyo);
    }
    private void Update()
    {
        
        Vector3 turretPosition = transform.position;
        Vector3 playerPosition = player.position;

        float distance = Vector3.Distance(turretPosition, playerPosition);
        if (distance <= maxRange && !isFiring)
        {
            if (activeBullet == null)
            {
                Fire();
            }
            else
            {

                Vector3 targetDirection = (playerPosition - activeBullet.transform.position).normalized;
                activeBullet.GetComponent<Rigidbody>().velocity = targetDirection * bulletSpeed;
            }
        }
    }
    private void Fire()
    {
        isFiring = true;
        activeBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        activeBullet.GetComponent<Bullet>().SetTarget(player);
        Destroy(activeBullet, maxRange / bulletSpeed);
        Invoke("ResetFiring", 1.5f);
    }
    private void ResetFiring()
    {
        isFiring = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxRange);
    }
}