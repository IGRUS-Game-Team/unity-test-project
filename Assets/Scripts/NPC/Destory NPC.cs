using UnityEngine;

public class DestoryNPC : MonoBehaviour
{
    const string NPC_STRING = "NPC";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(NPC_STRING))
        {
            Destroy(other.gameObject);
        }
    }
}
