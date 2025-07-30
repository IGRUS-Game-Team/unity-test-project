using System.Collections.Generic;
using UnityEngine;

// 카운터 앞에 줄을 세우고 이동·시선을 관리한다.
public class QueueManager : MonoBehaviour
{
    /* ---------- 인스펙터 설정 ---------- */

    [Header("줄 서는 위치(노드)")]
    [SerializeField] private Transform[] spots;          // 줄 칸 위치

    [Header("줄이 꽉 찼을 때 NPC가 배회할 지점")]
    [SerializeField] private Transform[] wanderPoints;   // 배회 포인트

    [Header("계산대 Transform")]
    [SerializeField] private Transform counter;          // 카운터 위치

    /* ---------- 내부 상태 ---------- */

    private readonly List<NpcController> waitingLine = new List<NpcController>(); // 줄에 선 NPC 목록

    /* ---------- 외부 접근 프로퍼티 ---------- */

    public Transform CounterTransform => counter;        // 카운터 위치 제공
    public Transform[] WanderPoints => wanderPoints;     // 배회 포인트 제공

    /* ---------- 초기화 ---------- */

    private void Awake()
    {
        // spots 배열이 비어 있으면, 자식 오브젝트 중 이름에 "spot"이 들어간 Transform을 자동 수집
        if (spots == null || spots.Length == 0)
        {
            List<Transform> foundSpots = new List<Transform>();

            foreach (Transform child in transform)                       // QueueManager의 자식 순회
            {
                if (child.name.Contains("spot"))                         // 이름에 "spot"이 있으면
                {
                    foundSpots.Add(child);                               // 후보 목록에 추가
                }
            }

            // 이름 순으로 정렬해 줄 순서를 유지
            foundSpots.Sort((Transform a, Transform b) => string.CompareOrdinal(a.name, b.name));

            spots = foundSpots.ToArray();                                // 배열로 변환
        }
    }

    /* ---------- 내부 유틸 ---------- */

    // 줄 전체의 시선을 다시 계산
    private void RefreshLookTargets()
    {
        // 첫 번째 NPC는 계산대를, 그 뒤는 앞사람을 바라보도록 설정
        for (int i = 0; i < waitingLine.Count; i++)
        {
            Transform lookTarget = (i == 0) ? counter : waitingLine[i - 1].transform;
            waitingLine[i].SetLookTarget(lookTarget);
        }
    }

    /* ---------- 외부 호출 메서드 ---------- */

    // NPC가 줄 서기를 시도
    public bool TryEnqueue(NpcController npcController, out Transform node)
    {
        // 줄이 가득 찼는지 확인
        if (waitingLine.Count >= spots.Length)
        {
            node = null;                                 // 자리 없음
            return false;
        }

        // 줄에 NPC 추가
        waitingLine.Add(npcController);

        // 방금 추가된 NPC가 서야 할 노드
        node = spots[waitingLine.Count - 1];

        // 시선 재계산
        RefreshLookTargets();

        return true;
    }

    // 맨 앞 NPC가 결제 완료 → 한 칸씩 앞으로 이동
    public void DequeueFront()
    {
        if (waitingLine.Count == 0) return;              // 줄이 비면 무시

        waitingLine.RemoveAt(0);                         // 맨 앞 제거

        // 남은 NPC들에게 새 노드 지정
        for (int i = 0; i < waitingLine.Count; i++)
        {
            waitingLine[i].SetQueueTarget(spots[i]);
        }

        RefreshLookTargets();                            // 시선 재계산
    }

    // 특정 NPC가 맨 앞인지 확인
    public bool IsFront(NpcController npcController)
    {
        return waitingLine.Count > 0 && waitingLine[0] == npcController;
    }

    // 줄 중간에서 NPC가 포기하거나 제거될 때
    public void Remove(NpcController npcController)
    {
        int index = waitingLine.IndexOf(npcController);  // NPC 위치 찾기
        if (index < 0) return;                           // 줄에 없으면 무시

        waitingLine.RemoveAt(index);                     // 목록에서 제거

        // 뒤쪽 NPC 노드 업데이트
        for (int i = index; i < waitingLine.Count; i++)
        {
            waitingLine[i].SetQueueTarget(spots[i]);
        }

        RefreshLookTargets();                            // 시선 재계산
    }
}