// using StarterAssets;
// using Unity.Android.Gradle;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerObjectHolder : MonoBehaviour
// {
//     StarterAssetsInputs starterAssetsInputs;
//     PlayerPickUpController playerPickUpController;
//     BlockIsHolding blockIsHolding;
//     [SerializeField] Transform holdPoint;
//     private BlockIsHolding heldObject;

//     private bool isHolding = false;
//     void Awake()
//     {
//         starterAssetsInputs = GetComponent<StarterAssetsInputs>();
//         playerPickUpController = GetComponent<PlayerPickUpController>();
//         blockIsHolding = GetComponent<BlockIsHolding>();
//     }

//     void Update()
//     {
//         if (starterAssetsInputs.hold)
//        {
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            if (Physics.Raycast(ray, out RaycastHit hit, 5f))
//            {
//                BlockIsHolding pickup = hit.transform.GetComponent<BlockIsHolding>();
//                if (pickup != null)
//                {
//                    TurnOffPhysics(pickup); // ���� ���ֱ�(����� �� ��鸮�� �ʵ���)
//                    pickup.transform.SetParent(holdPoint);               // ī�޶� �ڽ��� HoldPoint�� ����
//                    pickup.transform.localPosition = Vector3.zero;      // HoldPoint ��ġ�� ����
//                    pickup.transform.localRotation = Quaternion.identity; // ȸ�� �ʱ�ȭ
//                    pickup.isHeld = true;
//                }
//            }
//        }
//     }

//     // void pickup()
//     // {
//     //     if (starterAssetsInputs.hold == false) return; //좌클릭 안누른 상태면 리턴
//     //     if (blockIsHolding.isHeld == true) return; //이미 들려있는 상태면 리턴

//     //     if (starterAssetsInputs.hold == true)
//     //     {
//     //         isHolding = !isHolding;
//     //     }
//     //     if (isHolding)
//     //     {
            
//     //     }


//     // }

//     private void TryPickupObject()
//     {
//         if (isHolding) return;

//         Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
//         if (Physics.Raycast(ray, out RaycastHit hit, 5f))
//         {
//             BlockIsHolding pickup = hit.transform.GetComponent<BlockIsHolding>();
//             if (pickup != null)
//             {
//                 TurnOffPhysics(pickup);
//                 pickup.transform.SetParent(holdPoint);
//                 pickup.transform.localPosition = Vector3.zero;
//                 pickup.transform.localRotation = Quaternion.identity;
//                 pickup.isHeld = true;

//                 heldObject = pickup;
//                 isHolding = true;
//             }
//         }

//     }
//     private void DropPickupObject()
//     {
//         if (!isHolding || heldObject == null) return;
//         TurnOnPhysics(heldObject);

//         heldObject.transform.SetParent(null); 

//         Rigidbody rb = heldObject.GetComponent<Rigidbody>();
//         rb.AddForce(Camera.main.transform.forward * 10f, ForceMode.Impulse); //ī�޶� ���� �������� �о��ֱ�

//         heldObject.isHeld = false;
//         isHolding = false;
//         heldObject = null;

//     }

//     private void TurnOffPhysics(BlockIsHolding pickup)
//     {
//         pickup.GetComponent<Rigidbody>().detectCollisions = false; // �浹 ���� ����
//         pickup.GetComponent<Rigidbody>().isKinematic = true;       // ������ ��� ����
//         pickup.GetComponent<Rigidbody>().useGravity = false;       // �߷� ���� �� ����
//         pickup.GetComponent<Collider>().enabled = false; // �浹 ��ü ��Ȱ��ȭ
//     }
//     private void TurnOnPhysics(BlockIsHolding pickup)
//     {
//         pickup.GetComponent<Rigidbody>().detectCollisions = true; 
//         pickup.GetComponent<Rigidbody>().isKinematic = false;       
//         pickup.GetComponent<Rigidbody>().useGravity = true;       
//         pickup.GetComponent<Collider>().enabled = true; 
//     }
// }

using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using System;

public class PlayerObjectHolder : MonoBehaviour
{
    [SerializeField] float throwForce = 10f;
    [SerializeField] Transform holdPoint;
    [SerializeField] float pickupRange;
    [SerializeField] LayerMask pickupLayer;

    PlayerPickUpController playerPickUpController;

    private StarterAssetsInputs input;
    private BlockIsHolding heldObject;
    private bool isHolding => heldObject != null;

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
            heldObject.originalParent = target.transform.parent; // ⭐ 원래 부모 기억

            var rb = heldObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.detectCollisions = false;
            rb.useGravity = false;

            heldObject.GetComponent<Collider>().enabled = false;
            heldObject.transform.SetParent(holdPoint);
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.identity;
        }
    }

    private void Drop()
    {
        if (heldObject == null) return;

        var rb = heldObject.GetComponent<Rigidbody>();

        // 원래 부모로 복원 + 위치 유지
        heldObject.transform.SetParent(heldObject.originalParent, true);
        heldObject.GetComponent<Collider>().enabled = true;

        rb.isKinematic = false;
        rb.detectCollisions = true;
        rb.useGravity = true;

        heldObject.isHeld = false;
        heldObject = null;
        rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);

    }
}
