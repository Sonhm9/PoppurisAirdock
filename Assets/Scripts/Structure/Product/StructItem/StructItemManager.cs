using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StructItemManager : MonoBehaviour
{
    private static object _lock = new object();
    private object itemLock = new object();

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
    private void Awake()
    {
        _instance = this;

        // ������ ����, �ǹ� ���� �ʱ�ȭ
        InitializeItemCounts();
        InitializestructLevel();
    }
    private void OnDestroy()
    {
        applicationQuitting = true;
    }

    public enum ItemType
    {
        WOODBONE,
        LOWENGINE,
        WOODBODY
    }

    public Dictionary<ItemType, int> itemCount = new Dictionary<ItemType, int>(); // ������ ����
    public Dictionary<ItemType, int> structLevel = new Dictionary<ItemType, int>(); // �ǹ� ����


    public event Action<ItemType, int> OnItemChanged;

    //public bool itemConsumeSuccess;

    // �������� �����ϴ� �Լ�
    public void ProduceItem(ItemType itemType, int amount)
    {
        // ����� ������ ���� ����
        itemCount[itemType] += amount;
        OnItemChanged?.Invoke(itemType, amount);
        Debug.Log(itemCount);
        //Debug.Log(itemType + " ������ " + amount + "�� ����! ���� ����: " + itemCount[itemType]);
    }

    // �������� �Ҹ��ϴ� �Լ�
    public void ConsumeItem(ItemType[] itemType, int[] amount)
    {
        if (CheckItemCount(itemType, amount))
        {
            for (int i = 0; i < itemType.Length; i++)
            {
                itemCount[itemType[i]] -= amount[i];
                OnItemChanged?.Invoke(itemType[i], amount[i]);
                Debug.Log(itemType[i] + " ������ " + amount[i] + "�� �Ҹ�! ���� ����: " + itemCount[itemType[i]]);
            }
        }        
    }

    private void InitializeItemCounts()
    {
        // ��� ������ ������ ���� �ʱ� ������ 0���� ����
        foreach (ItemType itemType in (ItemType[])Enum.GetValues(typeof(ItemType)))
        {
            itemCount[itemType] = 0;
        }
    }

    private void InitializestructLevel()
    {
        // ��� �ǹ� ������ 0���� �ʱ�ȭ
        foreach (ItemType itemType in (ItemType[])Enum.GetValues(typeof(ItemType)))
        {
            structLevel[itemType] = 0;
        }
    }

    // �������� �Ҹ��ϱ� �� Ȯ���ϴ� �Լ�
    public bool CheckItemCount(ItemType[] itemType, int[] amount)
    {
        // ������ �Ҹ𰡴� üũ
        bool itemConsumeable = false;
        for (int i = 0; i < itemType.Length; i++)
        {
            if (itemCount[itemType[i]] >= amount[i])
            {
                itemConsumeable = true;
            }
            else itemConsumeable = false;

            if (!itemConsumeable)
            {
                break;
            }
        }
        if (!itemConsumeable)
        {
            return false;
        }
        else return true;

    }
}
