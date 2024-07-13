using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StructManager : MonoBehaviour
{
    private static object _lock = new object();
    private object itemLock = new object();

    private static StructManager _instance = null;
    public static StructManager instance
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
                    obj.AddComponent<StructManager>();
                    _instance = obj.GetComponent<StructManager>();
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

        // 재료 갯수, 건물 레벨 초기화
        InitializeItemCounts();
        InitializestructLevel();
    }
    private void OnDestroy()
    {
        applicationQuitting = true;
    }

    public enum ItemType
    {
        // 재료 타입
        WOODBONE,
        BUOYANCY,
        WOODBODY
    }

    public Dictionary<ItemType, int> itemCount = new Dictionary<ItemType, int>(); // 재료 갯수
    public Dictionary<ItemType, int> structLevel = new Dictionary<ItemType, int>(); // 건물 당 레벨     

    public event Action<ItemType, int> OnItemChanged; // 재료 갯수 변경 이벤트
    public event Action<ItemType, int> OnLevelChanged; // 재료 레벨 변경 이벤트


    public void ProduceItem(ItemType itemType, int amount)
    {
        // 아이템 생산 메서드
        itemCount[itemType] += amount; // 재료 갯수 증가
        OnItemChanged?.Invoke(itemType, amount); // 재료 갯수 변경 이벤트 실행
        Debug.Log(itemType + " 아이템 " + amount + "개 생산! 현재 갯수: " + itemCount[itemType]);
    }

    public void ConsumeItem(ItemType[] itemType, int[] amount)
    {
        // 재료 소모 메서드
        if (CheckItemCount(itemType, amount))
        {
            // 재료 소모 가능 여부 확인
            for (int i = 0; i < itemType.Length; i++)
            {
                // 각 재료 별 갯수
                itemCount[itemType[i]] -= amount[i]; // 재료 갯수 감소
                OnItemChanged?.Invoke(itemType[i], amount[i]); // 재료 갯수변경 이벤트 실행
                Debug.Log(itemType[i] + " 재료 " + amount[i] + "개 소모! 현재 갯수: " + itemCount[itemType[i]]);
            }
        }        
    }
    public void StructLevelUP(ItemType itemType)
    {
        // 건물 레벨업 메서드
        structLevel[itemType]++; // 건물 레벨 증가
        OnLevelChanged?.Invoke(itemType, structLevel[itemType]); // 건물 레벨 변경 이벤트 실행
        Debug.Log(itemType + " " + structLevel[itemType]+ " 레벨 ");
    }

    private void InitializeItemCounts()
    {
        // 모든 재료 종류에 대해 초기 갯수를 0으로 설정
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

    public bool CheckItemCount(ItemType[] itemType, int[] amount)
    {
        // 재료 소모하기 전 확인하는 함수

        // 재료 소모가능 체크
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
