using System.Collections.Generic;
using UnityEngine;

public class NpcSpawnManager : MonoBehaviour
{
    [SerializeField] Transform npcs;
    [SerializeField] Transform doorPoint;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject[] npcPrefabs;
    [SerializeField] float minDelay = 1f;
    [SerializeField] float maxDelay = 3f;

    readonly List<NpcController> _pool = new();
    float _next;

    void Update()
    {
        // 풀 정리
        _pool.RemoveAll(npc => npc == null);

        if (Time.time < _next) return;
        SpawnNpc();
        _next = Time.time + Random.Range(minDelay, maxDelay);
    }

    void SpawnNpc()
    {
        if (npcPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject gameObject = Instantiate(prefab, point.position, Quaternion.identity, npcs);
        _pool.Add(gameObject.GetComponent<NpcController>());
        
        NpcController npc = gameObject.GetComponent<NpcController>();
        npc.SetDoorPoint(doorPoint);
    }
}
