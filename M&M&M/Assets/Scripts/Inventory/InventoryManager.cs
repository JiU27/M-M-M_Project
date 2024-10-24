using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    // 单例实例
    public static InventoryManager Instance { get; private set; }

    // Inspector 引用
    [SerializeField] private GameObject inventorySlotPrefab;    // 物品槽预制体
    [SerializeField] private Transform inventoryContainer;      // 物品槽的父对象
    [SerializeField] private float slotSpacing = 5f;           // 物品槽之间的间距
    [SerializeField] private Color selectedColor = Color.yellow;// 选中时的颜色
    [SerializeField] private Color normalColor = Color.white;   // 正常状态的颜色
    [SerializeField] private InventoryData inventoryData;      // 物品数据引用

    // 私有变量
    private List<Item> inventory = new List<Item>();           // 当前仓库中的物品
    private Item selectedItem;                                 // 当前选中的物品
    private Dictionary<Item, GameObject> itemSlots = new Dictionary<Item, GameObject>(); // 物品与UI槽的对应关系

    private void Awake()
    {
        // 单例模式初始化
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
        // 确保所有必要组件都已设置
        if (inventorySlotPrefab == null || inventoryContainer == null)
        {
            Debug.LogError("InventoryManager: Missing required components!");
            return;
        }
    }

    // 添加物品到仓库
    public void AddItem(Item item)
    {
        if (!inventory.Contains(item))
        {
            inventory.Add(item);
            CreateItemSlot(item);
        }
    }

    // 创建物品槽UI
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

    // 更新所有物品槽的位置
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

    // 选择物品
    public void SelectItem(Item item)
    {
        // 如果点击已选中的物品，取消选择
        if (selectedItem == item)
        {
            selectedItem = null;
            itemSlots[item].GetComponent<Image>().color = normalColor;
        }
        else
        {
            // 取消之前选中的物品的高亮
            if (selectedItem != null && itemSlots.ContainsKey(selectedItem))
            {
                itemSlots[selectedItem].GetComponent<Image>().color = normalColor;
            }

            // 选中新物品
            selectedItem = item;
            itemSlots[item].GetComponent<Image>().color = selectedColor;
        }
    }

    // 使用当前选中的物品
    public void UseSelectedItem()
    {
        if (selectedItem != null)
        {
            RemoveItem(selectedItem);
            selectedItem = null;
        }
    }

    // 从仓库中移除物品
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

    // 获取当前选中的物品
    public Item GetSelectedItem()
    {
        return selectedItem;
    }

    // 检查是否有特定ID的物品
    public bool HasItem(string itemId)
    {
        return inventory.Exists(item => item.id == itemId);
    }

    // 获取仓库中的所有物品
    public List<Item> GetAllItems()
    {
        return new List<Item>(inventory);
    }

    // 清空仓库
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