using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask SolidLayer;
    [SerializeField] private float projectileSpeed;
    private Rigidbody rb;
    public Color paintColor;

    public float radius = 0.5f;
    public float strength = 1;
    public float hardness = 1;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        
    }
    public void Spawn(GooGun gun)
    {
        rb.AddForce(gun.GooSpawnPoint.forward*projectileSpeed,ForceMode.VelocityChange);
        switch (gun.GooType)
        {
            case GooGunMode.Bouncy:
                paintColor = Color.red;
                GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case GooGunMode.Sticky:
                paintColor = Color.green;
                GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            case GooGunMode.Slipery:
                paintColor = Color.blue;
                GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
            default:
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Paintable p = collision.collider.GetComponent<Paintable>();
        if (p != null)
        {
            Vector3 pos = collision.contacts[0].point;
            PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);
        }
        Destroy(this.gameObject);
    }
}
