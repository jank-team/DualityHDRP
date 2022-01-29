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
        transform.LookAt(_cameraTransform);
    }

    void FixedUpdate()
    {
        transform.LookAt(_cameraTransform);
    }
}
