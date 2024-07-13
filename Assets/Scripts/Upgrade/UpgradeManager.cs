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
    // �̱���


    public GameObject upgradeButton; // ���׷��̵� ��ư
    public Image itemImage; // ��� ��������Ʈ
    public TextMeshProUGUI itemName; // ��� �̸�
    public TextMeshProUGUI itemLevel; // ��� ����
    public TextMeshProUGUI itemCost; // ��� ���
    public List<StructDataTable> structDataTables; // ��� ������ ���̺� ����Ʈ

    private UpgradeBehavior upgradeBehavior;

    [HideInInspector] public int structProgress = 2; // ���� �ǹ� �����Ȳ
    [HideInInspector] public float costMultiple = 1.2f; // ��� ����



    public Dictionary<StructManager.ItemType, InfVal> upgradeCost = new Dictionary<StructManager.ItemType, InfVal>()
    {
        { StructManager.ItemType.WOODBONE, 100 },
        { StructManager.ItemType.BUOYANCY, 200 }, { StructManager.ItemType.WOODBODY, 300 }
    }; // ���׷��̵� ���

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
        // �ִ� �ڽ�Ʈ�� ���ϰ� ��ȭ�˾��� ���� �޼���
        StructManager.ItemType[] itemTypes = (StructManager.ItemType[])Enum.GetValues(typeof(StructManager.ItemType)); // ��� �� �迭�� ��ȯ
        
        InfVal maxValue = 0; // �ִ� �ڽ�Ʈ
        StructManager.ItemType maxKey = StructManager.ItemType.WOODBONE; // �ִ� �ڽ�Ʈ�� Ű

        for (int i = 0; i <= structProgress; i++)
        {
            StructManager.ItemType itemType = itemTypes[i];
            if (upgradeCost.ContainsKey(itemType))
            {
                if (upgradeCost[itemType] > maxValue && upgradeCost[itemType]< ResourceManager.instance.GetResourceValue(ResourceManager.ResourceType.Gold))
                {
                    // �ִ밪�� �����ϰ� ���� ��带 �Ҹ��� �� ���� ��
                    // �ִ� �ڽ�Ʈ ����
                    upgradeButton.SetActive(true);
                    maxValue = upgradeCost[itemType];
                    maxKey = itemType;
                }
            }
        }
        if (maxValue == 0)
        {
            // ��ȭ�� ��Ұ� �������
            upgradeButton.SetActive(false);
        }
        else
        {
            foreach (StructDataTable structdata in structDataTables)
            {
                // ��� �����Ϳ� ��ȭ�� ��Ҹ� ����
                if (structdata.buildingType == maxKey.ToString())
                {
                    // �̹���,�̸�,����,��� ǥ��
                    itemImage.sprite = structdata.buildingimage;
                    itemName.text = structdata.itemName;
                    itemLevel.text = "LV: " + (StructManager.instance.structLevel[maxKey] + 1).ToString();
                    itemCost.text = maxValue.ToString();
                }
            }
            upgradeBehavior.GetUpgradeItem(maxKey); // ��� ��ü �Ѱ��ֱ�
        }   
    }
    public void MultiplyUpgradeCost(StructManager.ItemType itemType)
    {
        // ���׷��̵� ��� ����
        InfVal cost = upgradeCost[itemType] * costMultiple;
        upgradeCost[itemType] = cost;
    }

    
}
