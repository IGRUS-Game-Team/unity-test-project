using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlyaerObjectSelector : MonoBehaviour
{
    public Transform holdPoint;  // ī�޶� �Ʒ� HoldPoint�� Inspector���� ����
    private bool isHolding = false;

    private BlockIsHolding heldObject;
    
    void Start()
    {
        PlayerInputHandler inputHandler = FindFirstObjectByType<PlayerInputHandler>();
        inputHandler.OnSelectEvent += TryPickupObject;
        inputHandler.OnDropEvent += DropPickupObject;
    }
    void Update()
    {
       if (Input.GetMouseButtonDown(0))
       {
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           if (Physics.Raycast(ray, out RaycastHit hit, 5f))
           {
               BlockIsHolding pickup = hit.transform.GetComponent<BlockIsHolding>();
               if (pickup != null)
               {
                   //TurnOffPhysics(pickup); // ���� ���ֱ�(����� �� ��鸮�� �ʵ���)
                   pickup.transform.SetParent(holdPoint);               // ī�޶� �ڽ��� HoldPoint�� ����
                   pickup.transform.localPosition = Vector3.zero;      // HoldPoint ��ġ�� ����
                   pickup.transform.localRotation = Quaternion.identity; // ȸ�� �ʱ�ȭ
                   //pickup.isHeld = true;
               }
           }
       }
    }
    private void TryPickupObject()
    {
        if (isHolding) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            BlockIsHolding pickup = hit.transform.GetComponent<BlockIsHolding>();
            if (pickup != null)
            {
                TurnOffPhysics(pickup);
                pickup.transform.SetParent(holdPoint);
                pickup.transform.localPosition = Vector3.zero;
                pickup.transform.localRotation = Quaternion.identity;
                pickup.isHeld = true;

                heldObject = pickup;
                isHolding = true;
            }
        }

    }
    private void DropPickupObject()
    {
        if (!isHolding || heldObject == null) return;
        TurnOnPhysics(heldObject);

        heldObject.transform.SetParent(null); //�θ𿡼� �и�

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        rb.AddForce(Camera.main.transform.forward * 10f, ForceMode.Impulse); //ī�޶� ���� �������� �о��ֱ�

        heldObject.isHeld = false;
        isHolding = false;
        heldObject = null;

    }

    private void TurnOffPhysics(BlockIsHolding pickup)
    {
        pickup.GetComponent<Rigidbody>().detectCollisions = false; // �浹 ���� ����
        pickup.GetComponent<Rigidbody>().isKinematic = true;       // ������ ��� ����
        pickup.GetComponent<Rigidbody>().useGravity = false;       // �߷� ���� �� ����
        pickup.GetComponent<Collider>().enabled = false; // �浹 ��ü ��Ȱ��ȭ
    }
    private void TurnOnPhysics(BlockIsHolding pickup)
    {
        pickup.GetComponent<Rigidbody>().detectCollisions = true; 
        pickup.GetComponent<Rigidbody>().isKinematic = false;       
        pickup.GetComponent<Rigidbody>().useGravity = true;       
        pickup.GetComponent<Collider>().enabled = true; 
    }
}
