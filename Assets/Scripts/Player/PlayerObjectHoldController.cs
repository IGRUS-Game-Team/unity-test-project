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

    PlayerPickUpController playerPickUpController;
    PlayerObjectDropController playerObjectDropController;
    PlayerObjectSetController playerObjectSetController;


    public BlockIsHolding heldObject;
    private bool isHolding => heldObject != null;
    void Awake()
    {
        playerPickUpController = GetComponent<PlayerPickUpController>();
        playerObjectDropController = GetComponent<PlayerObjectDropController>();
        playerObjectSetController = GetComponent<PlayerObjectSetController>();
        pickupRange = playerPickUpController.pickupRange;
        pickupLayer = playerPickUpController.pickupLayer;
    }
    void Start()
    {
        InterActionController.Instance.OnClick += TryPickup;
    }

    private void TryPickup()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer))
        {
            var target = hit.collider.GetComponent<BlockIsHolding>();
            if (target == null || target.isHeld) return;

            // heldObject = target;
            // heldObject.isHeld = true;
            SetHeldObject(target);
            heldObject.originalParent = target.transform.parent; // 원래 부모 기억

            var rb = heldObject.GetComponent<Rigidbody>();
            TurnOffPhysics(rb);

            heldObject.GetComponent<Collider>().enabled = false;
            heldObject.transform.SetParent(holdPoint);
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.identity;
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
