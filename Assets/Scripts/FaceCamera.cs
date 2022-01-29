using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FaceCamera : MonoBehaviour
{
    private Transform _cameraTransform;
    
    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_cameraTransform);
    }

    private void OnDrawGizmos()
    {
        var direction = (Camera.main.transform.position - transform.position).normalized;
        Gizmos.DrawRay(transform.position, direction * 100);
    }
}
