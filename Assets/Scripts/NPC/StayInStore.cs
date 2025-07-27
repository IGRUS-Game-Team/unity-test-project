using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StayInStore : MonoBehaviour
{
    [SerializeField] float storeStayTime = 5f;
    [SerializeField] float NavMeshAgentDelay = 2f;

    const string ARRIVED_SHELF_STRING = "Arrived Shelf";
    const string STANDING_STRING = "Standing";
    const string PICKUP_ANIMATION_STRING = "Picking Up Object";
    const string WALKING_STRING = "Walking";

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
        animator.Play(STANDING_STRING, 0, 0f);
        yield return new WaitForSeconds(storeStayTime);
        animator.Play(PICKUP_ANIMATION_STRING, 0, 0f);
        agent.isStopped = false;
        yield return new WaitForSeconds(NavMeshAgentDelay);

        animator.Play(WALKING_STRING, 0, 0f);
    }
    
}
