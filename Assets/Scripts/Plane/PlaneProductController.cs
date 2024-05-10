using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlaneProductController : MonoBehaviour
{
    [SerializeField] PlaneProductManager.planeType planeType; // ����ü�� ������ ����
    private PlaneDataTable planeData;
    [SerializeField] GameObject planeDisplayPrefab; // ����ü ������

    [SerializeField] StructItemManager.ItemType[] itemType; // �Ҹ��� ������ ����
    [SerializeField] int[] itemConsumeCount; // �Ҹ� ���� ����

    void Start()
    {
        planeData = GetComponent<ConsumablePlane>().planeDataTable;
    }

    public void ProductButton()
    {
        if (PlaneProductManager.instance.planeQueue.Count < PlaneProductManager.instance.MaxPlaneQueueIndex)
        {
            if (StructItemManager.instance.CheckItemCount(itemType, itemConsumeCount))
            {
                // ������ �Ҹ� ���� ����
                StructItemManager.instance.ConsumeItem(itemType, itemConsumeCount); // ������ �Ҹ�
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
