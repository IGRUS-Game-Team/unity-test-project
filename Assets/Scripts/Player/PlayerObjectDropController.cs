using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using System;
/// <summary>
/// PlayerObjectDropController.cs 박정민
/// drop 키 (R) 눌렀을때 Hold된 오브젝트 던지는 역할 하는 클래스
/// Player에 붙인다.
/// </summary>

public class PlayerObjectDropController : MonoBehaviour
{
    [SerializeField] float throwForce = 10f;
    PlayerObjectHoldController playerObjectHoldController;

    public BlockIsHolding heldObject;
    private bool isHolding => heldObject != null;

    void Awake()
    {
        playerObjectHoldController = GetComponent<PlayerObjectHoldController>();
    }
    void Start()
    {
        InterActionController.Instance.OnThrowBox += Drop; //이벤트 구독
    }
    void Update()
    {
        heldObject = playerObjectHoldController.heldObject;
    }

    // void Update() //예전 이벤트 구조 쓰기 전 방식
    // {
    //     heldObject = playerObjectHoldController.heldObject;

    //     if (heldObject == null && input.drop == true) input.DropInput(false);
    //     if (input.drop && isHolding)
    //     {
    //         Drop();
    //         input.DropInput(false);
    //     }

    // }

    private void Drop()
    {
        Debug.Log("던지기");
        if (heldObject == null)
        {
            Debug.Log("아왜안됨");
            return;
        }

        //var rb = heldObject.GetComponent<Rigidbody>();
        // heldObject는 box1에 붙어있다고 가정
        Transform root = heldObject.transform;

        // Rigidbody와 Collider도 box1에 붙어 있다고 가정
        var rb = root.GetComponent<Rigidbody>();
        var col = root.GetComponent<Collider>();

        // 원래 부모로 복원 + 위치 유지
        // heldObject.transform.SetParent(heldObject.originalParent, true);
        // heldObject.GetComponent<Collider>().enabled = true;
        root.SetParent(heldObject.originalParent, true);
        col.enabled = true; // Collider 도 root에 붙어야함

        TurnOnPhysics(rb);

        heldObject.isHeld = false;
        playerObjectHoldController.heldObject = null;
        rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

    }

    private static void TurnOnPhysics(Rigidbody rb)
    {
        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.useGravity = true;
    }
}
