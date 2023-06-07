using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrePaintGoo : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = dir;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
