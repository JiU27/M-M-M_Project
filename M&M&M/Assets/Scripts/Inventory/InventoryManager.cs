using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    // ����ʵ��
    public static InventoryManager Instance { get; private set; }

    // Inspector ����
    [SerializeField] private GameObject inventorySlotPrefab;    // ��Ʒ��Ԥ����
    [SerializeField] private Transform inventoryContainer;      // ��Ʒ�۵ĸ�����
    [SerializeField] private float slotSpacing = 5f;           // ��Ʒ��֮��ļ��
    [SerializeField] private Color selectedColor = Color.yellow;// ѡ��ʱ����ɫ
    [SerializeField] private Color normalColor = Color.white;   // ����״̬����ɫ
    [SerializeField] private InventoryData inventoryData;      // ��Ʒ��������

    // ˽�б���
    private List<Item> inventory = new List<Item>();           // ��ǰ�ֿ��е���Ʒ
    private Item selectedItem;                                 // ��ǰѡ�е���Ʒ
    private Dictionary<Item, GameObject> itemSlots = new Dictionary<Item, GameObject>(); // ��Ʒ��UI�۵Ķ�Ӧ��ϵ

    private void Awake()
    {
        // ����ģʽ��ʼ��
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ȷ�����б�Ҫ�����������
        if (inventorySlotPrefab == null || inventoryContainer == null)
        {
            Debug.LogError("InventoryManager: Missing required components!");
            return;
        }
    }

    // �����Ʒ���ֿ�
    public void AddItem(Item item)
    {
        if (!inventory.Contains(item))
        {
            inventory.Add(item);
            CreateItemSlot(item);
        }
    }

    // ������Ʒ��UI
    private void CreateItemSlot(Item item)
    {
        GameObject slot = Instantiate(inventorySlotPrefab, inventoryContainer);
        Image slotImage = slot.GetComponent<Image>();
        slotImage.sprite = item.sprite;
        slotImage.color = normalColor;

        Button slotButton = slot.GetComponent<Button>();
        slotButton.onClick.AddListener(() => SelectItem(item));

        itemSlots[item] = slot;
        UpdateSlotPositions();
    }

    // ����������Ʒ�۵�λ��
    private void UpdateSlotPositions()
    {
        float startX = -(inventory.Count - 1) * (slotSpacing + inventorySlotPrefab.GetComponent<RectTransform>().sizeDelta.x) / 2f;

        for (int i = 0; i < inventory.Count; i++)
        {
            RectTransform slotRect = itemSlots[inventory[i]].GetComponent<RectTransform>();
            slotRect.anchoredPosition = new Vector2(
                startX + i * (slotSpacing + slotRect.sizeDelta.x),
                0f
            );
        }
    }

    // ѡ����Ʒ
    public void SelectItem(Item item)
    {
        // ��������ѡ�е���Ʒ��ȡ��ѡ��
        if (selectedItem == item)
        {
            selectedItem = null;
            itemSlots[item].GetComponent<Image>().color = normalColor;
        }
        else
        {
            // ȡ��֮ǰѡ�е���Ʒ�ĸ���
            if (selectedItem != null && itemSlots.ContainsKey(selectedItem))
            {
                itemSlots[selectedItem].GetComponent<Image>().color = normalColor;
            }

            // ѡ������Ʒ
            selectedItem = item;
            itemSlots[item].GetComponent<Image>().color = selectedColor;
        }
    }

    // ʹ�õ�ǰѡ�е���Ʒ
    public void UseSelectedItem()
    {
        if (selectedItem != null)
        {
            RemoveItem(selectedItem);
            selectedItem = null;
        }
    }

    // �Ӳֿ����Ƴ���Ʒ
    private void RemoveItem(Item item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            Destroy(itemSlots[item]);
            itemSlots.Remove(item);
            UpdateSlotPositions();
        }
    }

    // ��ȡ��ǰѡ�е���Ʒ
    public Item GetSelectedItem()
    {
        return selectedItem;
    }

    // ����Ƿ����ض�ID����Ʒ
    public bool HasItem(string itemId)
    {
        return inventory.Exists(item => item.id == itemId);
    }

    // ��ȡ�ֿ��е�������Ʒ
    public List<Item> GetAllItems()
    {
        return new List<Item>(inventory);
    }

    // ��ղֿ�
    public void ClearInventory()
    {
        foreach (GameObject slot in itemSlots.Values)
        {
            Destroy(slot);
        }

        inventory.Clear();
        itemSlots.Clear();
        selectedItem = null;
    }
}