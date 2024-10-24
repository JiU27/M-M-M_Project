using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] protected string[] acceptableItemIds;  // ������ö��󽻻�����ƷID����
    [SerializeField] protected bool destroyAfterUse = true; // ʹ�ú��Ƿ�������Ʒ

    // �����¼�
    public UnityEvent<Item> onInteractionSuccess;    // �����ɹ�ʱ����
    public UnityEvent onInteractionFailed;           // ����ʧ��ʱ����

    // ��������
    protected virtual void OnMouseDown()
    {
        HandleInteraction();
    }

    // �������߼�
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

    // ����Ƿ��������Ʒ����
    protected virtual bool CanInteractWith(Item item)
    {
        return System.Array.Exists(acceptableItemIds, id => id == item.id);
    }

    // ����Ľ����߼�����������д��
    protected virtual void OnInteract(Item item)
    {
        Debug.Log($"Interacted with {gameObject.name} using {item.itemName}");
    }

    // ����ʧ��ʱ�Ĵ�������������д��
    protected virtual void OnInteractionFailed(Item item)
    {
        Debug.Log($"Cannot interact with {gameObject.name} using {item.itemName}");
    }

    // ������ʾ����ѡ��
    protected virtual void OnMouseEnter()
    {
        if (InventoryManager.Instance.GetSelectedItem() != null)
        {
            // ʵ�ָ���Ч��
        }
    }

    protected virtual void OnMouseExit()
    {
        // ȡ������Ч��
    }
}