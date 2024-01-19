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

    // �̱���
    [SerializeField]
    private InfVal currentGold; // ���� ��� ��

    [SerializeField]
    private InfVal currentEnergy; // ���� ������ ��

    [SerializeField]
    private InfVal currentDiamond; // ���� ���̾� ��

    private InfVal startGold = 0;// ���� ��� ��
    private InfVal startEnergy = 0; // ���� ������ ��
    private InfVal startDiamond = 0; // ���� ���̾� ��

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
            if (value != currentGold) // ���� ����� ��쿡�� �ִϸ��̼� ����
            {
                StartCoroutine(IncreaseResourceValue(ResourceType.Gold, currentGold, value));
            }
            currentGold = value;

            Debug.Log(currentGold);
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
                StartCoroutine(IncreaseResourceValue(ResourceType.Energy, currentEnergy, value));
            }
            currentEnergy = value;
            Debug.Log(currentEnergy);
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
                StartCoroutine(IncreaseResourceValue(ResourceType.Diamond, currentDiamond, value));
            }
            currentDiamond = value;
            Debug.Log(currentDiamond);
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

    private IEnumerator IncreaseResourceValue(ResourceType resourceType, InfVal startValue, InfVal endValue)
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

}
