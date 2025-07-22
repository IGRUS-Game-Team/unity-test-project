using UnityEngine;
using System.Collections;

public class ClickController : MonoBehaviour
{
    [SerializeField] Animator openAnimation;
    [SerializeField] ParticleSystem openParticle;
    [SerializeField] GameObject deliveryBox;

    [SerializeField] float PadeoutTime= 0.25f;

    //������
    private bool isProcessing = false;

    //���ڸ� Ŭ���ϸ� �ִϸ��̼��� 3�� �ݺ��ǰ�, ��ƼŬ�� �����鼭 ���ڰ� ������� ����


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
            Debug.Log("�¾Ҿ�");

            if (hit.collider.CompareTag("Box"))
            {
                Debug.Log("box �¾�");

                
                GameObject clickedBox = hit.collider.gameObject; //ray ���� �ڽ� ����
                StartCoroutine(InteractionBox(clickedBox));
            }
        }
    }

    IEnumerator InteractionBox(GameObject targetBox)
    {
        Debug.Log("�ڷ�ƾ ����");
        isProcessing = true;

        // Ŭ���� �ڽ� ������Ʈ ��������
        Animator targetAnimator = targetBox.GetComponent<Animator>();
        ParticleSystem targetParticle = targetBox.GetComponentInChildren<ParticleSystem>();//��ƼŬ�� �ڽĿ� �� ����

        Debug.Log("�ִϸ��̼� ����");
        targetAnimator.Play("BoxOpening", 0, 0);
        yield return new WaitForSeconds(0.5f);

        Debug.Log("��ƼŬ ���");
        targetParticle.Play();

        yield return new WaitForSeconds(PadeoutTime);
        Destroy(targetBox); 

        isProcessing = false;
    }
}