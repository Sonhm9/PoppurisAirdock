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
        // 싱글톤
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
    private InfVal currentGold; // 현재 골드 값

    [SerializeField]
    private InfVal currentEnergy; // 현재 에너지 값

    [SerializeField]
    private InfVal currentDiamond; // 현재 다이아 값

    public bool resourceConsumeSuccess; // 자원 소모 성공

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
            if (value != currentGold) // 값이 변경된 경우에만 애니메이션 실행
            {
                StartCoroutine(ChangeResourceValue(ResourceType.Gold, currentGold, value));
            }
            currentGold = value;
            UpgradeManager.instance.FindMaxUpgradeCost();


        }
    } 
    // 골드 프로퍼티
    public InfVal CurrentEnergy
    { 
        get { return currentEnergy; }
        set
        {
            if (value != currentEnergy) // 값이 변경된 경우에만 애니메이션 실행
            {
                StartCoroutine(ChangeResourceValue(ResourceType.Energy, currentEnergy, value));
            }
            currentEnergy = value;
        }

    }
    // 에너지 프로퍼티
    public InfVal CurrentDiamond 
    { 
        get { return currentDiamond; }
        set
        {
            if (value != currentDiamond) // 값이 변경된 경우에만 애니메이션 실행
            {
                StartCoroutine(ChangeResourceValue(ResourceType.Diamond, currentDiamond, value));
            }
            currentDiamond = value;
        }
    }
    // 다이아 프로퍼티

    public InfVal GetResourceValue(ResourceType resourceType)
    {
        // 자원 값 읽기 메서드
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
        // 자원 증가 메서드
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
        // 자원 감소 메서드
        switch (resourceType)
        {
            case ResourceType.Gold:
                if (currentGold >= amount)
                {
                    CurrentGold -= amount;
                }
                else Debug.Log($"{resourceType}이 부족합니다.");
                break;
            case ResourceType.Energy:
                if (currentEnergy >= amount)
                {
                    CurrentEnergy -= amount;
                }
                else Debug.Log($"{resourceType}이 부족합니다.");
                break;
            case ResourceType.Diamond:
                if (currentDiamond >= amount)
                {
                    CurrentDiamond -= amount;
                }
                else Debug.Log($"{resourceType}이 부족합니다.");
                break;
        }
    }

    public void CheckRemoveResources(ResourceType[] resourceTypes, int[] amount)
    {
        for(int i=0; i<resourceTypes.Length; i++)
        {
            // 소모 가능 여부 확인
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
            // 소모 가능하면 소모
            for (int i = 0; i < resourceTypes.Length; i++)
            {
                RemoveResource(resourceTypes[i], amount[i]);
            }
        }
        
    }

    private IEnumerator ChangeResourceValue(ResourceType resourceType, InfVal startValue, InfVal endValue)
    {
        float duration = 1.0f; // 애니메이션 지속 시간 (초)
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
        // 자원 저장 메서드
        // 각 자원을 저장데이터에 대입
        DataSaveManager.instance.saveDatas.gold = GetResourceValue(ResourceType.Gold).ToString();
        DataSaveManager.instance.saveDatas.energy = GetResourceValue(ResourceType.Energy).ToString();
        DataSaveManager.instance.saveDatas.diamond = GetResourceValue(ResourceType.Diamond).ToString();

        // 데이터 저장
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
