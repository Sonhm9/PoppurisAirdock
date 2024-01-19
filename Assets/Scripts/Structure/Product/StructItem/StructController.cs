using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class StructController : MonoBehaviour
{
    public StructItemManager.ItemType itemType; // �������� ������ ����
    private StructDataTable structData;
    private TimeCheckerController timeChecker;
    private Coroutine productionCoroutine;

    private float _elapsedTime = 0f; // ���� �ð�
    private float _gaugeTime; // ���� �ð��� ��Ÿ�� �� ������ �ð�


    void Start()
    {
        structData = GetComponent<Consumable>().structDataTable;
        timeChecker = GetComponentInChildren<TimeCheckerController>();
        _gaugeTime = structData.productTime; // ������ �ð��� �� �ð� ����
        StartProduction();    
    }
    private void Update()
    {
        if (_elapsedTime < _gaugeTime)
        {
            // ������ �ð��� ���� �ð� ����ȭ
            if (_gaugeTime != structData.productTime)
            {
                _gaugeTime = structData.productTime;
                _elapsedTime = 0f;
                StopProduction();
                StartProduction();
            }
            _elapsedTime += Time.deltaTime;
            timeChecker.UpdateGauge(_elapsedTime / structData.productTime);
            if (_elapsedTime >= _gaugeTime) _elapsedTime = 0f;
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
            yield return new WaitForSeconds(structData.productTime); // ���� �ð� ���
            StructItemManager.instance.ProduceItem(itemType, 1); // ������ ����
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
}
