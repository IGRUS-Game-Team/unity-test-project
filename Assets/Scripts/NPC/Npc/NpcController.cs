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

    // 내부 상태 공유
    public bool HasItemInHand { get; set; }
    public bool InStore  { get; set; } = false;
    public bool IsLeaving { get; set; } = false;

    Transform doorPoint;

    void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponentInChildren<Animator>();
        SM = new StateMachine();
    }
    void Start() => SM.SetState(new NpcState_ToDoor(this, doorPoint));
    void Update() => SM.Tick();

    public void SetDoor(DoorTrigger door) => Door = door;

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
}