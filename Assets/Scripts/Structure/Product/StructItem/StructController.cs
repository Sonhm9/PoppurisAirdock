using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class StructController : MonoBehaviour
{
    [SerializeField] StructItemManager.ItemType itemType; // 아이템을 정의할 변수
    [SerializeField] StructItemManager.ItemType[] consumeItemType; // 소모할 아이템 배열
    [SerializeField] int[] consumeItemCount; // 소모할 아이템의 개수

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

        currentStructTime = CalProductTIme(structData, StructItemManager.instance.structLevel[itemType]); // 생산시간 계산
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
        // 건물 레벨 변경시 텍스트 갱신
        LevelController levelController = GetComponentInChildren<LevelController>();
        if (levelController != null)
        {
            levelController.SetLevelText();
        }
    }

    public float CalProductTIme(StructDataTable structData, int level)
    {
        float time = 0f;
        time = structData.productTime - ((structData.productTime / 2) * (1 - Mathf.Pow(decreaseRate, level))); // 생산시간 - (최소시간 * (1 - 0.99^레벨))

        return time;
    }
}
