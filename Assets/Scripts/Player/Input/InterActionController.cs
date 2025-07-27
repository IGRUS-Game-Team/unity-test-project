using System;
using UnityEngine;

public class InterActionController : MonoBehaviour
{
    public static InterActionController Instance { get; private set; }
    private InteractionInput inputActions;

    public event Action OnClick;
    public event Action OnThrowBox;
    public event Action OnDrop;

    void Awake()
    {
        if (Instance != null && Instance != this) //인풋매니저 싱글톤으로 관리
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        inputActions = new InteractionInput();
        inputActions.Player.Enable();
        inputActions.Player.Click.performed += ctx => OnClick?.Invoke();
        inputActions.Player.ThrowBox.performed += ctx => OnThrowBox?.Invoke();
        inputActions.Player.Drop.performed += ctx => OnDrop?.Invoke();
    }
    
    private void OnDestroy()
    {
        inputActions.Player.Click.performed -= ctx => OnClick?.Invoke();
        inputActions.Player.ThrowBox.performed -= ctx => OnThrowBox?.Invoke();
        inputActions.Player.Drop.performed -= ctx => OnDrop?.Invoke();
    }
}
