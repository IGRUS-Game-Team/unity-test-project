using UnityEngine;

// 화면 중앙 에임으로 결제 아이템을 클릭하면 결제 처리해 주는 스크립트
public class PlayerPaymentInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;   // 플레이어 카메라 지정
    [SerializeField] private float maxDistance = 5f;// 상호작용 최대 거리

    void Update()
    {
        // 좌클릭이 들어왔는지 체크
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        // 화면 중앙 좌표로 레이 생성
        Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);

        // 레이캐스트로 충돌 정보 얻기
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, maxDistance))
        {
            // 충돌한 오브젝트에 PaymentItem 컴포넌트가 있는지 확인
            PaymentItem item = hitInfo.collider.GetComponent<PaymentItem>();
            if (item != null)
            {
                // 결제 처리 메서드 호출 → NPC 퇴장
                item.PayByPlayer();
            }
        }
    }
}