using UnityEngine;

public class DestoryNPC : MonoBehaviour
{
    const string NPC_STRING = "Npc";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(NPC_STRING))
        {
            Destroy(other.gameObject);
        }
    }
}
