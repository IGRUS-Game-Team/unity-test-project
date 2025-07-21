using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    public GameObject[] npcPrefabs;

    public float minDelay = 1f;
    public float maxDelay = 3f;

    float nextSpawnTime;

    void Start()
    {
        NextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnNPC();
            NextSpawn();
        }
    }
    void SpawnNPC()
    {
        if (npcPrefabs.Length == 0) return;

        GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }

    void NextSpawn()
    {
        float delay = Random.Range(minDelay, maxDelay);
        nextSpawnTime = Time.time + delay;
    }
}

