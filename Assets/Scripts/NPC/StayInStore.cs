using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StayInStore : MonoBehaviour
{
    [SerializeField] public float storeStayTime = 5f;

    const string STORE_STRING = "Store";

    NavMeshAgent agent;

    bool hasStayedOnce = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasStayedOnce) return;
        if (other.CompareTag(STORE_STRING))
        {
            hasStayedOnce = true;
            StartCoroutine(StayInStoreRoutine());
        }
    }

    IEnumerator StayInStoreRoutine()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(storeStayTime);
        agent.isStopped = false;
    }
}
