using UnityEngine;
using UnityEngine.AI;

public class NpcMover : MonoBehaviour
{
    const string DESTINATION_STRING = "Destination";

    NavMeshAgent agent;
    
    public bool hasVisitedStore = false; // ChangeDestination에서 Trigger 중복 방지용 불리언 값

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        GameObject gameObject = GameObject.Find(DESTINATION_STRING);
        if (gameObject)
        {
            SetTarget(gameObject.transform);
        }
    }

    public void SetTarget(Transform target)
    {
        if (target == null) return;
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }
}

