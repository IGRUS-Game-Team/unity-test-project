using UnityEngine;
using System.Collections.Generic;

public class BlockOutLiner : MonoBehaviour
{
    private bool lastSelectedState = false;
    [SerializeField] public bool selected = false;
    [SerializeField] Material sharedOutlineMaterial;

    Renderer[] renderers;

    void Awake()
    {
        if (sharedOutlineMaterial == null)
            Debug.LogError("인스펙터에 머터리얼 등록 안됨");
    }

    void Start()
    {
        // 자신 및 자식 전체에서 Renderer 컴포넌트 가져옴
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        if (selected != lastSelectedState)
        {
            foreach (var r in renderers)
            {
                if (selected)
                {
                    var mats = r.sharedMaterials;
                    var newMats = new Material[mats.Length + 1];
                    mats.CopyTo(newMats, 0);
                    newMats[mats.Length] = sharedOutlineMaterial;
                    r.materials = newMats;
                }
                else
                {
                    var mats = new List<Material>(r.sharedMaterials);
                    mats.Remove(sharedOutlineMaterial);
                    r.materials = mats.ToArray();
                }
            }

            lastSelectedState = selected;
        }
    }
}
