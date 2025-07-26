using UnityEngine;


public class BoxSpawner : MonoBehaviour
{
    [SerializeField] GameObject deilveryBox;
    [SerializeField] SelectionManager selectionManager;
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
        Debug.Log("트리거 닿음" + other.tag);

        if (other.CompareTag(PLAYER))
        {
            BoxDrop();
        }
    }

    private void BoxDrop()
    {
        Debug.Log("소환.");

        randomDropRange = Random.Range(0f, 3f);
        spawnPoint = new Vector3(currentPosition.x + randomDropRange, currentPosition.y + spawnHeight, currentPosition.z);
        var newBlock = Instantiate(deilveryBox, spawnPoint, Quaternion.identity);
        selectionManager.allBlocks.Add(newBlock.GetComponent<BlockOutLiner>());
    }
}
