using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class StructBehavior : MonoBehaviour
{
    [SerializeField] private StructManager.ItemType itemType; // 재료 정의할 변수
    [SerializeField] private StructManager.ItemType[] consumeItemType; // 소모할 아이템 배열
    [SerializeField] private int[] consumeItemCount; // 소모할 아이템의 개수

    private Animator popuriAnimator; // 포푸리 애니메이터
    private StructDataTable structData;
    private TimeCheckerController timeChecker;
    private Coroutine productionCoroutine;

    private float currentStructTime; // 현재 생산 시간
    private float elapsedTime = 0f; // 현재 시간
    private float gaugeTime; // 생산 시간을 나타낼 총 게이지 시간
    private float decreaseRate = 0.99f; // 레벨 당 생산 시간
    private bool productState = false;


    void Start()
    {
        structData = GetComponent<Consumable>().structDataTable;
        timeChecker = GetComponentInChildren<TimeCheckerController>();
        popuriAnimator = GetComponentInChildren<Animator>();

        currentStructTime = CalProductTIme(structData, StructManager.instance.structLevel[itemType]); // 생산시간 계산
        gaugeTime = currentStructTime;
        StartProduction();    
    }
    private void Update()
    {
        if(productState)
        {
            if (elapsedTime < gaugeTime)
            {
                // 게이지 시간과 생산 시간 동일화
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
            // 생산이 중단되면 타이머를 멈춤
            elapsedTime = 0f;
            timeChecker.UpdateGauge(0f); // 게이지를 초기화
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
            if(consumeItemCount.Length == 0)
            {
                // 재료 소모할 필요 없을때
                productState = true; // 생산 상태 True
                popuriAnimator.SetFloat("ActionNumber", structData.animationState); // 생산 애니메이션 실행
                yield return new WaitForSeconds(structData.productTime); // 생산 시간 동안 대기
                StructManager.instance.ProduceItem(itemType, 1); // 재료 생산
                currentStructTime = CalProductTIme(structData, StructManager.instance.structLevel[itemType]);
            }
            else if (StructManager.instance.CheckItemCount(consumeItemType, consumeItemCount))
            {
                // 아이템을 소모하여 아이템 생산 시
                productState = true; // 생산 상태 True
                StructManager.instance.ConsumeItem(consumeItemType, consumeItemCount); // 재료 소모
                popuriAnimator.SetFloat("ActionNumber", structData.animationState); // 생산 애니메이션 실행
                yield return new WaitForSeconds(structData.productTime); // 생산 시간 동안 대기
                StructManager.instance.ProduceItem(itemType, 1); // 재료 생산
                currentStructTime = CalProductTIme(structData, StructManager.instance.structLevel[itemType]);

            }
            else
            {
                // 아이템을 생산할 수 없을때(소모재료 부족)
                productState = false; // 생산 상태 False
                popuriAnimator.SetFloat("ActionNumber", 0); // 휴식 애니메이션 실행
                yield return new WaitForSeconds(1f); // 1초 대기
                currentStructTime = CalProductTIme(structData, StructManager.instance.structLevel[itemType]);
            }

        }
    }
    public int CalProductTIme(StructDataTable structData, int level)
    {
        // 레벨에 따른 생산시간 계산
        float time = 0f;
        time = structData.productTime - ((structData.productTime / 2) * (1 - Mathf.Pow(decreaseRate, level))); // 생산시간 - (최소시간 * (1 - 0.99^레벨))

        return (int)Mathf.Round(time);
    }
}
