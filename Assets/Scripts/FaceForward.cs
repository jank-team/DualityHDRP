using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceForward : MonoBehaviour
{
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + rb.velocity.normalized * 10);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.position + rb.velocity.normalized * 10);
    }
}
