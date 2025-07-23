using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChangeDestination : MonoBehaviour
{
    [Header("상점 내부 선반 위치들")]
    [SerializeField] Transform[] insidePoints;

    [Header("NPC 퇴장 위치")]
    [SerializeField] Transform exitPoint;
    [Header("선반에서 머무르는 시간")]
    [SerializeField] float minStayTime = 3f;
    [SerializeField] float maxStayTime = 7f;

    [Header("도착 판정 거리")]
    [SerializeField] float arrivalDistance = 0.2f;

    const string NPC_STRING = "NPC";

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(NPC_STRING)) return;
        NpcMover mover = other.GetComponent<NpcMover>();
        if (mover == null || mover.hasVisitedStore == true) return;

        mover.hasVisitedStore = true;
        Transform randomDestination = insidePoints[Random.Range(0, insidePoints.Length)];
        mover.SetTarget(randomDestination);

        StartCoroutine(HandleShopping(mover));
    }

    IEnumerator HandleShopping(NpcMover mover)
    {
        NavMeshAgent agent = mover.GetComponent<NavMeshAgent>();

        while (agent.pathPending || agent.remainingDistance > arrivalDistance)
        yield return null;

        float wait = Random.Range(minStayTime, maxStayTime);
        yield return new WaitForSeconds(wait);

        mover.SetTarget(exitPoint);
    }
}
