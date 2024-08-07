using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlaneProductBehavior : MonoBehaviour
{
    [SerializeField] PlaneProductManager.planeType planeType; // 비행체를 정의할 변수
    private PlaneDataTable planeData;
    [SerializeField] GameObject planeDisplayPrefab; // 비행체 아이콘

    [SerializeField] StructManager.ItemType[] itemType; // 소모할 아이템 정의
    [SerializeField] int[] itemConsumeCount; // 소모 갯수 정의

    void Start()
    {
        planeData = GetComponent<ConsumablePlane>().planeDataTable;
    }

    public void OnProductButtonClick()
    {
        if (PlaneProductManager.instance.planeQueue.Count < PlaneProductManager.instance.MaxPlaneQueueIndex)
        {
            // 현재 큐 갯수가 최대개수보다 작을 때
            if (StructManager.instance.CheckItemCount(itemType, itemConsumeCount))
            {
                // 아이템 소모 가능 여부 확인
                StructManager.instance.ConsumeItem(itemType, itemConsumeCount); // 아이템 소모
                PlaneProductManager.instance.EnqueueDisplay(planeDisplayPrefab); // 대기열에 아이콘 디스플레이
                PlaneProductManager.instance.EnqueuePlane(planeData); // 큐에 plane 삽입

                Debug.Log("타입:" + planeType + " 갯수:" + PlaneProductManager.instance.planeQueue.Count);
            }
        }
        
    }

    public void PlaneFlight()
    {
        GameObject flightPlane = Instantiate(planeData.planePrefab);
    }


   
}
