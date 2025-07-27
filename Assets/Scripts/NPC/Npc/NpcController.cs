using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NpcController : MonoBehaviour
{

    public NavMeshAgent Agent { get; private set; }
    public Animator Anim { get; private set; }
    public StateMachine SM { get; private set; }

    // 외부에서 대입
    public Transform TargetShelfSlot { get; set; }
    public Transform ExitPoint { get; set; }
    public DoorTrigger Door { get; private set; }
    public Transform QueueTarget { get; private set; }

    // 내부 상태 공유
    public bool HasItemInHand { get; private set; } = false;
    public bool InStore { get; set; } = false;
    public bool IsLeaving { get; set; } = false;

    [Header("현금 / 카드 프리팹")]
    [SerializeField] GameObject cashPrefab;
    [SerializeField] GameObject cardPrefab;

    [Header("Hand Socket")]          // ← 인스펙터에 보이게
    [SerializeField] Transform handSocket;

    Transform _lookTarget;
    Transform _handSocket;
    GameObject _spawnedObject;    // 현재 손에 쥔 오브젝트
    Transform doorPoint;

    void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponentInChildren<Animator>();
        _handSocket = GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.RightHand);
        SM = new StateMachine();
    }
    void Start() => SM.SetState(new NpcState_ToDoor(this, doorPoint));
    void Update() => SM.Tick();

    public Transform HandTransform => _handSocket;   // OfferPayment에서 사용
    public GameObject CashPrefab => cashPrefab;
    public GameObject CardPrefab => cardPrefab;
    public Transform LookTarget => _lookTarget;

    public void SetDoor(DoorTrigger door) => Door = door;
    public void SetLookTarget(Transform t) => _lookTarget = t;

    // 트리거에서 호출
    public void AllowEntry(Transform shelfSlot, Transform exitPoint)
    {
        TargetShelfSlot = shelfSlot;
        ExitPoint = exitPoint;
        SM.SetState(new NpcState_ToShelf(this));
    }

    public void StartLeaving(Transform exitPoint)
    {
        ExitPoint = exitPoint;
        IsLeaving = true;                 // ★ 여기
        SM.SetState(new NpcState_Leave(this));
    }

    public void OnPickAnimationFinished()
    {
        HasItemInHand = true;
    }

    public void SetDoorPoint(Transform dp) => doorPoint = dp;

    public void SetQueueTarget(Transform node)
    {
        QueueTarget = node;
        Agent.isStopped = false;
        Agent.SetDestination(node.position);
    }

    public void ShowMoneyOrCard()
    {
        if (_spawnedObject != null) return;      // 이미 들고 있으면 무시

        // 랜덤하게 현금 또는 카드 선택
        GameObject prefab = Random.value < 0.5f ? cashPrefab : cardPrefab;
        _spawnedObject = Instantiate(prefab, _handSocket);
        _spawnedObject.transform.localPosition = Vector3.zero;
        _spawnedObject.transform.localRotation = Quaternion.identity;

        HasItemInHand = true;
    }

    public void HideMoneyOrCard()
    {
        if (_spawnedObject != null) Destroy(_spawnedObject);
        _spawnedObject = null;
        HasItemInHand = false;
    }
    
    public void FaceLookTarget(float speedDegPerSec)
    {
        if (!_lookTarget) return;
        Vector3 dir = _lookTarget.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(
                            transform.rotation, rot,
                            speedDegPerSec * Time.deltaTime);
    }
}