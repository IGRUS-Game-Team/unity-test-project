using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class InputContextRouter : MonoBehaviour
{
    private void Start()
    {
        InterActionController.Instance.OnClick += HandleInteract;
    }


    private void HandleInteract()
    {
        GameObject hitObj = RaycastDetector.Instance.HitObject;
        if (hitObj == null)
        {
            Debug.Log("히트된 오브젝트 없음");
            return;
        }
        string tag = hitObj.tag;

        switch (tag)
        {
            case "Box":
                if (BoxInteractionManager.Instance != null)
                {
                    BoxInteractionManager.Instance.Activate();
                    BoxInteractionManager.Instance.Interact();
                }
                break;

            case "Monitor":
                if (MonitorInteractionManager.Instance != null)
                {
                    Debug.Log("hi");
                    MonitorInteractionManager.Instance.Interact();
                }
                break;

            // case "Kiosk":
            //     if (KioskInteractionManager.Instance != null)
            //     {
            //         //KioskInteractionManager.Instance.Interact();
            //     }
            //     break;
            default:
                Debug.Log($"상호작용 가능한 태그가 아님: {tag}");
                break;
        }
        // if (BoxInteractionManager.Instace.IsActive)
        // {
        //     //BoxInteractionManager.Instace.Interact();
        // }
        // else if (MonitorInteractionManager.Instace.IsActive)
        // {
        //     //MonitorInteractionManager.Instace.Interact();
        // }
        // else if (KioskInteractionManager.Instace.IsActive)
        // {
        //     //KioskInteractionManager.Instace.Interact();
        // }
        // else
        // {
        //     Debug.Log("상호작용 대상이 없음");
        // }
    }
}
