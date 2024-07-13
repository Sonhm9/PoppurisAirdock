using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfiniteValue;
using System;


public class ArtifactBehavior : MonoBehaviour
{
    [SerializeField] ArtifactDataTable artifactData;
    private float energyProductTime = 1f; // 에너지 생산 시간(초)
    private InfVal energyTotalValue { get; set; } // 에너지 총 량
    private InfVal energyProductValue { get; set; } // 초 당 에너지 생산 량
    private InfVal energyNowValue { get; set; } // 에너지 현재 량


    private Liquid liquid;
    private Material material;
    public enum ArtifactType
    {
        // 아티팩트 타입
        GARDEN,
        METEOR
    }
    public Dictionary<ArtifactType, InfVal> artifactValue = new Dictionary<ArtifactType, InfVal>(); // 아티팩트 값 딕셔너리

    void Start()
    {
        liquid = GetComponent<Liquid>();
        if (liquid == null)
        {
            Debug.LogWarning("Liquid 컴포넌트가 없습니다!");
        }
        Renderer renderer = GetComponent<Renderer>();
        material = new Material(renderer.material);
        // 머터리얼 인스턴스 생성

        artifactData = GetComponentInParent<ArtConsumable>().artifactDataTable;

        InitializeArtifactValue();
        energyTotalValue = 1000;
        energyProductValue = artifactData.productValue;

        StartCoroutine(ProduceEnergyRoutine());
    }

    private void InitializeArtifactValue()
    {
        // 모든 아티팩트 종류에 대해 초기 값을 0으로 설정
        foreach (ArtifactType artifactType in (ArtifactType[])Enum.GetValues(typeof(ArtifactType)))
        {
            artifactValue[artifactType] = 0;
        }
    }
    private IEnumerator ProduceEnergyRoutine()
    {
        // 에너지 생산 루틴
        while (true)
        {
            while (energyNowValue < energyTotalValue)
            {
                // 현재 에너지 값이 최대값보다 적을 때
                yield return new WaitForSeconds(energyProductTime); // 생산 시간 동안 대기
                energyNowValue += energyProductValue; // 현재 에너지 값 증가
                if (liquid != null)
                {
                    // 액체 쉐이더 총량 계산
                    liquid.fillAmount = (float)MathInfVal.Clamp01(1 - (energyNowValue / energyTotalValue));
                }
            }

            // 현재 에너지 값이 0이 될때까지 대기
            yield return new WaitUntil(() => energyNowValue == 0);
        }
    }

    public void AddEnergyValue()
    {
        // 에너지 획득 메서드
        if (energyNowValue > 10)
        {
            // 모은 에너지가 10 초과부터 획득가능
            ResourceManager.instance.AddResource(ResourceManager.ResourceType.Energy, energyNowValue); // 에너지 자원 획득
            energyNowValue = 0; // 현재 에너지 초기화
            if (liquid != null)
            {
                // 액체 쉐이더 총량 계산
                liquid.fillAmount = (float)MathInfVal.Clamp01(1 - (energyNowValue / energyTotalValue));
            }
        }
        else Debug.Log("Not Enough Energy");
    }

    public void SaveArtifactValue(ArtifactType artifactType)
    {
        // 생산된 아티팩트 값 저장
        artifactValue[artifactType] += energyNowValue;
        Debug.Log(artifactType + " 타입 현재 값: " + artifactValue[artifactType]);
    }

}
