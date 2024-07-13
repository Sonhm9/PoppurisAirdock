using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class StructBehavior : MonoBehaviour
{
    [SerializeField] private StructManager.ItemType itemType; // ��� ������ ����
    [SerializeField] private StructManager.ItemType[] consumeItemType; // �Ҹ��� ������ �迭
    [SerializeField] private int[] consumeItemCount; // �Ҹ��� �������� ����

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

        currentStructTime = CalProductTIme(structData, StructManager.instance.structLevel[itemType]); // ����ð� ���
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
                // ��� �Ҹ��� �ʿ� ������
                productState = true; // ���� ���� True
                popuriAnimator.SetFloat("ActionNumber", structData.animationState); // ���� �ִϸ��̼� ����
                yield return new WaitForSeconds(structData.productTime); // ���� �ð� ���� ���
                StructManager.instance.ProduceItem(itemType, 1); // ��� ����
                currentStructTime = CalProductTIme(structData, StructManager.instance.structLevel[itemType]);
            }
            else if (StructManager.instance.CheckItemCount(consumeItemType, consumeItemCount))
            {
                // �������� �Ҹ��Ͽ� ������ ���� ��
                productState = true; // ���� ���� True
                StructManager.instance.ConsumeItem(consumeItemType, consumeItemCount); // ��� �Ҹ�
                popuriAnimator.SetFloat("ActionNumber", structData.animationState); // ���� �ִϸ��̼� ����
                yield return new WaitForSeconds(structData.productTime); // ���� �ð� ���� ���
                StructManager.instance.ProduceItem(itemType, 1); // ��� ����
                currentStructTime = CalProductTIme(structData, StructManager.instance.structLevel[itemType]);

            }
            else
            {
                // �������� ������ �� ������(�Ҹ���� ����)
                productState = false; // ���� ���� False
                popuriAnimator.SetFloat("ActionNumber", 0); // �޽� �ִϸ��̼� ����
                yield return new WaitForSeconds(1f); // 1�� ���
                currentStructTime = CalProductTIme(structData, StructManager.instance.structLevel[itemType]);
            }

        }
    }
    public int CalProductTIme(StructDataTable structData, int level)
    {
        // ������ ���� ����ð� ���
        float time = 0f;
        time = structData.productTime - ((structData.productTime / 2) * (1 - Mathf.Pow(decreaseRate, level))); // ����ð� - (�ּҽð� * (1 - 0.99^����))

        return (int)Mathf.Round(time);
    }
}
