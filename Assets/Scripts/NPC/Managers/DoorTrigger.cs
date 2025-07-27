using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Header("Npc가 갈 수 있는 선반들")]
    [SerializeField] List<Transform> slotParents = new();

    [Header("퇴장 포인트")]
    [SerializeField] Transform exitPoint;

    [Header("최대 인원")]
    [SerializeField] int maxInStore = 10;

    ShelfSlot[] _slots;
    int _current;

    void Awake()
    {
        var list = new List<ShelfSlot>();

        // 부모가 여러 개면 각각 돌면서 슬롯 모으기
        foreach (var parent in slotParents)
        {
            if (parent == null) continue;
            list.AddRange(parent.GetComponentsInChildren<ShelfSlot>(false));
        }

        _slots = list.ToArray();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("NPC")) return;
        var npc = other.GetComponent<NpcController>();
        if (npc == null || npc.IsLeaving) return;

        // 1) 인원 초과?
        if (_current >= maxInStore)
        {
            npc.StartLeaving(exitPoint);
            return;
        }

        // 2) 빈 슬롯 예약
        List<ShelfSlot> freeSlots = new();

        foreach (var slot in _slots)
        {
            if (!slot.IsReserved) freeSlots.Add(slot);
        }

        // 3) 빈 슬롯 없으면 나가기
        if (freeSlots.Count == 0 || _current >= maxInStore)
        {
            npc.StartLeaving(exitPoint);
            return;
        }

        // ★ 무작위 슬롯 하나 뽑기
        ShelfSlot chosen = freeSlots[Random.Range(0, freeSlots.Count)];
        chosen.TryReserve();   // 예약 확정

        // 4) 입장 허가
        _current++;
        npc.SetDoor(this);
        npc.InStore = true;

        npc.TargetShelfSlot = chosen.transform;
        npc.ExitPoint = exitPoint;
        npc.SM.SetState(new NpcState_ToShelf(npc));
    }

    
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("NPC")) return;

        var npc = other.GetComponent<NpcController>();
        if (npc == null) return;

        // “매장 안이었고” + “퇴장 중일 때”만 카운트 감소
        if (npc.InStore && npc.IsLeaving)
        {
            DecreaseCount();
            npc.InStore = false;        // 플래그 초기화
        }
    }

    void DecreaseCount()
    {
        _current = Mathf.Max(0, _current - 1);
    }
}