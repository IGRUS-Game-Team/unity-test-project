using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using System;

public class PlayerObjectHoldController : MonoBehaviour
{
    [SerializeField] float placeRange = 3f;
    [SerializeField] Transform holdPoint;
    [SerializeField] float pickupRange;
    [SerializeField] LayerMask pickupLayer;
    [SerializeField] Camera PlayerCam;

    SelectionManager selectionManager;
    PlayerObjectDropController playerObjectDropController;
    PlayerObjectSetController playerObjectSetController;


    public BlockIsHolding heldObject;
    private bool isHolding => heldObject != null;
    void Awake()
    {
        selectionManager = GetComponent<SelectionManager>();
        playerObjectDropController = GetComponent<PlayerObjectDropController>();
        playerObjectSetController = GetComponent<PlayerObjectSetController>();
        pickupRange = selectionManager.pickupRange;
        pickupLayer = selectionManager.pickupLayer;
    }
    void Start()
    {
        InterActionController.Instance.OnClick += TryPickup;
    }

    private void TryPickup()
    {
        if (heldObject != null) return;

        Ray ray = PlayerCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer))
        {
            var target = hit.collider.GetComponentInParent<BlockIsHolding>();
            if (target == null || target.isHeld) return;
            //테스트코드
            string tag = hit.collider.transform.root.tag;
            if (tag != "Box") return;
            // heldObject = target;
            // heldObject.isHeld = true;
            SetHeldObject(target);
            heldObject.originalParent = target.transform.parent; // 원래 부모 기억

            var rb = heldObject.GetComponent<Rigidbody>();
            TurnOffPhysics(rb);

            heldObject.GetComponent<Collider>().enabled = false;
            // heldObject.transform.SetParent(holdPoint);
            // heldObject.transform.localPosition = Vector3.zero;
            // heldObject.transform.localRotation = Quaternion.identity;
            GameObject rootObj = heldObject.transform.gameObject;
            rootObj.transform.SetParent(holdPoint);
            rootObj.transform.localPosition = Vector3.zero;
            rootObj.transform.localRotation = Quaternion.identity;
            // 하위 계층에 있는 모델을 옮기는 구조가 아니라 전체 계층을 전부 이동시키기 위해 로직 변경
        }
    }

    private static void TurnOffPhysics(Rigidbody rb)
    {
        rb.isKinematic = true;
        rb.detectCollisions = false;
        rb.useGravity = false;
    }
    public void ReleaseHeldObject()
    {
        if (heldObject != null)
            {
                heldObject.isHeld = false;
                heldObject = null;
            }
    }
    public void SetHeldObject(BlockIsHolding target)
    {
        heldObject = target;
        heldObject.isHeld = true;
        playerObjectDropController.heldObject = target;
        playerObjectSetController.heldObject = target;
        
    }
}
