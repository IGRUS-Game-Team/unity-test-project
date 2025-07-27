using UnityEngine;

public class ShelfSlot : MonoBehaviour
{
    public bool IsReserved { get; private set; } = false;

    public bool TryReserve()          // 비어 있으면 true 반환 + 예약
    {
        if (IsReserved) return false;
        IsReserved = true;
        return true;
    }

    public void Release()             // NPC가 떠날 때 호출
    {
        IsReserved = false;
    }
}