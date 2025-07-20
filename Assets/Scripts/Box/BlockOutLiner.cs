using UnityEngine;

public class BlockOutLiner : MonoBehaviour
{
    private bool lastSelectedState = false;
    [SerializeField] public bool selected = false; 
    [SerializeField] Material sharedOutlineMaterial;

    Renderer renderer;

    void Awake()
    {
        // if (sharedOutlineMaterial == null)
        // {
        //     sharedOutlineMaterial = Resources.Load<Material>("Arts/Materials/Outline");
        //     if (sharedOutlineMaterial == null)
        //         Debug.LogError("Outline Material?? Resources/Materials/Outline이 없습니다");
        // }
        if (sharedOutlineMaterial == null) Debug.LogError("인스펙터에 머터리얼 등록 안됨");
    }

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (selected != lastSelectedState)
        {
            if (selected)
            {
                Material[] mats = renderer.sharedMaterials;
                Material[] newMats = new Material[mats.Length + 1];
                mats.CopyTo(newMats, 0);
                newMats[mats.Length] = sharedOutlineMaterial;
                renderer.materials = newMats;
            }
            else
            {
                var mats = new System.Collections.Generic.List<Material>(renderer.sharedMaterials);
                mats.Remove(sharedOutlineMaterial);
                renderer.materials = mats.ToArray();
            }

            lastSelectedState = selected;
        }
    }
}
