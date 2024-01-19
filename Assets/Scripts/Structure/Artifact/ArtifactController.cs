using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfiniteValue;
using System;


public class ArtifactController : MonoBehaviour
{
    public ArtifactDataTable artifactData;
    private float energyProductTime = 1f; // 에너지 생산 시간(초)
    private InfVal energyTotalValue { get; set; } // 에너지 총 량
    private InfVal energyProductValue { get; set; } // 초 당 에너지 생산 량
    private InfVal energyNowValue { get; set; } // 에너지 현재 량


    private Liquid liquid;
    private Material material;
    public enum ArtifactType
    {
        GARDEN,
        METEOR
    }
    public Dictionary<ArtifactType, InfVal> artifactValue = new Dictionary<ArtifactType, InfVal>();

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
                yield return new WaitForSeconds(energyProductTime); // 생산 시간 대기
                energyNowValue += energyProductValue;
                if (liquid != null)
                {
                    liquid.fillAmount = (float)MathInfVal.Clamp01(1 - (energyNowValue / energyTotalValue));
                }
            }

            // AddEnergyValue() 메서드를 통해 energyNowValue를 0으로 만들 때까지 대기합니다.
            yield return new WaitUntil(() => energyNowValue == 0);
        }
    }
    public void AddEnergyValue()
    {
        if (energyNowValue > 10)
        {
            ResourceManager.instance.AddResource(ResourceManager.ResourceType.Energy, energyNowValue);
            energyNowValue = 0;
            if (liquid != null)
            {
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
