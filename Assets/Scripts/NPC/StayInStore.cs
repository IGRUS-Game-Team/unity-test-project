using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StayInStore : MonoBehaviour
{
    [SerializeField] float storeStayTime = 5f;
    [SerializeField] float NavMeshAgentDelay = 2f;

    const string ARRIVED_SHELF_STRING = "Arrived Shelf";

    NavMeshAgent agent;
    Animator animator;

    bool hasStayedOnce = false;
    

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasStayedOnce) return;


        if (other.CompareTag(ARRIVED_SHELF_STRING))
        {
            hasStayedOnce = true;
            StartCoroutine(StayInStoreRoutine());
        }
    }

    IEnumerator StayInStoreRoutine()
    {
        agent.isStopped = true;
        animator.Play("Standing", 0, 0f);
        yield return new WaitForSeconds(storeStayTime);
        animator.Play("Picking Up Object", 0, 0f);
        agent.isStopped = false;
        yield return new WaitForSeconds(NavMeshAgentDelay);

        animator.Play("Walking", 0, 0f);
    }
    
}
