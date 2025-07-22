using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using System;
public class PlayerObjectDropController : MonoBehaviour
{
    [SerializeField] float throwForce = 10f;
    PlayerObjectHoldController playerObjectHoldController;
    private StarterAssetsInputs input;
    private BlockIsHolding heldObject;
    private bool isHolding => heldObject != null;

    void Awake()
    {
        input = GetComponent<StarterAssetsInputs>();
        playerObjectHoldController = GetComponent<PlayerObjectHoldController>();
    }

    void Update()
    {
        heldObject = playerObjectHoldController.heldObject;

        if (heldObject == null && input.drop == true) input.DropInput(false);
        if (input.drop && isHolding)
        {
            Drop();
            input.DropInput(false);
        }

    }

    private void Drop()
    {

        if (heldObject == null)
        {
            Debug.Log("좇까");
            return;
        }

        var rb = heldObject.GetComponent<Rigidbody>();

        // 원래 부모로 복원 + 위치 유지
        heldObject.transform.SetParent(heldObject.originalParent, true);
        heldObject.GetComponent<Collider>().enabled = true;

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
