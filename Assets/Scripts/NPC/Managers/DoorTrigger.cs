using System.Collections.Generic;
using UnityEngine;

// NPC가 들어올 때 빈 선반을 배정하고, 동시에 머무는 인원 수를 제한한다.
public class DoorTrigger : MonoBehaviour
{
    [Header("NPC가 탐색할 선반 부모")]
    [SerializeField] private List<Transform> slotParents = new List<Transform>();

    [Header("NPC 퇴장 위치")]
    [SerializeField] private Transform exitPoint;

    [Header("매장 최대 인원")]
    [SerializeField] private int maxInStore = 10;

    private const string TagNpc = "Npc";      // 다른 스크립트와 동일하게 유지

    private ShelfSlot[] slots;                // 매장 안 모든 슬롯
    private int insideCount;                  // 현재 매장 내부 NPC 수

    private void Awake()
    {
        // slotParents 하위에서 ShelfSlot을 수집
        List<ShelfSlot> collected = new List<ShelfSlot>();
        foreach (Transform parent in slotParents)
        {
            if (parent == null) continue;
            collected.AddRange(parent.GetComponentsInChildren<ShelfSlot>(false));
        }
        slots = collected.ToArray();
    }

    private void OnTriggerEnter(Collider other)
    {
        // NPC만 처리
        if (!other.CompareTag(TagNpc)) return;

        NpcController npc = other.GetComponent<NpcController>();
        if (npc == null || npc.isLeaving) return;

        // 비어 있는 슬롯 찾기
        List<ShelfSlot> freeSlots = new List<ShelfSlot>();
        foreach (ShelfSlot slot in slots)
        {
            if (!slot.IsReserved) freeSlots.Add(slot);
        }

        // 슬롯이 없거나 인원 초과면 입장 거부
        if (freeSlots.Count == 0 || insideCount >= maxInStore)
        {
            npc.StartLeaving(exitPoint);
            return;
        }

        // 랜덤 빈 슬롯 선택 후 예약
        ShelfSlot chosen = freeSlots[Random.Range(0, freeSlots.Count)];
        chosen.TryReserve();

        // NPC 입장 처리
        insideCount++;
        npc.SetDoor(this);
        npc.inStore = true;
        npc.AllowEntry(chosen.transform, exitPoint);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(TagNpc)) return;

        NpcController npc = other.GetComponent<NpcController>();
        if (npc == null) return;

        // 매장에 있었고 퇴장 중이라면 인원 수 감소
        if (npc.inStore && npc.isLeaving)
        {
            insideCount = Mathf.Max(0, insideCount - 1);
            npc.inStore = false;
        }
    }
}