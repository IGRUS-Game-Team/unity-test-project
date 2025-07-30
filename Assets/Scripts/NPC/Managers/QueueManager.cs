using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    /* ── 인스펙터 슬롯 ───────────────────────────── */
    [Header("줄 노드(맨 앞 = Element 0)")]
    [SerializeField] Transform[] nodes;          // Node0 ~ NodeN

    [Header("NPC 배회 포인트들")]
    [SerializeField] Transform[] wanderPoints;

    [Header("카운터 Transform")]
    [SerializeField] Transform counter;          // 계산대 피벗

    /* ── 프로퍼티 ───────────────────────────────── */
    readonly List<NpcController> line = new();

    public Transform Counter         => counter;
    public Transform[] WanderPoints  => wanderPoints;

    /* ── 초기화 : Node 자동 수집 (옵션) ───────────── */
    void Awake()
    {
        if (nodes == null || nodes.Length == 0)
        {
            var list = new List<Transform>();
            foreach (Transform t in transform)
                if (t.name.Contains("Node"))
                    list.Add(t);

            list.Sort((a, b) => string.CompareOrdinal(a.name, b.name)); // Node0, Node1 …
            nodes = list.ToArray();
        }
    }

    /* ─────────────────────────────────────────────
       0)  줄 전체 ‘시선’ 재계산  (맨 앞 → Counter, 그 뒤 → 앞사람)
    ───────────────────────────────────────────── */
    void RefreshLookTargets()
    {
        for (int i = 0; i < line.Count; i++)
        {
            Transform look = (i == 0) ? counter
                                      : line[i - 1].transform;

            line[i].SetLookTarget(look);   // NpcController 쪽에 구현되어 있어야 함
        }
    }

    /* ─────────────────────────────────────────────
       1)  줄 서기 시도
    ───────────────────────────────────────────── */
    public bool TryEnqueue(NpcController npc, out Transform node)
    {
        if (line.Count >= nodes.Length)
        {
            node = null;                               // 줄 가득 참
            return false;
        }

        line.Add(npc);
        node = nodes[line.Count - 1];                  // 내 자리
        RefreshLookTargets();                          // ★ 시선 재계산
        return true;
    }

    /* ─────────────────────────────────────────────
       2)  맨 앞 결제 완료 → 한 칸씩 당기기
    ───────────────────────────────────────────── */
    public void DequeueFront()
    {
        if (line.Count == 0) return;

        line.RemoveAt(0);

        // 각 NPC에게 새 노드 전달
        for (int i = 0; i < line.Count; i++)
            line[i].SetQueueTarget(nodes[i]);

        RefreshLookTargets();                          // ★ 시선 재계산
    }

    /* ─────────────────────────────────────────────
       3)  내가 맨 앞인가?
    ───────────────────────────────────────────── */
    public bool IsFront(NpcController npc)
    {
        return line.Count > 0 && line[0] == npc;
    }

    /* ─────────────────────────────────────────────
       4)  줄 중간에서 포기·삭제될 때
    ───────────────────────────────────────────── */
    public void Remove(NpcController npc)
    {
        int idx = line.IndexOf(npc);
        if (idx < 0) return;                           // 줄에 없으면 무시

        line.RemoveAt(idx);

        for (int i = idx; i < line.Count; i++)
            line[i].SetQueueTarget(nodes[i]);

        RefreshLookTargets();                          // ★ 시선 재계산
    }
}