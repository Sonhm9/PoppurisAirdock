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

    // 싱글톤
    [SerializeField]
    private InfVal currentGold; // 현재 골드 값

    [SerializeField]
    private InfVal currentEnergy; // 현재 에너지 값

    [SerializeField]
    private InfVal currentDiamond; // 현재 다이아 값

    private InfVal startGold = 0;// 시작 골드 값
    private InfVal startEnergy = 0; // 시작 에너지 값
    private InfVal startDiamond = 0; // 시작 다이아 값

    public event Action<ResourceType, InfVal> OnResourceChanged;

    public enum ResourceType
    {
        Gold,
        Energy,
        Diamond
    }

    private void Awake()
    {
        _instance = this;

        CurrentGold = startGold;
        CurrentEnergy = startEnergy;
        CurrentDiamond = startDiamond;
    }
    private void OnDestroy()
    {
        applicationQuitting = true;
    }

    public InfVal CurrentGold
    {
        get { return currentGold; }
        set
        {
            if (value != currentGold) // 값이 변경된 경우에만 애니메이션 실행
            {
                StartCoroutine(IncreaseResourceValue(ResourceType.Gold, currentGold, value));
            }
            currentGold = value;

            Debug.Log(currentGold);
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
                StartCoroutine(IncreaseResourceValue(ResourceType.Energy, currentEnergy, value));
            }
            currentEnergy = value;
            Debug.Log(currentEnergy);
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
                StartCoroutine(IncreaseResourceValue(ResourceType.Diamond, currentDiamond, value));
            }
            currentDiamond = value;
            Debug.Log(currentDiamond);
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

    private IEnumerator IncreaseResourceValue(ResourceType resourceType, InfVal startValue, InfVal endValue)
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

}
