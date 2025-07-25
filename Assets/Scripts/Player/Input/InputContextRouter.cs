using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class InputContextRouter : MonoBehaviour
{
    private void Start()
    {
        //InterActionController.Instance.OnClick += HandleInteract;
    }

    // void Update()
    // {

    //     if (hit != null && hit.CompareTag("Box"))
    //     {
    //         BoxInteractionManager.Instace.Activate();
    //     }
    //     else
    //     {
    //         BoxInteractionManager.Instace.Deactivate();
    //     }

    //     // 다른 대상들 (모니터, 키오스크 등)도 여기에 추가 가능
    // }

    private void HandleInteract()
    {
        if (BoxInteractionManager.Instace.IsActive)
        {
            //BoxInteractionManager.Instace.Interact();
        }
        else if (MonitorInteractionManager.Instace.IsActive)
        {
            //MonitorInteractionManager.Instace.Interact();
        }
        else if (KioskInteractionManager.Instace.IsActive)
        {
            //KioskInteractionManager.Instace.Interact();
        }
        else
        {
            Debug.Log("상호작용 대상이 없음");
        }
    }
}
