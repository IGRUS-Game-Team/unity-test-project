using UnityEngine;

public class NpcState_OfferPayment : IState
{
    /* ── 필드 ─────────────────────────────────────── */
    readonly NpcController _npc;
    readonly QueueManager  _queue;
    readonly GameObject    _cashPrefab, _cardPrefab;
    readonly Transform     _counter;               // 계산대 트랜스폼

    const float ROT_SPEED  = 720f;                 // °/s ‒ 회전 속도
    const float DONE_ANGLE = 3f;                   // 허용 오차(°)

    bool _itemSpawned;
    bool _rotationComplete;   // 스냅 후 true
    PaymentContext _ctx;

    /* ── 생성자 ───────────────────────────────────── */
    public NpcState_OfferPayment(
        NpcController npc,
        QueueManager  queue,
        GameObject    cashPrefab,
        GameObject    cardPrefab,
        Transform     counter)
    {
        _npc        = npc;
        _queue      = queue;
        _cashPrefab = cashPrefab;
        _cardPrefab = cardPrefab;
        _counter    = counter;
    }

    /* ── 상태 진입 ────────────────────────────────── */
    public void Enter()
    {
        // 결제 데이터 미리 준비
        _ctx = new PaymentContext
        {
            totalPrice = 0f,                                   // 나중 확장
            method     = Random.value < .5f
                         ? PaymentType.Cash : PaymentType.Card,
            payer      = _npc
        };

        // 에이전트 회전·이동 중지
        _npc.Agent.isStopped      = true;
        _npc.Agent.updateRotation = false;
    }

    /* ── 매 프레임 로직 ───────────────────────────── */
    public void Tick()
    {
        if (!_rotationComplete)
        {
            // 1) 목표 방향 계산 (수평)
            Vector3 dir = _counter.position - _npc.transform.position;
            dir.y = 0f;

            // 2) 목표 회전
            Quaternion targetRot = Quaternion.LookRotation(dir);

            // 3) 부드럽게 회전
            _npc.transform.rotation = Quaternion.RotateTowards(
                                      _npc.transform.rotation,
                                      targetRot,
                                      ROT_SPEED * Time.deltaTime);

            // 4) 근사치에 도달했는가?
            if (Quaternion.Angle(_npc.transform.rotation, targetRot) <= 1f)
            {
            // 4-1) **정확히** 맞춘다 (오차 0°)
            _npc.transform.rotation = targetRot;

            // 4-2) 이후 Tick에서 더 이상 회전시키지 않음
            _rotationComplete = true;
            }

            // 5) 정확히 맞춘 **그 프레임**에만 아이템 소환
            if (!_itemSpawned && _rotationComplete)
            {
                SpawnPaymentItem();
                _itemSpawned = true;
            }
        }
    /* _rotationComplete == true 이후에는 회전·스폰 로직 모두 건너뜁니다 */
    }

    /* ── 아이템 생성 & 클릭 콜백 ───────────────────── */
    void SpawnPaymentItem()
    {
        Transform hand = _npc.HandTransform;
        GameObject prefab = _ctx.method == PaymentType.Cash
                            ? _cashPrefab : _cardPrefab;

        if (hand == null || prefab == null)
        {
            Debug.LogError($"[OfferPayment] Missing ref ▶ hand:{hand} prefab:{prefab}");
            _npc.SM.SetState(new NpcState_Leave(_npc));
            return;
        }

        // 손 소켓에 생성 (약간 앞으로 5 cm 밀어 가려움 방지) ⬅︎
        GameObject item = Object.Instantiate(prefab, hand);
        item.transform.localPosition = Vector3.forward * 0.05f; // ⬅︎
        item.transform.localRotation = Quaternion.identity;      // ⬅︎

        item.GetComponent<PaymentItem>().Init(_ctx, OnPaid);
    }

    void OnPaid()
    {
        _queue.DequeueFront();
        _npc.SM.SetState(new NpcState_Leave(_npc));
        // EventBus.RaisePaymentCompleted(_ctx);  // 필요하면 사용
    }

    /* ── 상태 종료 ───────────────────────────────── */
    public void Exit()
    {
        _npc.Agent.updateRotation = true;
        _npc.Agent.isStopped      = false;
    }
}