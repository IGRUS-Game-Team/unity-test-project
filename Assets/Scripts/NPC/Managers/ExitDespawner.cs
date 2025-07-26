using UnityEngine;

public class ExitDespawner : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            Destroy(other.gameObject);  
        }
    }
}