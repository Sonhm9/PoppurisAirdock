using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlaneProductBehavior : MonoBehaviour
{
    [SerializeField] PlaneProductManager.planeType planeType; // ����ü�� ������ ����
    private PlaneDataTable planeData;
    [SerializeField] GameObject planeDisplayPrefab; // ����ü ������

    [SerializeField] StructManager.ItemType[] itemType; // �Ҹ��� ������ ����
    [SerializeField] int[] itemConsumeCount; // �Ҹ� ���� ����

    void Start()
    {
        planeData = GetComponent<ConsumablePlane>().planeDataTable;
    }

    public void OnProductButtonClick()
    {
        if (PlaneProductManager.instance.planeQueue.Count < PlaneProductManager.instance.MaxPlaneQueueIndex)
        {
            // ���� ť ������ �ִ밳������ ���� ��
            if (StructManager.instance.CheckItemCount(itemType, itemConsumeCount))
            {
                // ������ �Ҹ� ���� ���� Ȯ��
                StructManager.instance.ConsumeItem(itemType, itemConsumeCount); // ������ �Ҹ�
                PlaneProductManager.instance.EnqueueDisplay(planeDisplayPrefab); // ��⿭�� ������ ���÷���
                PlaneProductManager.instance.EnqueuePlane(planeData); // ť�� plane ����

                Debug.Log("Ÿ��:" + planeType + " ����:" + PlaneProductManager.instance.planeQueue.Count);
            }
        }
        
    }

    public void PlaneFlight()
    {
        GameObject flightPlane = Instantiate(planeData.planePrefab);
    }


   
}
