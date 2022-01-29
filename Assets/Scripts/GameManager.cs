using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Town { get; private set; }
    public EntityFactory EntityFactory { get; private set; }

    public int dayLength = 600;
    public int nightLength = 300;
    public int currentTime = 0;

    public GameObject WeaponSmith{get; private set; }

    public GameObject ArmorSmith{get; private set;}

    private void Awake()
    {
        Instance = this;

        Town = GameObject.FindWithTag("Town");

        WeaponSmith = GameObject.FindWithTag("WeaponSmith");

        ArmorSmith = GameObject.FindWithTag("ArmorSmith");
        
        EntityFactory = GetComponent<EntityFactory>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating(nameof(TimedUpdate), 0, 0.1f);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void TimedUpdate()
    {
        if (currentTime == dayLength + nightLength)
        {
            currentTime = 0;
        }
        else if (currentTime == 0)
        {
            StartDay();
        }
        else if (currentTime == dayLength)
        {
            StartNight();
        }

        EntityFactory.MakePlayer();
        EntityFactory.MakeMonster();
        EntityFactory.MakeResource();
        EntityFactory.MakeWorker();

        currentTime++;
    }

    private void StartDay()
    {
        // @todo change light tone
    }

    private void StartNight()
    {
        // @todo change light tone
    }
}