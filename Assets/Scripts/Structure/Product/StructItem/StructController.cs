using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class StructController : MonoBehaviour
{
    public StructItemManager.ItemType itemType; // 아이템을 정의할 변수
    private StructDataTable structData;
    private TimeCheckerController timeChecker;
    private Coroutine productionCoroutine;

    private float _elapsedTime = 0f; // 현재 시간
    private float _gaugeTime; // 생산 시간을 나타낼 총 게이지 시간


    void Start()
    {
        structData = GetComponent<Consumable>().structDataTable;
        timeChecker = GetComponentInChildren<TimeCheckerController>();
        _gaugeTime = structData.productTime; // 게이지 시간에 총 시간 대입
        StartProduction();    
    }
    private void Update()
    {
        if (_elapsedTime < _gaugeTime)
        {
            // 게이지 시간과 생산 시간 동일화
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
        // 생산 시작하는 메서드
        productionCoroutine = StartCoroutine(ProduceItemRoutine());
    }
    

    private void StopProduction()
    {
        // 생산 중단하는 메서드
        if (productionCoroutine != null)
        {
            StopCoroutine(productionCoroutine);
        }
    }

    private IEnumerator ProduceItemRoutine()
    {
        // 생산 코루틴
        while (true)
        {
            yield return new WaitForSeconds(structData.productTime); // 생산 시간 대기
            StructItemManager.instance.ProduceItem(itemType, 1); // 아이템 생산
        }
    }

    public void ChangeBuildingLevel()
    {
        // 건물 레벨 변경시 텍스트 갱신
        LevelController levelController = GetComponentInChildren<LevelController>();
        if (levelController != null)
        {
            levelController.SetLevelText();
        }
    }
}
