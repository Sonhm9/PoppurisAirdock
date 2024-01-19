using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlaneProductController : MonoBehaviour
{
    public PlaneProductManager.planeType planeType; // ����ü�� ������ ����
    private PlaneDataTable planeData;
    public GameObject planeDisplayPrefab; // ����ü ������
    public TextMeshProUGUI timeText; // �ð� �ؽ�Ʈ
     
    public StructItemManager.ItemType[] itemType; // �Ҹ��� ������ ����
    public int[] itemConsumeCount; // �Ҹ� ���� ����

    void Start()
    {
        planeData = GetComponent<ConsumablePlane>().planeDataTable;
    }

    public void ProductButton()
    {
        for (int i = 0; i < itemType.Length; i++)
        {
            // ������ �Ҹ𰡴� Ȯ��
            StructItemManager.instance.CheckItemCount(itemType[i], itemConsumeCount[i]);
            if (!StructItemManager.instance.itemConsumeSuccess)
            {
                break;
            }
        }
        if(PlaneProductManager.instance.planeQueue.Count < PlaneProductManager.instance.MaxContentIndex && StructItemManager.instance.itemConsumeSuccess)
        {
            for (int i = 0; i < itemType.Length; i++)
            {
                // ������ �Ҹ�
                StructItemManager.instance.ConsumeItem(itemType[i], itemConsumeCount[i]);
            }
            PlaneProductManager.instance.EnqueueDisplay(timeText, planeDisplayPrefab); // ��⿭�� ������ ���÷���
            PlaneProductManager.instance.EnqueuePlane(planeData); // ť�� plane ����

            Debug.Log("Ÿ��:" + planeType + " ����:" + PlaneProductManager.instance.planeQueue.Count);
        }
    }

    public void PlaneFlight()
    {
        GameObject flightPlane = Instantiate(planeData.planePrefab);
    }


   
}
