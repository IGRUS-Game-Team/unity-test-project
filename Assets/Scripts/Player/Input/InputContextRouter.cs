using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

//InputContextRouter.cs 박정민
//오브젝트마다 한 키(ex click이 박스에서는 Hold로, 모니터에서는 상호작용으로, UI에서는 버튼의 이벤트 작동으로 적용)가
//다르게 쓰이는걸 나누기 위해 만든 클래스
//빈오브젝트에 붙임

public class InputContextRouter : MonoBehaviour
{
    private void Start()
    {
        UIModeState.IsInUIMode = false; //UIModeState.cs 의 bool 값, 구현시 시간 단축 위해 일단 static class로 정의함.
                                        //버그가 상당히 많음. 모니터 인터렉션을 한 후 게임을 끄면 bool값이 안바뀌어 다시 게임 실행시
                                        //게임이 비정상적으로 동작. 이를 해결위해 어떤 class에서(기억안남 ㅈㅅ ㅎ) awake 구문에서 초기화시킴

        Debug.Log("InputContextRouter 활성화됨");
        InterActionController.Instance.OnClick += HandleInteract; //클릭동작에 이벤트 구독
    }


    private void HandleInteract() //일단 지금은 클릭만을 분기시키지만 추후 기능이 늘어감에 따라 메서드를 확장시켜야함.
    {
        GameObject hitObj = RaycastDetector.Instance.HitObject; //RaycastDetector에서 static 선언한 Instance 변수로 태그 판별

        if (hitObj == null)
        {
            Debug.Log("히트된 오브젝트 없음");
            return;
        }
        string tag = hitObj.tag;

        switch (tag) //ray쏴서 맞은 물체의 태그에 따라 클릭 기능이 분기됨.
        {
            case "Box":
                if (BoxInteractionManager.Instance != null) //Box면 박스에 해당하는 동작 실행
                {
                    BoxInteractionManager.Instance.Activate();
                    BoxInteractionManager.Instance.Interact();
                }
                break;

            case "Monitor": //모니터면 모니터에 해당하는 동작 실행
                if (MonitorInteractionManager.Instance != null)
                {
                    Debug.Log("hi");
                    MonitorInteractionManager.Instance.Interact();
                }
                break;

            // case "Kiosk": //키오스크는 구현 안해서 일단 주석처리
            //     if (KioskInteractionManager.Instance != null)
            //     {
            //         //KioskInteractionManager.Instance.Interact();
            //     }
            //     break;
            default:
                Debug.Log($"상호작용 가능한 태그가 아님: {tag}");
                break;
        }

    }
}
