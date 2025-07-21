using UnityEngine;


public class BoxSpawner : MonoBehaviour
{
    [SerializeField] GameObject deilveryBox;
    Vector3 currentPosition;
    Vector3 spawnPoint;
    float randomDropRange;
    int spawnHeight = 7;

    const string PLAYER = "Player";
    private void Awake()
    {
        currentPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER))
        {
            BoxDrop();
        }
    }

    private void BoxDrop()
    {
        randomDropRange = Random.Range(0f,3f);
        spawnPoint = new Vector3(currentPosition.x+randomDropRange, currentPosition.y+spawnHeight, currentPosition.z);
        Instantiate(deilveryBox, spawnPoint, Quaternion.identity);
    }
}
