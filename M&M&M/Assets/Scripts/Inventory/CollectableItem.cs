// CollectableItem.cs - 可收集物品脚本
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectableItem : MonoBehaviour
{
    [Header("物品信息")]
    [SerializeField] private Item itemData;              // 对应的物品数据
    [SerializeField] private SpriteRenderer itemSprite;  // 物品的精灵渲染器
    [SerializeField] private bool destroyOnCollect = true; // 收集后是否销毁物体

    [Header("交互设置")]
    [SerializeField] private float hoverScaleMultiplier = 1.1f; // 悬停时的放大倍数
    [SerializeField] private Color hoverColor = Color.yellow;    // 悬停时的颜色

    [Header("事件")]
    public UnityEvent<Item> onCollected;    // 物品被收集时触发的事件

    private Color originalColor;
    private Vector3 originalScale;
    private bool isCollected = false;

    private void Start()
    {
        if (itemSprite == null)
            itemSprite = GetComponent<SpriteRenderer>();

        originalColor = itemSprite.color;
        originalScale = transform.localScale;
    }

    private void OnMouseEnter()
    {
        if (!isCollected)
        {
            // 悬停效果
            transform.localScale = originalScale * hoverScaleMultiplier;
            itemSprite.color = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        if (!isCollected)
        {
            // 恢复正常外观
            transform.localScale = originalScale;
            itemSprite.color = originalColor;
        }
    }

    private void OnMouseDown()
    {
        if (!isCollected)
        {
            CollectItem();
        }
    }

    public void CollectItem()
    {
        if (!isCollected && itemData != null)
        {
            isCollected = true;

            // 将物品添加到仓库
            InventoryManager.Instance.AddItem(itemData);

            // 触发收集事件
            onCollected?.Invoke(itemData);

            // 播放收集动画
            StartCoroutine(CollectAnimation());
        }
    }

    private System.Collections.IEnumerator CollectAnimation()
    {
        // 简单的收集动画
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 缩小并上浮
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            transform.position = Vector3.Lerp(startPos, startPos + Vector3.up * 0.5f, t);

            yield return null;
        }

        if (destroyOnCollect)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}