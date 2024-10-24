using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] protected string[] acceptableItemIds;  // 可以与该对象交互的物品ID数组
    [SerializeField] protected bool destroyAfterUse = true; // 使用后是否销毁物品

    // 交互事件
    public UnityEvent<Item> onInteractionSuccess;    // 交互成功时触发
    public UnityEvent onInteractionFailed;           // 交互失败时触发

    // 鼠标点击检测
    protected virtual void OnMouseDown()
    {
        HandleInteraction();
    }

    // 处理交互逻辑
    protected virtual void HandleInteraction()
    {
        Item selectedItem = InventoryManager.Instance.GetSelectedItem();

        if (selectedItem != null)
        {
            if (CanInteractWith(selectedItem))
            {
                OnInteract(selectedItem);
                onInteractionSuccess?.Invoke(selectedItem);

                if (destroyAfterUse)
                {
                    InventoryManager.Instance.UseSelectedItem();
                }
            }
            else
            {
                OnInteractionFailed(selectedItem);
                onInteractionFailed?.Invoke();
            }
        }
    }

    // 检查是否可以与物品交互
    protected virtual bool CanInteractWith(Item item)
    {
        return System.Array.Exists(acceptableItemIds, id => id == item.id);
    }

    // 具体的交互逻辑（由子类重写）
    protected virtual void OnInteract(Item item)
    {
        Debug.Log($"Interacted with {gameObject.name} using {item.itemName}");
    }

    // 交互失败时的处理（可由子类重写）
    protected virtual void OnInteractionFailed(Item item)
    {
        Debug.Log($"Cannot interact with {gameObject.name} using {item.itemName}");
    }

    // 高亮显示（可选）
    protected virtual void OnMouseEnter()
    {
        if (InventoryManager.Instance.GetSelectedItem() != null)
        {
            // 实现高亮效果
        }
    }

    protected virtual void OnMouseExit()
    {
        // 取消高亮效果
    }
}