using System;
using System.Threading;
using UnityEngine;

public class InputContextRouter : MonoBehaviour
{
    private void Start()
    {
        InterActionController.Instance.OnClick += HandleInteract;
    }

    private void HandleInteract()
    {
        if (BoxInteractionManager.Instace.IsActive)
        {

        }
        else if (MonitorInteractionManager.Instace.IsActive)
        {

        }
        else if (KioskInteractionManager.Instace.IsActive)
        {

        }
        else
        {
            Debug.Log("상호작용 대상이 없음");
        }
    }
}
