using UnityEngine;

public class RaycastDetector : MonoBehaviour
{
    public static RaycastDetector Instance { get; private set; }
    public GameObject HitObject { get; private set; }

    [SerializeField] float range = 5f;
    [SerializeField] LayerMask layer;

    private void Awake() => Instance = this;

    void Update()
    {
        HitObject = null;

        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, range, layer))
        {
            HitObject = hit.collider.gameObject;
            Debug.DrawRay(ray.origin, ray.direction * range, Color.blue);
        }
    }
}

