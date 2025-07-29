using UnityEngine;
using UnityEngine.InputSystem;
using System;
/// <summary>
/// PlayerObjectSetController.cs 박정민
/// set 키 (G) 눌렀을때 Hold된 오브젝트 놓는 역할 하는 클래스
/// Player에 붙인다.
/// 직렬화로 previewGreen, previewRed 넣어줘야함 (비활성 상태로)
/// </summary>

public class PlayerObjectSetController : MonoBehaviour
{
    [SerializeField] GameObject previewGreen; //물건 set 가능한 부분 나타내는 초록 블록
    [SerializeField] GameObject previewRed; //물건 set 불가능한 부분 나타내는 빨강 블록
    [SerializeField] float placeRange = 3f;
    [SerializeField] Transform holdPoint;


    public BlockIsHolding heldObject;
    PlayerObjectHoldController playerObjectHoldController;
    private bool isHolding => heldObject != null;

    private bool canPlace = false;
    private Vector3 previewTransform;
    void Awake()
    {
        playerObjectHoldController = GetComponent<PlayerObjectHoldController>();
        //HoldController의 heldObject와 동기화 하려고 불러옴

    }
    void Start()
    {
        InterActionController.Instance.OnDrop += PlaceObject; //이벤트 구독
    }

    void Update()
    {
        heldObject = playerObjectHoldController.heldObject; //heldObject 동기화

        if (isHolding)
        {
            ShowPlacementPreview();
        }
        else
        {
            HidePreview();
        }
    }

    private void TurnOnPhysics(Rigidbody rb)
    {
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.useGravity = true;
    }

    void ShowPlacementPreview()
    {
        Ray ray = new Ray(holdPoint.position, holdPoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, placeRange))
        {
            bool valid = hit.collider.CompareTag("SettableSurface"); //태그 이거여만 valid = canPlace = true
            Vector3 placePos = hit.point;

            previewGreen.SetActive(valid);
            previewRed.SetActive(!valid);

            previewGreen.transform.position = placePos;
            previewRed.transform.position = placePos;

            canPlace = valid;
            previewTransform = placePos;
        }
        else
        {
            previewGreen.SetActive(false);
            previewRed.SetActive(false);
            canPlace = false;
        }
    }

    void PlaceObject()
    {
        if (heldObject == null || !canPlace) //놓기 불가능한 장소(SettableSurface 태그 x)면 메서드 탈출
        {
            Debug.Log("놓을 수 없는 위치입니다.");
            return;
        }
        Vector3 attribute = new Vector3(0, 0.5f, 0);
        Quaternion quaternion = Quaternion.identity;
        quaternion.eulerAngles = new Vector3(0, 0, 0); // 물건 배치 조정값(그냥 set하면 살짝 어긋나게 배치되어서 하드코딩함)
        // TODO : 물건 배치 조정값 하드코딩 안하는 로직으로 변경
        heldObject.transform.rotation = quaternion;
        heldObject.transform.position = previewTransform + attribute;
        var rb = heldObject.GetComponent<Rigidbody>();
        heldObject.transform.SetParent(heldObject.originalParent, true);
        heldObject.GetComponent<Collider>().enabled = true;

        heldObject.isHeld = false;
        heldObject = null;
        TurnOnPhysics(rb);
        heldObject = null;
        playerObjectHoldController.ReleaseHeldObject();
        HidePreview();
    }

    void HidePreview()
    {
        previewGreen.SetActive(false);
        previewRed.SetActive(false);
    }

}
