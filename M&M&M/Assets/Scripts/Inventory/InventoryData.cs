using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "InventoryData", menuName = "RustyLake/InventoryData")]
public class InventoryData : ScriptableObject
{
    // ���п�����Ʒ���б�
    public List<Item> availableItems = new List<Item>();

    // ͨ��ID������Ʒ
    public Item GetItemById(string id)
    {
        return availableItems.Find(item => item.id == id);
    }

    // �����ƷID�Ƿ����
    public bool HasItem(string id)
    {
        return availableItems.Exists(item => item.id == id);
    }

    // �������Ʒ�������б�
    public void AddAvailableItem(Item item)
    {
        if (!HasItem(item.id))
        {
            availableItems.Add(item);
        }
    }

    // �ӿ����б��Ƴ���Ʒ
    public void RemoveAvailableItem(string id)
    {
        availableItems.RemoveAll(item => item.id == id);
    }
}