using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ShopUIController.cs 박정민
/// 아이템의 DB 가져와서 DB의 있는 아이템 수 만큼 아이템 카드 복사해서 ScrollView에 넣는 클래스
/// </summary>
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