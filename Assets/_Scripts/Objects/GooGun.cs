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
    [SerializeField] private GameObject gooVisual;
    [SerializeField] private GameObject gooVisual2;
    private bool canSwitch=true;


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
    public void SwitchType(float y)
    {
        if (!canSwitch)
        {
            return;
        }
        canSwitch = false;
        Invoke(nameof(ResetCanSwitch), 0.1f);
        switch (GooType)
        {
            case GooGunMode.Bouncy:
                if (!GameManager.Instance.GooTypes[0])
                {
                    return;
                }
                else if(GameManager.Instance.GooTypes[1])
                {
                    if (y > 0)
                    {
                        GooType = GooGunMode.Sticky;
                        gooVisual.GetComponent<MeshRenderer>().material.color = Color.green;
                        gooVisual2.GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    else
                    {
                        GooType = GooGunMode.Slipery;
                        gooVisual.GetComponent<MeshRenderer>().material.color = Color.blue;
                        gooVisual2.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }
                }
                else
                {
                    GooType = GooGunMode.Sticky;
                    gooVisual.GetComponent<MeshRenderer>().material.color = Color.green;
                    gooVisual2.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                
                break;
            case GooGunMode.Sticky:
                if (!GameManager.Instance.GooTypes[1])
                {
                    GooType = GooGunMode.Bouncy;
                    gooVisual.GetComponent<MeshRenderer>().material.color = Color.red;
                    gooVisual2.GetComponent<MeshRenderer>().material.color = Color.red;
                    return;
                }
                if (y > 0)
                {
                    GooType = GooGunMode.Slipery;
                    gooVisual.GetComponent<MeshRenderer>().material.color = Color.blue;
                    gooVisual2.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                else
                {
                    GooType = GooGunMode.Bouncy;
                    gooVisual.GetComponent<MeshRenderer>().material.color = Color.red;
                    gooVisual2.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                break;
            case GooGunMode.Slipery:
                if (y > 0)
                {
                    GooType = GooGunMode.Bouncy;
                    gooVisual.GetComponent<MeshRenderer>().material.color = Color.red;
                    gooVisual2.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    GooType = GooGunMode.Sticky;
                    gooVisual.GetComponent<MeshRenderer>().material.color = Color.green;
                    gooVisual2.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                break;
            default:
                break;
        }
    }
    void ResetCanShoot()
    {
        CanShoot = true;
    }
    void ResetCanSwitch()
    {
        canSwitch = true;
    }
}
