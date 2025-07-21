using UnityEngine;
using UnityEngine.AI;

public class NpcMover : MonoBehaviour
{
    NavMeshAgent agent;

    const string DESTINATION_STRING = "Destination";

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        MoveToDestination();
    }

    void MoveToDestination()
    {
        if (agent.isStopped) return;

        Transform target = GameObject.Find(DESTINATION_STRING).transform;
        agent.SetDestination(target.position);
    }
}

