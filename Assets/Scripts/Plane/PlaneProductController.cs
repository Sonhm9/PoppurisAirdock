using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlaneProductController : MonoBehaviour
{
    public PlaneProductManager.planeType planeType; // 비행체를 정의할 변수
    private PlaneDataTable planeData;
    public GameObject planeDisplayPrefab; // 비행체 아이콘
    public TextMeshProUGUI timeText; // 시간 텍스트
     
    public StructItemManager.ItemType[] itemType; // 소모할 아이템 정의
    public int[] itemConsumeCount; // 소모 갯수 정의

    void Start()
    {
        planeData = GetComponent<ConsumablePlane>().planeDataTable;
    }

    public void ProductButton()
    {
        for (int i = 0; i < itemType.Length; i++)
        {
            // 아이템 소모가능 확인
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
                // 아이템 소모
                StructItemManager.instance.ConsumeItem(itemType[i], itemConsumeCount[i]);
            }
            PlaneProductManager.instance.EnqueueDisplay(timeText, planeDisplayPrefab); // 대기열에 아이콘 디스플레이
            PlaneProductManager.instance.EnqueuePlane(planeData); // 큐에 plane 삽입

            Debug.Log("타입:" + planeType + " 갯수:" + PlaneProductManager.instance.planeQueue.Count);
        }
    }

    public void PlaneFlight()
    {
        GameObject flightPlane = Instantiate(planeData.planePrefab);
    }


   
}
