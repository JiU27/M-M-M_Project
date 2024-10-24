// CollectableItem.cs - ���ռ���Ʒ�ű�
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectableItem : MonoBehaviour
{
    [Header("��Ʒ��Ϣ")]
    [SerializeField] private Item itemData;              // ��Ӧ����Ʒ����
    [SerializeField] private SpriteRenderer itemSprite;  // ��Ʒ�ľ�����Ⱦ��
    [SerializeField] private bool destroyOnCollect = true; // �ռ����Ƿ���������

    [Header("��������")]
    [SerializeField] private float hoverScaleMultiplier = 1.1f; // ��ͣʱ�ķŴ���
    [SerializeField] private Color hoverColor = Color.yellow;    // ��ͣʱ����ɫ

    [Header("�¼�")]
    public UnityEvent<Item> onCollected;    // ��Ʒ���ռ�ʱ�������¼�

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
            // ��ͣЧ��
            transform.localScale = originalScale * hoverScaleMultiplier;
            itemSprite.color = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        if (!isCollected)
        {
            // �ָ��������
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

            // ����Ʒ��ӵ��ֿ�
            InventoryManager.Instance.AddItem(itemData);

            // �����ռ��¼�
            onCollected?.Invoke(itemData);

            // �����ռ�����
            StartCoroutine(CollectAnimation());
        }
    }

    private System.Collections.IEnumerator CollectAnimation()
    {
        // �򵥵��ռ�����
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // ��С���ϸ�
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