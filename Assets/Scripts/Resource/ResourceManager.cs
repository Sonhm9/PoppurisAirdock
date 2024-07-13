using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using InfiniteValue;


public class ResourceManager : MonoBehaviour
{
    private static object _lock = new object();
    private static ResourceManager _instance = null;
    public static ResourceManager instance
    {
        // �̱���
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
                    GameObject obj = new GameObject("ResourceManager ");
                    obj.AddComponent<ResourceManager>();
                    _instance = obj.GetComponent<ResourceManager>();
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


    [SerializeField]
    private InfVal currentGold; // ���� ��� ��

    [SerializeField]
    private InfVal currentEnergy; // ���� ������ ��

    [SerializeField]
    private InfVal currentDiamond; // ���� ���̾� ��

    public bool resourceConsumeSuccess; // �ڿ� �Ҹ� ����

    private string keyName = "resourceDatas";

    private void Awake()
    {
        _instance = this;
    }
    private void OnDestroy()
    {
        applicationQuitting = true;
    }

    public enum ResourceType
    {
        Gold,
        Energy,
        Diamond
    }
    public event Action<ResourceType, InfVal> OnResourceChanged;

    public InfVal CurrentGold
    {
        get { return currentGold; }
        set
        {
            if (value != currentGold) // ���� ����� ��쿡�� �ִϸ��̼� ����
            {
                StartCoroutine(ChangeResourceValue(ResourceType.Gold, currentGold, value));
            }
            currentGold = value;
            UpgradeManager.instance.FindMaxUpgradeCost();


        }
    } 
    // ��� ������Ƽ
    public InfVal CurrentEnergy
    { 
        get { return currentEnergy; }
        set
        {
            if (value != currentEnergy) // ���� ����� ��쿡�� �ִϸ��̼� ����
            {
                StartCoroutine(ChangeResourceValue(ResourceType.Energy, currentEnergy, value));
            }
            currentEnergy = value;
        }

    }
    // ������ ������Ƽ
    public InfVal CurrentDiamond 
    { 
        get { return currentDiamond; }
        set
        {
            if (value != currentDiamond) // ���� ����� ��쿡�� �ִϸ��̼� ����
            {
                StartCoroutine(ChangeResourceValue(ResourceType.Diamond, currentDiamond, value));
            }
            currentDiamond = value;
        }
    }
    // ���̾� ������Ƽ

    public InfVal GetResourceValue(ResourceType resourceType)
    {
        // �ڿ� �� �б� �޼���
        switch (resourceType)
        {
            case ResourceType.Gold:
                return CurrentGold;

            case ResourceType.Energy:
                return CurrentEnergy;

            case ResourceType.Diamond:
                return CurrentDiamond;

            default:
                return 0;
        }
    }
    public void AddResource(ResourceType resourceType, InfVal amount)
    {
        // �ڿ� ���� �޼���
        switch (resourceType)
        {
            case ResourceType.Gold:
                CurrentGold += amount;
                break;
            case ResourceType.Energy:
                CurrentEnergy += amount;
                break;
            case ResourceType.Diamond:
                CurrentDiamond += amount;
                break;
        }
    }

    public void RemoveResource(ResourceType resourceType, InfVal amount)
    {
        // �ڿ� ���� �޼���
        switch (resourceType)
        {
            case ResourceType.Gold:
                if (currentGold >= amount)
                {
                    CurrentGold -= amount;
                }
                else Debug.Log($"{resourceType}�� �����մϴ�.");
                break;
            case ResourceType.Energy:
                if (currentEnergy >= amount)
                {
                    CurrentEnergy -= amount;
                }
                else Debug.Log($"{resourceType}�� �����մϴ�.");
                break;
            case ResourceType.Diamond:
                if (currentDiamond >= amount)
                {
                    CurrentDiamond -= amount;
                }
                else Debug.Log($"{resourceType}�� �����մϴ�.");
                break;
        }
    }

    public void CheckRemoveResources(ResourceType[] resourceTypes, int[] amount)
    {
        for(int i=0; i<resourceTypes.Length; i++)
        {
            // �Ҹ� ���� ���� Ȯ��
            if (GetResourceValue(resourceTypes[i]) >= amount[i])
            {
                resourceConsumeSuccess = true;
            }
            else
            {
                resourceConsumeSuccess = false;
                break;
            }
        }
        if (resourceConsumeSuccess)
        {
            // �Ҹ� �����ϸ� �Ҹ�
            for (int i = 0; i < resourceTypes.Length; i++)
            {
                RemoveResource(resourceTypes[i], amount[i]);
            }
        }
        
    }

    private IEnumerator ChangeResourceValue(ResourceType resourceType, InfVal startValue, InfVal endValue)
    {
        float duration = 1.0f; // �ִϸ��̼� ���� �ð� (��)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            InfVal newValue = MathInfVal.Round(Mathf.Lerp((float)startValue, (float)endValue, t));
            OnResourceChanged?.Invoke(resourceType, newValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        OnResourceChanged?.Invoke(resourceType, endValue);
    }
    public void SaveResourceData()
    {
        // �ڿ� ���� �޼���
        // �� �ڿ��� ���嵥���Ϳ� ����
        DataSaveManager.instance.saveDatas.gold = GetResourceValue(ResourceType.Gold).ToString();
        DataSaveManager.instance.saveDatas.energy = GetResourceValue(ResourceType.Energy).ToString();
        DataSaveManager.instance.saveDatas.diamond = GetResourceValue(ResourceType.Diamond).ToString();

        // ������ ����
        DataSaveManager.instance.DataSave(keyName);
    }
    public void LoadResourceData()
    {
        DataSaveManager.instance.LoadData(keyName);

        currentGold = InfVal.Parse(DataSaveManager.instance.saveDatas.gold);
        StartCoroutine(ChangeResourceValue(ResourceType.Gold, currentGold, currentGold));

        currentEnergy = InfVal.Parse(DataSaveManager.instance.saveDatas.energy);
        StartCoroutine(ChangeResourceValue(ResourceType.Energy, currentEnergy, currentEnergy));

        currentDiamond = InfVal.Parse(DataSaveManager.instance.saveDatas.diamond);
        StartCoroutine(ChangeResourceValue(ResourceType.Diamond, currentDiamond, currentDiamond));
    }

}
