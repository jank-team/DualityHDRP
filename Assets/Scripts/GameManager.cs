using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Town { get; private set; }

    public GameObject WeaponSmith{get; private set; }

    public GameObject ArmorSmith{get; private set;}

    private void Awake()
    {
        Instance = this;

        Town = GameObject.FindWithTag("Town");

        WeaponSmith = GameObject.FindWithTag("WeaponSmith");

        ArmorSmith = GameObject.FindWithTag("ArmorSmith");
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}