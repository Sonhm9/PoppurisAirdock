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
    // 싱글톤

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
        // 모든 아이템 종류에 대해 초기 갯수를 0으로 설정
        foreach (ItemType itemType in (ItemType[])Enum.GetValues(typeof(ItemType)))
        {
            itemCount[itemType] = 0;
        }
    }
    // 아이템을 생산하는 함수
    public void ProduceItem(ItemType itemType, int amount)
    {
        // 생산된 아이템 갯수 증가
        itemCount[itemType] += amount;
        OnItemChanged?.Invoke(itemType, amount);
        //Debug.Log(itemType + " 아이템 " + amount + "개 생산! 현재 갯수: " + itemCount[itemType]);
    }

    // 아이템을 소모하는 함수
    public void ConsumeItem(ItemType itemType, int amount)
    {
        // 아이템 갯수가 0 이상인 경우에만 소모 가능
        if (itemConsumeSuccess)
        {
            // 아이템 갯수 감소
            itemCount[itemType] -= amount;
            OnItemChanged?.Invoke(itemType, amount);

            Debug.Log(itemType + " 아이템 " + amount + "개 소모! 현재 갯수: " + itemCount[itemType]);
        }
        else
        {
            Debug.Log(itemType + " 아이템이 부족합니다!");
        }
    }

    // 아이템을 소모하기 전 확인하는 함수
    public void CheckItemCount(ItemType itemType, int amount)
    {
        if (itemCount[itemType] >= amount)
        {
            itemConsumeSuccess = true;
        }
        else itemConsumeSuccess = false;

    }
}
