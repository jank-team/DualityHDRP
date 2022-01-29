using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayBalance : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = $"${Town.Instance.balance}";
    }
}
