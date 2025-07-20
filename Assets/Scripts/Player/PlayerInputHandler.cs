// using UnityEngine;

// public class PlayerInputHandler : MonoBehaviour
// {
//     PlayerInput
//     public event System.Action OnSelectEvent;
//     public event System.Action OnDropEvent;
//     public event System.Action OnSetEvent;

//     void Awake()
//     {
//         starterAssets = new StarterAssets();

//         // Player �׼Ǹ��� Select �׼ǿ� �ݹ� ���
//         starterAssets.Player.select.performed += ctx => OnSelect();
//         starterAssets.Player.drop.performed += ctx => OnDrop();
//     }

//     void OnEnable()
//     {
//         starterAssets.Enable();
//     }

//     void OnDisable()
//     {
//         starterAssets.Disable();
//     }

//     private void OnSelect()
//     {
//         Debug.Log("Select �׼��� ȣ��Ǿ����ϴ�!");
//         OnSelectEvent?.Invoke();
//         // ���ϴ� ����: ���� ��� Raycast�� ������Ʈ Ŭ�� ó��
//     }
//     private void OnDrop()
//     {
//         Debug.Log("Drop �׼��� ȣ��Ǿ����ϴ�!");
//         OnDropEvent?.Invoke();
//     }
//     private void OnSet()
//     {
//         Debug.Log("Set �׼��� ȣ��Ǿ����ϴ�!");
//         OnSetEvent?.Invoke();
//     }
// }
