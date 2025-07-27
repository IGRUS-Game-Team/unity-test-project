using UnityEngine;
using UnityEngine.InputSystem;

//PressDetector.cs 박정민
//박스는 클릭하면 바로 집도록 로직을 짯지만 모니터나 키오스크, 가구들은 클릭을 몇초동안 지속하면(꾹 누르면)
//집도록 설계한 클래스입니다. 7/25 회의에서 가구들은 고정된 것으로 취급하도록 결정되었으므로
//해당 클래스는 사용되지 않습니다.
public class PressDetector : MonoBehaviour
{
    public float holdThreshold = 0.5f; // 꾹 누른 기준 시간 (초)

    private float pressTime = 0f;
    private bool isPressing = false;
    private bool hasTriggered = false;

    public System.Action OnShortPress;
    public System.Action OnLongPress;

    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            if (!isPressing)
            {
                isPressing = true;
                pressTime = 0f;
                hasTriggered = false;
            }

            pressTime += Time.deltaTime;

            if (pressTime >= holdThreshold && !hasTriggered)
            {
                hasTriggered = true;
                OnLongPress?.Invoke(); // 꾹 누름
            }
        }
        else
        {
            if (isPressing)
            {
                if (!hasTriggered)
                {
                    OnShortPress?.Invoke(); // 짧게 누름
                }
                isPressing = false;
            }
        }
    }
}