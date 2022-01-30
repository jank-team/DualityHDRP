using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class AttachRandomObject : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();
    public Vector3 position =  Vector3.zero;
    public Vector3 scale =  Vector3.one;

    private void Awake()
    {
        if (prefabs.Count == 0) return; 
        var prefab = prefabs[Random.Range(0, prefabs.Count)];
        var instance = Instantiate(prefab, transform.position, Quaternion.identity, transform);
        instance.transform.localPosition = position;
        instance.transform.localScale = scale;
    }
}
