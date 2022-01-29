using UnityEngine;

public class EntityFactory : MonoBehaviour
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

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void MakePlayer()
    {
        Instantiate(playerPrefab, GetRandomPointInRadius(playerSpawnPoint.transform.position, playerSpawnRadius), Quaternion.identity);
    }

    public void MakeMonster()
    {
        Instantiate(monsterPrefab, GetRandomPointInRadius(monsterSpawnPoint.transform.position, monsterSpawnRadius), Quaternion.identity);
    }

    public void MakeWorker()
    {
        Instantiate(workerPrefab, GetRandomPointInRadius(workerSpawnPoint.transform.position, workerSpawnRadius), Quaternion.identity);
    }

    public void MakeResource()
    {
        Instantiate(resourcePrefab, GetRandomPointInRadius(resourceSpawnPoint.transform.position, resourceSpawnRadius), Quaternion.identity);
    }

    private Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        var x = Random.Range(center.x - radius, center.x + radius);
        var y = center.y;
        var z = Random.Range(center.z - radius, center.z + radius);
        return new Vector3(x, y, z);
    }
}