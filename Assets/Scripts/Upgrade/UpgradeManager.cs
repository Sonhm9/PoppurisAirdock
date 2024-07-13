using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfiniteValue;
using System;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    private static object _lock = new object();
    private object itemLock = new object();

    private static UpgradeManager _instance = null;
    public static UpgradeManager instance
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
                    GameObject obj = new GameObject("UpgradeManager ");
                    obj.AddComponent<UpgradeManager>();
                    _instance = obj.GetComponent<UpgradeManager>();
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


    public GameObject upgradeButton; // 업그레이드 버튼
    public Image itemImage; // 재료 스프라이트
    public TextMeshProUGUI itemName; // 재료 이름
    public TextMeshProUGUI itemLevel; // 재료 레벨
    public TextMeshProUGUI itemCost; // 재료 비용
    public List<StructDataTable> structDataTables; // 재료 데이터 테이블 리스트

    private UpgradeBehavior upgradeBehavior;

    [HideInInspector] public int structProgress = 2; // 현재 건물 진행상황
    [HideInInspector] public float costMultiple = 1.2f; // 비용 배율



    public Dictionary<StructManager.ItemType, InfVal> upgradeCost = new Dictionary<StructManager.ItemType, InfVal>()
    {
        { StructManager.ItemType.WOODBONE, 100 },
        { StructManager.ItemType.BUOYANCY, 200 }, { StructManager.ItemType.WOODBODY, 300 }
    }; // 업그레이드 비용

    private void Awake()
    {
        _instance = this;
    }
    public void Start()
    {
        upgradeBehavior = GetComponentInChildren<UpgradeBehavior>();
        UpgradeManager.instance.FindMaxUpgradeCost();
    }
    private void OnDestroy()
    {
        applicationQuitting = true;
    }

    public void FindMaxUpgradeCost()
    {
        // 최대 코스트를 구하고 강화팝업을 띄우는 메서드
        StructManager.ItemType[] itemTypes = (StructManager.ItemType[])Enum.GetValues(typeof(StructManager.ItemType)); // 재료 값 배열로 변환
        
        InfVal maxValue = 0; // 최대 코스트
        StructManager.ItemType maxKey = StructManager.ItemType.WOODBONE; // 최대 코스트의 키

        for (int i = 0; i <= structProgress; i++)
        {
            StructManager.ItemType itemType = itemTypes[i];
            if (upgradeCost.ContainsKey(itemType))
            {
                if (upgradeCost[itemType] > maxValue && upgradeCost[itemType]< ResourceManager.instance.GetResourceValue(ResourceManager.ResourceType.Gold))
                {
                    // 최대값을 갱신하고 현재 골드를 소모할 수 있을 때
                    // 최대 코스트 갱신
                    upgradeButton.SetActive(true);
                    maxValue = upgradeCost[itemType];
                    maxKey = itemType;
                }
            }
        }
        if (maxValue == 0)
        {
            // 강화할 요소가 없을경우
            upgradeButton.SetActive(false);
        }
        else
        {
            foreach (StructDataTable structdata in structDataTables)
            {
                // 재료 데이터와 강화할 요소를 대조
                if (structdata.buildingType == maxKey.ToString())
                {
                    // 이미지,이름,레벨,비용 표시
                    itemImage.sprite = structdata.buildingimage;
                    itemName.text = structdata.itemName;
                    itemLevel.text = "LV: " + (StructManager.instance.structLevel[maxKey] + 1).ToString();
                    itemCost.text = maxValue.ToString();
                }
            }
            upgradeBehavior.GetUpgradeItem(maxKey); // 재료 객체 넘겨주기
        }   
    }
    public void MultiplyUpgradeCost(StructManager.ItemType itemType)
    {
        // 업그레이드 비용 증가
        InfVal cost = upgradeCost[itemType] * costMultiple;
        upgradeCost[itemType] = cost;
    }

    
}
