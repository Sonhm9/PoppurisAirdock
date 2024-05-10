using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class StructController : MonoBehaviour
{
    [SerializeField] StructItemManager.ItemType itemType; // �������� ������ ����
    [SerializeField] StructItemManager.ItemType[] consumeItemType; // �Ҹ��� ������ �迭
    [SerializeField] int[] consumeItemCount; // �Ҹ��� �������� ����

    private Animator popuriAnimator; // ��Ǫ�� �ִϸ�����
    private StructDataTable structData;
    private TimeCheckerController timeChecker;
    private Coroutine productionCoroutine;

    private float currentStructTime; // ���� ���� �ð�
    private float elapsedTime = 0f; // ���� �ð�
    private float gaugeTime; // ���� �ð��� ��Ÿ�� �� ������ �ð�
    private float decreaseRate = 0.99f; // ���� �� ���� �ð�
    private bool productState = false;


    void Start()
    {
        structData = GetComponent<Consumable>().structDataTable;
        timeChecker = GetComponentInChildren<TimeCheckerController>();
        popuriAnimator = GetComponentInChildren<Animator>();

        currentStructTime = CalProductTIme(structData, StructItemManager.instance.structLevel[itemType]); // ����ð� ���
        gaugeTime = currentStructTime;
        StartProduction();    
    }
    private void Update()
    {
        if(productState)
        {
            if (elapsedTime < gaugeTime)
            {
                // ������ �ð��� ���� �ð� ����ȭ
                if (gaugeTime != currentStructTime)
                {
                    gaugeTime = currentStructTime;
                    elapsedTime = 0f;
                    StopProduction();
                    StartProduction();
                }
                elapsedTime += Time.deltaTime;
                timeChecker.UpdateGauge(elapsedTime / currentStructTime);
                if (elapsedTime >= gaugeTime) elapsedTime = 0f;
            }
        }
        else
        {
            // ������ �ߴܵǸ� Ÿ�̸Ӹ� ����
            elapsedTime = 0f;
            timeChecker.UpdateGauge(0f); // �������� �ʱ�ȭ
        }

    }

    private void StartProduction()
    {
        // ���� �����ϴ� �޼���
        productionCoroutine = StartCoroutine(ProduceItemRoutine());
    }
    

    private void StopProduction()
    {
        // ���� �ߴ��ϴ� �޼���
        if (productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
        }
    }

    private IEnumerator ProduceItemRoutine()
    {
        // ���� �ڷ�ƾ
        while (true)
        {
            if(consumeItemCount.Length == 0)
            {
                productState = true;
                popuriAnimator.SetFloat("ActionNumber", structData.animationState);
                yield return new WaitForSeconds(structData.productTime);
                StructItemManager.instance.ProduceItem(itemType, 1);
            }
            else if (StructItemManager.instance.CheckItemCount(consumeItemType, consumeItemCount))
            {
                productState = true;
                StructItemManager.instance.ConsumeItem(consumeItemType, consumeItemCount);
                popuriAnimator.SetFloat("ActionNumber", structData.animationState);
                yield return new WaitForSeconds(structData.productTime);
                StructItemManager.instance.ProduceItem(itemType, 1);
            }
            else
            {
                productState = false;
                popuriAnimator.SetFloat("ActionNumber", 0);
                yield return new WaitForSeconds(1f);
            }

        }
    }
    public void ChangeBuildingLevel()
    {
        // �ǹ� ���� ����� �ؽ�Ʈ ����
        LevelController levelController = GetComponentInChildren<LevelController>();
        if (levelController != null)
        {
            levelController.SetLevelText();
        }
    }

    public float CalProductTIme(StructDataTable structData, int level)
    {
        float time = 0f;
        time = structData.productTime - ((structData.productTime / 2) * (1 - Mathf.Pow(decreaseRate, level))); // ����ð� - (�ּҽð� * (1 - 0.99^����))

        return time;
    }
}
