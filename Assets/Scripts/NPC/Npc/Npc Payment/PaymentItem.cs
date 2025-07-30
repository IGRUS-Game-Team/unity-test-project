using UnityEngine;
using UnityEngine.EventSystems;

// NPC 손에 붙는 결제 오브젝트(현금·카드). 클릭 시 결제 완료 처리.
public class PaymentItem : MonoBehaviour, IPointerClickHandler
{
    private PaymentContext payContext;           // 결제 정보
    private System.Action  onPaidCallback;       // 결제 완료 후 실행할 콜백

    // NPC가 아이템을 내밀 때 호출해 초기화
    public void Init(PaymentContext context, System.Action onPaid)
    {
        this.payContext      = context;          // 결제 정보 저장
        this.onPaidCallback  = onPaid;           // 외부 콜백 저장
    }

    // 플레이어가 아이템을 클릭했을 때 자동 실행
    public void OnPointerClick(PointerEventData eventData)
    {
        // 1) 외부 콜백 실행(NPC 상태 전환·줄 처리 등)
        if (onPaidCallback != null)
        {
            onPaidCallback.Invoke();
        }

        // 2) 결제 안내 패널 숨김
        if (UIManager.uiManager != null)
        {
            UIManager.uiManager.HidePayPrompt();
        }

        // 3) 손에서 아이템 제거
        Destroy(this.gameObject);
    }
}