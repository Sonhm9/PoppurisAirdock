using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StructItemManager : MonoBehaviour
{
    private static object _lock = new object();

    private static StructItemManager _instance = null;
    public static StructItemManager instance
    {
        get
        {
            if (applicationQuitting)
            {
                return null;
            }
            lock (_lock)
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("StructItemManager ");
                    obj.AddComponent<StructItemManager>();
                    _instance = obj.GetComponent<StructItemManager>();
                }
                return _instance;
            }
        }
        set
        {
            _instance = value;
        }
    }
    private static bool applicationQuitting = false;
    // �̱���

    public enum ItemType
    {
        WOODBONE,
        LOWENGINE,
        WOODBODY
    }
    public Dictionary<ItemType, int> itemCount = new Dictionary<ItemType, int>();

    public event Action<ItemType, int> OnItemChanged;

    public bool itemConsumeSuccess;

    private void Awake()
    {
        _instance = this;
        InitializeItemCounts();
    }
    private void OnDestroy()
    {
        applicationQuitting = true;
    }

    private void InitializeItemCounts()
    {
        // ��� ������ ������ ���� �ʱ� ������ 0���� ����
        foreach (ItemType itemType in (ItemType[])Enum.GetValues(typeof(ItemType)))
        {
            itemCount[itemType] = 0;
        }
    }
    // �������� �����ϴ� �Լ�
    public void ProduceItem(ItemType itemType, int amount)
    {
        // ����� ������ ���� ����
        itemCount[itemType] += amount;
        OnItemChanged?.Invoke(itemType, amount);
        //Debug.Log(itemType + " ������ " + amount + "�� ����! ���� ����: " + itemCount[itemType]);
    }

    // �������� �Ҹ��ϴ� �Լ�
    public void ConsumeItem(ItemType itemType, int amount)
    {
        // ������ ������ 0 �̻��� ��쿡�� �Ҹ� ����
        if (itemConsumeSuccess)
        {
            // ������ ���� ����
            itemCount[itemType] -= amount;
            OnItemChanged?.Invoke(itemType, amount);

            Debug.Log(itemType + " ������ " + amount + "�� �Ҹ�! ���� ����: " + itemCount[itemType]);
        }
        else
        {
            Debug.Log(itemType + " �������� �����մϴ�!");
        }
    }

    // �������� �Ҹ��ϱ� �� Ȯ���ϴ� �Լ�
    public void CheckItemCount(ItemType itemType, int amount)
    {
        if (itemCount[itemType] >= amount)
        {
            itemConsumeSuccess = true;
        }
        else itemConsumeSuccess = false;

    }
}
