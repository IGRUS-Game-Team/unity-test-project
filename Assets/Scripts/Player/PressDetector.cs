using UnityEngine;
using UnityEngine.InputSystem;

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