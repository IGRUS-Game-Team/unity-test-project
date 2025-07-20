using System.Collections.Generic;
using UnityEngine;


public class PlayerPickUpController : MonoBehaviour
{
    private RaycastHit hit;
    private List<BlockOutLiner> allBlocks = new List<BlockOutLiner>();

    void Start()
    {
        BlockOutLiner[] foundBlocks = FindObjectsOfType<BlockOutLiner>();
        allBlocks.AddRange(foundBlocks);
    }

    void Update()
    {
        if (Camera.main == null)
        {
            Debug.LogError("MainCamera!");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

     
        foreach (BlockOutLiner block in allBlocks)
        {
            block.selected = false; 
        }

        
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue);

            BlockOutLiner block = hit.transform.GetComponent<BlockOutLiner>();
            if (block != null)
            {
                block.selected = true; 
                Debug.Log(block.name);
            }
        }
    }
}
