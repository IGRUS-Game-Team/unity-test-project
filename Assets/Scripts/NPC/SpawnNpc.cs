using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    [Header("NPC 스포너 위치")]
    [SerializeField] Transform spawnPoint;

    [SerializeField] Transform NPCManager;

    [Header("NPC 프리팹들")]
    [SerializeField] GameObject[] npcPrefabs;

    [Header("NPC 스폰 딜레이")]
    [SerializeField] float minDelay = 1f;
    [SerializeField] float maxDelay = 3f;

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
        Instantiate(prefab, spawnPoint.position, Quaternion.identity, NPCManager);
    }

    void NextSpawn()
    {
        float delay = Random.Range(minDelay, maxDelay);
        nextSpawnTime = Time.time + delay;
    }
}

