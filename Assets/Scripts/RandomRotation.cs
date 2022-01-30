using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    void Awake()
    {
        transform.Rotate(Vector3.up * Random.Range(0f, 361f));
    }
}
