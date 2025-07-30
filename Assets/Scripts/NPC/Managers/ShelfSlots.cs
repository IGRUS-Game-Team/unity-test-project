using UnityEngine;

// 선반의 한 칸을 나타내는 컴포넌트.
// ─ IsReserved : 현재 슬롯이 예약되었는지 여부.
// ─ TryReserve : 비어 있을 때 예약 후 true 반환.
// ─ Release    : 예약 해제.
public class ShelfSlot : MonoBehaviour
{
    // 외부에서 읽기만 가능, 수정은 내부 메서드로만
    public bool IsReserved { get; private set; }

    // 슬롯이 비어 있으면 예약하고 true, 이미 사용 중이면 false
    public bool TryReserve()
    {
        if (IsReserved) return false;   // 이미 예약된 경우

        IsReserved = true;              // 예약 성공
        return true;
    }

    // NPC가 떠날 때 호출하여 예약 해제
    public void Release()
    {
        IsReserved = false;
    }
}