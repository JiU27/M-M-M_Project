using UnityEngine;

[System.Serializable]
public class Item
{
    // ��Ʒ�Ļ�������
    public string id;                    // ��Ʒ��Ψһ��ʶ��
    public string itemName;              // ��Ʒ����ʾ����
    public Sprite sprite;                // ��Ʒ��ͼƬ
    public string[] interactableObjects; // ���������Ʒ�����Ķ����ǩ
    public string description;           // ��Ʒ��������ѡ��

    // ���캯��
    public Item(string id, string itemName, Sprite sprite)
    {
        this.id = id;
        this.itemName = itemName;
        this.sprite = sprite;
    }
}