using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityFactory : MonoBehaviour, IObserver<EntityEvent>
{
    public GameObject playerPrefab;
    public GameObject playerSpawnPoint;
    public float playerSpawnRadius;

    public GameObject workerPrefab;
    public GameObject workerSpawnPoint;
    public float workerSpawnRadius;

    public GameObject monsterPrefab;
    public GameObject monsterSpawnPoint;
    public float monsterSpawnRadius;

    public GameObject resourcePrefab;
    public GameObject resourceSpawnPoint;
    public float resourceSpawnRadius;


    public int maxPlayers = 10;
    public int maxWorkers = 10;
    public int maxMonsters = 10;
    public int maxResources = 10;
    
    [Header("For debugging only, don't edit these!")]
    [SerializeField]
    private int _monsterCount = 0;
    [SerializeField]
    private int _playerCount = 0;
    [SerializeField]
    private int _resourceCount = 0;
    [SerializeField]
    private int _workerCount = 0;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(EntityEvent entityEvent)
    {
        if (entityEvent.EventType != EntityEventType.Death) return;

        switch (entityEvent.Occupation)
        {
            case Occupation.Player:
                _playerCount -= 1;
                break;
            case Occupation.Monster:
                _monsterCount -= 1;
                break;
            case Occupation.Worker:
                _workerCount -= 1;
                break;
            case Occupation.Resource:
                _resourceCount -= 1;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void MakePlayer()
    {
        if (_playerCount >= maxPlayers) return;
        _playerCount += 1;
        InstantiateEntity(playerPrefab, playerSpawnPoint.transform.position, playerSpawnRadius);
    }

    public void MakeMonster()
    {
        if (_monsterCount >= maxMonsters) return;
        _monsterCount += 1;
        InstantiateEntity(monsterPrefab, monsterSpawnPoint.transform.position, monsterSpawnRadius);
    }

    public void MakeWorker()
    {
        if (_workerCount >= maxWorkers) return;
        _workerCount += 1;
        InstantiateEntity(workerPrefab, workerSpawnPoint.transform.position, workerSpawnRadius);
    }

    public void MakeResource()
    {
        if (_resourceCount >= maxResources) return;
        _resourceCount += 1;
        InstantiateEntity(resourcePrefab, resourceSpawnPoint.transform.position, resourceSpawnRadius);
    }

    public Entity InstantiateEntity(GameObject prefab, Vector3 spawnPoint, float spawnRadius)
    {
        var instance = Instantiate(prefab, GetRandomPointInRadius(spawnPoint, spawnRadius), Quaternion.identity);
        var entity = instance.GetComponent<Entity>();
        entity.Subscribe(this);
        return entity;
    }

    private Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        var x = Random.Range(center.x - radius, center.x + radius);
        var y = center.y;
        var z = Random.Range(center.z - radius, center.z + radius);
        return new Vector3(x, y, z);
    }
}
