using UnityEngine;

public class NpcState_Wander : IState
{
    readonly NpcController _npc;
    readonly Transform[] _wanderPoints;   // Inspector 에서 배열로 주입하거나, 매니저 통해 받기
    readonly float _minIdle = 2f;   // 한 지점 도착 후 최소 대기
    readonly float _maxIdle = 5f;   // 한 지점 도착 후 최대 대기
    readonly float _maxWanderTime;  // Wander 최대 지속(초) → 넘으면 Leave

    float _nextMoveTime;
    float _wanderEnd;

    public NpcState_Wander(NpcController npc, Transform[] wanderPoints, float maxWanderTime = 20f)
    {
        _npc            = npc;
        _wanderPoints   = wanderPoints;
        _maxWanderTime  = maxWanderTime;
    }

    public void Enter()
    {
        _npc.Agent.isStopped = false;
        PickNextDestination();

        // Wander 를 무한 반복하지 않도록 종료 시각 지정
        _wanderEnd = Time.time + _maxWanderTime;
        _npc.Anim.Play("Walking");
    }

    public void Tick()
    {
        // Wander 시간이 끝나면 바로 Leave 상태
        if (Time.time >= _wanderEnd)
        {
            _npc.SM.SetState(new NpcState_Leave(_npc));
            return;
        }

        // 도착 판정
        bool arrived =
            !_npc.Agent.pathPending &&
            _npc.Agent.remainingDistance  <= 0.1f &&
            _npc.Agent.velocity.sqrMagnitude < 0.01f;

        if (!arrived) return;

        // 지점에 도착했으면 잠깐 서 있기
        if (Time.time < _nextMoveTime) return;

        PickNextDestination();
    }

    public void Exit() { }

    void PickNextDestination()
    {
        if (_wanderPoints == null || _wanderPoints.Length == 0)
        {
            // 대체 경로: 출구로 이동
            _npc.SM.SetState(new NpcState_Leave(_npc));
            return;
        }

        // 랜덤 지점 선택
        Transform p = _wanderPoints[Random.Range(0, _wanderPoints.Length)];
        _npc.Agent.SetDestination(p.position);

        // 다음 지점으로 이동하기 전 대기 시간 설정
        _nextMoveTime = Time.time + Random.Range(_minIdle, _maxIdle);
    }
}