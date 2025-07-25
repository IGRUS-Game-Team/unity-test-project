using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private List<BlockOutLiner> allBlocks = new List<BlockOutLiner>();
    [SerializeField] public float pickupRange = 5f;
    [SerializeField] public LayerMask pickupLayer;
    void Start()
    {
        BlockOutLiner[] found = FindObjectsOfType<BlockOutLiner>();
        allBlocks.AddRange(found);
    }

    void Update()
    {
        foreach (var block in allBlocks)
        {
            block.selected = false;
        }
        
        GameObject obj = RaycastDetector.Instance.HitObject;
        if (obj == null) return;

        BlockOutLiner hitBlock = obj.GetComponent<BlockOutLiner>();
        if (hitBlock != null)
        {
            hitBlock.selected = true;
        }
    }
}

