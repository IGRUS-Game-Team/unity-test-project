using UnityEngine;

public class BoxInteractionManager : MonoBehaviour
{
    public static BoxInteractionManager Instance { get; private set; }

    public bool IsActive => isBoxInteractionManagerPlaying;
    private bool isBoxInteractionManagerPlaying;

    private void Awake()
    {
        Instance = this;
    }

    public void Activate()
    {
        if (!isBoxInteractionManagerPlaying)
        {
            isBoxInteractionManagerPlaying = true;
            Debug.Log("박스 인터랙션 가능 상태 진입");
        }
    }

    public void Deactivate()
    {
        if (isBoxInteractionManagerPlaying)
        {
            isBoxInteractionManagerPlaying = false;
            Debug.Log("박스 인터랙션 상태 종료");
        }
    }

    public void Interact()
    {
        if (!isBoxInteractionManagerPlaying) return;
        // 예: 애니메이션 재생, 아이템 지급 등
        Deactivate();
    }

}
