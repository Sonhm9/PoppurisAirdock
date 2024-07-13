using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InfiniteValue;




public class PlaneProductManager : MonoBehaviour
{
    private static object _lock = new object();

    private static PlaneProductManager _instance = null;
    public static PlaneProductManager instance
    {
        get
        {
            if (applicationQuitting)
            {
                return null;
            }
            lock (_lock)
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("PlaneProductManager ");
                    obj.AddComponent<PlaneProductManager>();
                    _instance = obj.GetComponent<PlaneProductManager>();
                }
                return _instance;
            }
        }
        set
        {
            _instance = value;
        }
    }
    private static bool applicationQuitting = false;
    // 싱글톤

    [SerializeField] public ScrollRect standByContent; // 대기열 콘텐츠
    [SerializeField] GameObject HangerContent; // 격납고 콘텐츠

    [SerializeField] TextMeshProUGUI goldText; // 판매가격 텍스트

    [HideInInspector] public int MaxPlaneQueueIndex = 7; // 최대 비행체 대기열 인덱스
    [HideInInspector] public int MaxHangerIndex = 25; // 최대 겹납고 대기열 인덱스


    private QueueBehavior queueController; // 큐 컨트롤러

    private void Awake()
    {
        queueController = GetComponent<QueueBehavior>();
        _instance = this;

        goldText.text = sumPrice.ToString();
    }
    private void OnDestroy()
    {
        applicationQuitting = true;
    }
    public enum planeType
    {
        BICHA,
        AIRBALLON,
        AIRSHIP,
        STEAM_GLIDER,
        IRON_AIRSHIP,
        HELICOPTER,
        IRON_GLIDER
    }
    public Queue<PlaneDataTable> planeQueue = new Queue<PlaneDataTable>(); // 비행체 큐
    public Queue<PlaneDataTable> hangerQueue = new Queue<PlaneDataTable>(); // 격납고 큐

    public InfVal sumPrice = 0; // 비행체 총 판매가격

    public void EnqueuePlane(PlaneDataTable planeType)
    {
        // 비행체 인큐
        planeQueue.Enqueue(planeType);
        if (planeQueue.Count == 1 ) queueController.StartProductQueue();
    }

    public void DequeuePlane()
    {
        if (planeQueue.Count > 0)
        {
            Destroy(standByContent.content.GetChild(0).gameObject); // 상단 큐의 첫번째 오브젝트 파괴
        }
        planeQueue.Dequeue(); // 비행체 디큐
    }

    public void EnqueueDisplay(GameObject buttonPrefab)
    {
        // 아이콘 삽입
        GameObject planeButton = Instantiate(buttonPrefab, standByContent.content);
    }
    public void HangerAddElement(PlaneDataTable planeType)
    {
        // 격납고 요소 추가 메서드
        hangerQueue.Enqueue(planeType); // 격납고 큐 삽입
        HangerDisplayIcon(planeType.planeIconPrefab); // 격납고 디스플레이 아이콘

        SumPlanePrice(planeType); // 판매가격 합산
    }
    public void HangerClearElement()
    {
        // 격납고 요소 클리어 메서드
        if (hangerQueue.Count > 0)
        {
            ResourceManager.instance.AddResource(ResourceManager.ResourceType.Gold, sumPrice); // 골드 획득
            sumPrice = 0; // 판매가격 초기화
            goldText.text = sumPrice.ToString(); // 판매가격 텍스트 갱신

            hangerQueue.Clear(); // 격납고 큐 클리어

            foreach (Transform child in HangerContent.transform)
            {
                // 격납고 요소 삭제
                Destroy(child.gameObject);
            }

            if (queueController.ProcessAbailable())
            {
                // 격납고에 여유자리가 있을때
                queueController.StartCoroutine("DescreaseTimeCoroutine"); // 다시 대기 큐 활성화
            }
            ResourceManager.instance.SaveResourceData(); // 자원 데이터 저장
        }
        
    }

    public void HangerDisplayIcon(GameObject hangerIcon)
    {
        //격납고 아이콘 디스플레이
        GameObject icon = Instantiate(hangerIcon);
        icon.transform.SetParent(HangerContent.transform, false);
    }
    
    public void SumPlanePrice(PlaneDataTable planeData)
    {
        // 격납고 판매가격 합산
        sumPrice += planeData.planeValue;
        goldText.text = sumPrice.ToString();
    }



}
