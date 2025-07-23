using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RenderTextureUIClicker : MonoBehaviour
{
    [SerializeField] Camera playerCam;
    [SerializeField] Camera monitorCam;             // UI를 찍는 카메라
    [SerializeField] RectTransform monitorCanvasRt; // UI Canvas RectTransform
    [SerializeField] Renderer monitorScreen;        // RenderTexture 붙은 Mesh
    [SerializeField] GraphicRaycaster uiRaycaster;
    [SerializeField] EventSystem eventSystem;

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            Debug.Log("안눌림");
            return;
        }
        Debug.Log("병신");
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("No Physics Hit");
            return;
        }
        if (hit.collider.gameObject != monitorScreen.gameObject)
        {
            Debug.Log("Hit something else: " + hit.collider.name);
            return;
        }
        Debug.Log("느금마" + ray);
        // 1. UV 좌표 얻기 (0~1)
        Vector2 uv = hit.textureCoord;
        Debug.Log("아시시ㅣ시이이이이이이이이발" + uv);

        // 2. UV -> Canvas 스크린좌표로 변환
        Vector2 canvasSize = monitorCanvasRt.sizeDelta;
        Vector2 local = new Vector2(
            (uv.x * canvasSize.x) - canvasSize.x * 0.5f,
            (uv.y * canvasSize.y) - canvasSize.y * 0.5f);
        Debug.Log("하시발왜안되는거야애미" + local);

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
            monitorCam,
            monitorCanvasRt.TransformPoint(local));

        // 3. GraphicRaycaster로 클릭 전달
        PointerEventData ped = new PointerEventData(eventSystem) { position = screenPos };
        var results = new List<RaycastResult>();
        uiRaycaster.Raycast(ped, results);
        foreach (var r in results)
            ExecuteEvents.Execute(r.gameObject, ped, ExecuteEvents.pointerClickHandler);
    }


}
