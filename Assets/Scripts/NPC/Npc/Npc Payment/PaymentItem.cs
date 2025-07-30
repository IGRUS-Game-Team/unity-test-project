using UnityEngine;
using UnityEngine.EventSystems;

public class PaymentItem : MonoBehaviour, IPointerClickHandler
{
    PaymentContext _ctx;
    System.Action _onPaid;

    public void Init(PaymentContext ctx, System.Action onPaid)
    {
        _ctx    = ctx;
        _onPaid = onPaid;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onPaid?.Invoke();          // NPC에게 “결제 완료” 콜백
        Destroy(gameObject);        // 손에서 사라짐
    }
}