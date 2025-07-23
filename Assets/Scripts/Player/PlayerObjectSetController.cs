using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerObjectSetController : MonoBehaviour
{
    [SerializeField] GameObject previewGreen;
    [SerializeField] GameObject previewRed;
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

    }
    void Start()
    {
        InterActionController.Instance.OnDrop += PlaceObject;
    }

    // void Update()
    // {
    //     heldObject = playerObjectHoldController.heldObject;
    //     if (heldObject == null && input.set == true) input.SetInput(false);
    //     if (isHolding)
    //     {

    //         ShowPlacementPreview();
    //     }
    //     else
    //     {
    //         HidePreview();
    //     }


    //     if (input.set && isHolding && canPlace)
    //     {
    //         Debug.Log("병신");
    //         PlaceObject();
    //         input.SetInput(false);
    //         HidePreview();
    //     }
    //     else input.SetInput(false);
    // }
    void Update()
    {
        heldObject = playerObjectHoldController.heldObject;

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
            bool valid = hit.collider.CompareTag("SettableSurface");
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
        if (heldObject == null) return;
        Debug.Log("a~~~~~~~~~~~~~~~~~~~~~~");
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
        playerObjectHoldController.ReleaseHeldObject(); 
        HidePreview();
    }

    void HidePreview()
    {
        previewGreen.SetActive(false);
        previewRed.SetActive(false);
    }

}
