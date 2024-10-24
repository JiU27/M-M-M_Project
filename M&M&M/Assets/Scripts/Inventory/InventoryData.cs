using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "InventoryData", menuName = "RustyLake/InventoryData")]
public class InventoryData : ScriptableObject
{
    // 所有可用物品的列表
    public List<Item> availableItems = new List<Item>();

    // 通过ID查找物品
    public Item GetItemById(string id)
    {
        return availableItems.Find(item => item.id == id);
    }

    // 检查物品ID是否存在
    public bool HasItem(string id)
    {
        return availableItems.Exists(item => item.id == id);
    }

    // 添加新物品到可用列表
    public void AddAvailableItem(Item item)
    {
        if (!HasItem(item.id))
        {
            availableItems.Add(item);
        }
    }

    // 从可用列表移除物品
    public void RemoveAvailableItem(string id)
    {
        availableItems.RemoveAll(item => item.id == id);
    }
}