using UnityEngine;

[System.Serializable]
public class Item
{
    // 物品的基本属性
    public string id;                    // 物品的唯一标识符
    public string itemName;              // 物品的显示名称
    public Sprite sprite;                // 物品的图片
    public string[] interactableObjects; // 可以与该物品交互的对象标签
    public string description;           // 物品描述（可选）

    // 构造函数
    public Item(string id, string itemName, Sprite sprite)
    {
        this.id = id;
        this.itemName = itemName;
        this.sprite = sprite;
    }
}