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
    // 싱글톤
    private void Awake()
    {
        _instance = this;

        // 아이템 갯수, 건물 레벨 초기화
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

    public Dictionary<ItemType, int> itemCount = new Dictionary<ItemType, int>(); // 아이템 갯수
    public Dictionary<ItemType, int> structLevel = new Dictionary<ItemType, int>(); // 건물 레벨


    public event Action<ItemType, int> OnItemChanged;

    //public bool itemConsumeSuccess;

    // 아이템을 생산하는 함수
    public void ProduceItem(ItemType itemType, int amount)
    {
        // 생산된 아이템 갯수 증가
        itemCount[itemType] += amount;
        OnItemChanged?.Invoke(itemType, amount);
        Debug.Log(itemCount);
        //Debug.Log(itemType + " 아이템 " + amount + "개 생산! 현재 갯수: " + itemCount[itemType]);
    }

    // 아이템을 소모하는 함수
    public void ConsumeItem(ItemType[] itemType, int[] amount)
    {
        if (CheckItemCount(itemType, amount))
        {
            for (int i = 0; i < itemType.Length; i++)
            {
                itemCount[itemType[i]] -= amount[i];
                OnItemChanged?.Invoke(itemType[i], amount[i]);
                Debug.Log(itemType[i] + " 아이템 " + amount[i] + "개 소모! 현재 갯수: " + itemCount[itemType[i]]);
            }
        }        
    }

    private void InitializeItemCounts()
    {
        // 모든 아이템 종류에 대해 초기 갯수를 0으로 설정
        foreach (ItemType itemType in (ItemType[])Enum.GetValues(typeof(ItemType)))
        {
            itemCount[itemType] = 0;
        }
    }

    private void InitializestructLevel()
    {
        // 모든 건물 레벨을 0으로 초기화
        foreach (ItemType itemType in (ItemType[])Enum.GetValues(typeof(ItemType)))
        {
            structLevel[itemType] = 0;
        }
    }

    // 아이템을 소모하기 전 확인하는 함수
    public bool CheckItemCount(ItemType[] itemType, int[] amount)
    {
        // 아이템 소모가능 체크
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
