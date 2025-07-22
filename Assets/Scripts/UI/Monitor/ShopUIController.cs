using System.Collections.Generic;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] ItemDatabase database;
    [SerializeField] Transform contentRoot;          // ScrollView/Viewport/Content
    [SerializeField] ItemCardView itemCardPrefab;    // 프리팹

    
    List<ItemCardView> spawnedCards = new();

    void Start()
    {
        Populate();
    }

    public void Populate()
    {
        Clear();

        foreach (var item in database.items)
        {
            var card = Instantiate(itemCardPrefab, contentRoot);
            card.Setup(item);
            spawnedCards.Add(card);
        }
    }

    void Clear()
    {
        foreach (var c in spawnedCards)
            Destroy(c.gameObject);
        spawnedCards.Clear();
    }
}