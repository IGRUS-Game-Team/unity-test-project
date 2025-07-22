using UnityEngine;
using System.Collections;

public class ClickController : MonoBehaviour
{
    [SerializeField] Animator openAnimation;
    [SerializeField] ParticleSystem openParticle;
    [SerializeField] GameObject deliveryBox;

    [SerializeField] float PadeoutTime= 0.25f;

    //동작중
    private bool isProcessing = false;

    //상자를 클릭하면 애니메이션이 3번 반복되고, 파티클이 나오면서 상자가 사라지는 로직


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isProcessing)
        {
            Click();
        }
    }

    void Click()
    {
        RaycastHit hit;
        Vector3 rayDirection = Camera.main.transform.forward;

        bool rayHit = Physics.Raycast(Camera.main.transform.position, rayDirection, out hit, 10);
        if (rayHit)
        {
            Debug.Log("맞았어");

            if (hit.collider.CompareTag("Box"))
            {
                Debug.Log("box 맞아");

                
                GameObject clickedBox = hit.collider.gameObject; //ray 맞은 박스 선택
                StartCoroutine(InteractionBox(clickedBox));
            }
        }
    }

    IEnumerator InteractionBox(GameObject targetBox)
    {
        Debug.Log("코루틴 시작");
        isProcessing = true;

        // 클릭한 박스 컴포넌트 가져오기
        Animator targetAnimator = targetBox.GetComponent<Animator>();
        ParticleSystem targetParticle = targetBox.GetComponentInChildren<ParticleSystem>();//파티클은 자식에 들어가 있음

        Debug.Log("애니메이션 시작");
        targetAnimator.Play("BoxOpening", 0, 0);
        yield return new WaitForSeconds(0.5f);

        Debug.Log("파티클 재생");
        targetParticle.Play();

        yield return new WaitForSeconds(PadeoutTime);
        Destroy(targetBox); 

        isProcessing = false;
    }
}