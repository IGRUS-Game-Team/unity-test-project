using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using System;

public class PlayerObjectHolder : MonoBehaviour
{
    [SerializeField] GameObject previewGreen;
    [SerializeField] GameObject previewRed;
    [SerializeField] float placeRange = 3f;
    [SerializeField] float throwForce = 10f;
    [SerializeField] Transform holdPoint;
    [SerializeField] float pickupRange;
    [SerializeField] LayerMask pickupLayer;

    PlayerPickUpController playerPickUpController;

    private StarterAssetsInputs input;
    private BlockIsHolding heldObject;
    private bool isHolding => heldObject != null;

    private bool canPlace = false;
    private Vector3 previewTransform;
    void Awake()
    {
        input = GetComponent<StarterAssetsInputs>();
        playerPickUpController = GetComponent<PlayerPickUpController>();
        pickupRange = playerPickUpController.pickupRange;
        pickupLayer = playerPickUpController.pickupLayer;
    }

    void Update()
    {
        if (input.hold) Debug.Log("클릭");
        if (input.hold && !isHolding)
        {
            TryPickup();
            input.HoldInput(false);
        }
        else if (input.drop && isHolding)
        {
            Drop();
            input.DropInput(false);
        }

        if (isHolding) 
        {
            ShowPlacementPreview();
        }
        else HidePreview();

        if (input.set && isHolding && canPlace)
        {
            PlaceObject();
            input.SetInput(false);
        }
    }

    private void TryPickup()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer))
        {
            var target = hit.collider.GetComponent<BlockIsHolding>();
            if (target == null || target.isHeld) return;

            heldObject = target;
            heldObject.isHeld = true;
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

    private void Drop()
    {
        if (heldObject == null) return;

        var rb = heldObject.GetComponent<Rigidbody>();

        // 원래 부모로 복원 + 위치 유지
        heldObject.transform.SetParent(heldObject.originalParent, true);
        heldObject.GetComponent<Collider>().enabled = true;

        TurnOnPhysics(rb);

        heldObject.isHeld = false;
        heldObject = null;
        rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

    }

    private static void TurnOnPhysics(Rigidbody rb)
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
            bool valid = hit.collider.CompareTag("SettableSurface");
            Debug.Log("응거기맞아" + valid);
            Vector3 placePos = hit.point;
            Debug.Log(placePos + " " + hit);
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
        Vector3 attribute = new Vector3(0, 0.5f, 0);
        Quaternion quaternion = Quaternion.identity;
        quaternion.eulerAngles = new Vector3(0, 0, 0);
        heldObject.transform.rotation = quaternion;
        heldObject.transform.position = previewTransform + attribute;
        var rb = heldObject.GetComponent<Rigidbody>();
        heldObject.transform.SetParent(heldObject.originalParent, true);
        heldObject.GetComponent<Collider>().enabled = true;

        heldObject.isHeld = false;
        heldObject = null;
        TurnOnPhysics(rb);
        heldObject = null;
        HidePreview();
    }

    void HidePreview()
    {
        previewGreen.SetActive(false);
        previewRed.SetActive(false);
    }
}
