using UnityEngine;

public class MonitorInteractionManager : MonoBehaviour
{
    public static MonitorInteractionManager Instance { get; private set; }

    [SerializeField] Camera playerCam;
    [SerializeField] Camera monitorCam;
    [SerializeField] RenderTextureUIClicker uiClicker;

    private bool isUIActive = false;

    private void Awake()
    {
        Instance = this;
    }

    public void Interact()
    {
        if (isUIActive) return;

        isUIActive = true;
        Debug.Log("모니터 UI 진입");

        playerCam.gameObject.SetActive(false);
        monitorCam.gameObject.SetActive(true);
        uiClicker.enabled = true;

        // 필요하면 마우스 커서 표시
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitUI()
    {
        isUIActive = false;
        playerCam.gameObject.SetActive(true);
        monitorCam.gameObject.SetActive(false);
        uiClicker.enabled = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}