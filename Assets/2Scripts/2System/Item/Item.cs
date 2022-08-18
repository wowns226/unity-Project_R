using UnityEngine;
public enum ItemType 
{ 
    Equipment, 
    Used, 
    Default
}

[CreateAssetMenu(fileName = "DefaultItem", menuName = "Assets/Items/Default Item")]
public class Item : ScriptableObject
{
    [SerializeField] private int id;
    public int ID { get { return id; } }
    public string itemName;
    public Sprite itemImage;
    public GameObject itemPrefabs;
    [TextArea(5, 10)]
    public string itemDescription;
    public ItemType itemType;
    public int itemCost;
}