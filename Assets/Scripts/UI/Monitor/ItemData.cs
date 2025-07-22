using UnityEngine;

[CreateAssetMenu(menuName="Shop/Item")]
public class ItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite icon;
    public float unitPrice;
    public ItemCategory category;
    public DisplayType displayType;
}