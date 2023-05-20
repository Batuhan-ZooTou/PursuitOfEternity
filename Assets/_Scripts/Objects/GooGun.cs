using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GooGunMode
{
    Bouncy,
    Sticky,
    Slipery,
}
public class GooGun : MonoBehaviour
{
    public GooGunMode GooType;
    public Transform GooSpawnPoint;
    [SerializeField] private float shootDelay;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private LayerMask SolidLayer;
    [SerializeField] private bool CanShoot;
    [SerializeField] private float ShootCooldown;
    [SerializeField] private GameObject ProjectilePrefab;

    private void OnEnable()
    {
        CanShoot = true;
    }
    private void OnDisable()
    {
        CanShoot = false;

    }
    public void Shoot()
    {
        if (CanShoot)
        {
            CanShoot = false;
            Invoke(nameof(ResetCanShoot), ShootCooldown);
            GameObject goo=Instantiate(ProjectilePrefab, GooSpawnPoint.position, GooSpawnPoint.rotation, null);
            goo.GetComponent<GooProjectile>().Spawn(this);
        }
    }
    void ResetCanShoot()
    {
        CanShoot = true;
    }
}
