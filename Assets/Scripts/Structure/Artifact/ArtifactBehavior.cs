using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfiniteValue;
using System;


public class ArtifactBehavior : MonoBehaviour
{
    [SerializeField] ArtifactDataTable artifactData;
    private float energyProductTime = 1f; // ������ ���� �ð�(��)
    private InfVal energyTotalValue { get; set; } // ������ �� ��
    private InfVal energyProductValue { get; set; } // �� �� ������ ���� ��
    private InfVal energyNowValue { get; set; } // ������ ���� ��


    private Liquid liquid;
    private Material material;
    public enum ArtifactType
    {
        // ��Ƽ��Ʈ Ÿ��
        GARDEN,
        METEOR
    }
    public Dictionary<ArtifactType, InfVal> artifactValue = new Dictionary<ArtifactType, InfVal>(); // ��Ƽ��Ʈ �� ��ųʸ�

    void Start()
    {
        liquid = GetComponent<Liquid>();
        if (liquid == null)
        {
            Debug.LogWarning("Liquid ������Ʈ�� �����ϴ�!");
        }
        Renderer renderer = GetComponent<Renderer>();
        material = new Material(renderer.material);
        // ���͸��� �ν��Ͻ� ����

        artifactData = GetComponentInParent<ArtConsumable>().artifactDataTable;

        InitializeArtifactValue();
        energyTotalValue = 1000;
        energyProductValue = artifactData.productValue;

        StartCoroutine(ProduceEnergyRoutine());
    }

    private void InitializeArtifactValue()
    {
        // ��� ��Ƽ��Ʈ ������ ���� �ʱ� ���� 0���� ����
        foreach (ArtifactType artifactType in (ArtifactType[])Enum.GetValues(typeof(ArtifactType)))
        {
            artifactValue[artifactType] = 0;
        }
    }
    private IEnumerator ProduceEnergyRoutine()
    {
        // ������ ���� ��ƾ
        while (true)
        {
            while (energyNowValue < energyTotalValue)
            {
                // ���� ������ ���� �ִ밪���� ���� ��
                yield return new WaitForSeconds(energyProductTime); // ���� �ð� ���� ���
                energyNowValue += energyProductValue; // ���� ������ �� ����
                if (liquid != null)
                {
                    // ��ü ���̴� �ѷ� ���
                    liquid.fillAmount = (float)MathInfVal.Clamp01(1 - (energyNowValue / energyTotalValue));
                }
            }

            // ���� ������ ���� 0�� �ɶ����� ���
            yield return new WaitUntil(() => energyNowValue == 0);
        }
    }

    public void AddEnergyValue()
    {
        // ������ ȹ�� �޼���
        if (energyNowValue > 10)
        {
            // ���� �������� 10 �ʰ����� ȹ�氡��
            ResourceManager.instance.AddResource(ResourceManager.ResourceType.Energy, energyNowValue); // ������ �ڿ� ȹ��
            energyNowValue = 0; // ���� ������ �ʱ�ȭ
            if (liquid != null)
            {
                // ��ü ���̴� �ѷ� ���
                liquid.fillAmount = (float)MathInfVal.Clamp01(1 - (energyNowValue / energyTotalValue));
            }
        }
        else Debug.Log("Not Enough Energy");
    }

    public void SaveArtifactValue(ArtifactType artifactType)
    {
        // ����� ��Ƽ��Ʈ �� ����
        artifactValue[artifactType] += energyNowValue;
        Debug.Log(artifactType + " Ÿ�� ���� ��: " + artifactValue[artifactType]);
    }

}
