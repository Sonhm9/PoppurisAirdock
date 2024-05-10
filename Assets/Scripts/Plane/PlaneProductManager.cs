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


    private QueueController queueController; // 큐 컨트롤러
    private PlanePrice price; // 비행체 가격표 스크립트

    private void Awake()
    {
        queueController = GetComponent<QueueController>();
        _instance = this;

        InitializedPlanePrice();
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
        GLIDER,
        MINIPLANE,
        IMPAIRSHIP,
        HELICOPTER
    }
    public Queue<PlaneDataTable> planeQueue = new Queue<PlaneDataTable>(); // 비행체 큐
    public Queue<PlaneDataTable> hangerQueue = new Queue<PlaneDataTable>(); // 격납고 큐

    private Dictionary<planeType, InfVal> planePrice = new Dictionary<planeType, InfVal>(); // 비행체 가격 딕셔너리


    private InfVal sumPrice = 0; // 비행체 총 판매가격

    public void InitializedPlanePrice()
    {
        price = GetComponent<PlanePrice>();
        planePrice[PlaneProductManager.planeType.BICHA] = price.BICHA_PRC;
        planePrice[PlaneProductManager.planeType.AIRBALLON] = price.AIRBALLON_PRC;
        planePrice[PlaneProductManager.planeType.AIRSHIP] = price.AIRSHIP_PRC;
        planePrice[PlaneProductManager.planeType.GLIDER] = price.GLIDER_PRC;
        planePrice[PlaneProductManager.planeType.MINIPLANE] = price.MINIPLANE_PRC;
        planePrice[PlaneProductManager.planeType.IMPAIRSHIP] = price.IMPAIRSHIP_PRC;
        planePrice[PlaneProductManager.planeType.HELICOPTER] = price.HELICOPTER_PRC;
    }
    public void EnqueuePlane(PlaneDataTable planeType)
    {
        planeQueue.Enqueue(planeType);
        if (planeQueue.Count == 1 ) queueController.StartProductQueue();
    }

    public void DequeuePlane()
    {
        if (planeQueue.Count > 0)
        {
            PlaneDataTable planeType = planeQueue.Dequeue();
            Destroy(standByContent.content.GetChild(0).gameObject);
        }
    }

    public void EnqueueDisplay(GameObject buttonPrefab)
    {
        GameObject planeButton = Instantiate(buttonPrefab, standByContent.content);
        // 아이콘 삽입
    }
    public void HangerAddElement(PlaneDataTable planeType)
    {
        // 격납고 요소 추가
        hangerQueue.Enqueue(planeType); // 격납고 큐 삽입
        HangerDisplayIcon(planeType.planeIconPrefab); // 격납고 디스플레이 아이콘

        SumPlanePrice(planeType); // 판매가격 합산
        Debug.Log(hangerQueue.Count);
    }
    public void HangerClearElement()
    {
        // 격납고 요소 클리어
        ResourceManager.instance.AddResource(ResourceManager.ResourceType.Gold, sumPrice); // 골드 획득
        sumPrice = 0; // 판매가격 초기화
        goldText.text = sumPrice.ToString(); // 판매가격 텍스트 갱신

        hangerQueue.Clear(); // 큐 클리어

        foreach (Transform child in HangerContent.transform)
        {
            // 격납고 요소 삭제
            Destroy(child.gameObject);
        }

        if (queueController.ProcessAbailable())
        {
            // 대기 큐 활성화
            queueController.StartCoroutine("DescreaseTimeCoroutine");
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
        // 비행체 판매가격 더하기
        switch (planeData.planeName)
        {
            case "Bicha":
                sumPrice += planePrice[planeType.BICHA];
                goldText.text = sumPrice.ToString();
                break;
            case "AirBalloon":
                sumPrice += planePrice[planeType.AIRBALLON];
                goldText.text = sumPrice.ToString();
                break;
            case "AirShip":
                sumPrice += planePrice[planeType.AIRSHIP];
                goldText.text = sumPrice.ToString();
                break;
            case "Glider":
                sumPrice += planePrice[planeType.GLIDER];
                goldText.text = sumPrice.ToString();
                break;
            case "MiniPlane":
                sumPrice += planePrice[planeType.MINIPLANE];
                goldText.text = sumPrice.ToString();
                break;
            case "Improved Airship":
                sumPrice += planePrice[planeType.IMPAIRSHIP];
                goldText.text = sumPrice.ToString();
                break;
            case "Helicopter":
                sumPrice += planePrice[planeType.HELICOPTER];
                goldText.text = sumPrice.ToString();
                break;
            default:
                break;
        }
    }
    /*public void MaxContentUpgrade()
    {
        // 최대 대기열 칸 늘림
        GameObject standBy = Instantiate(standByPrefab, standByContent.content);
        standBy.transform.SetSiblingIndex(MaxContentIndex);
        MaxContentIndex++;

    }*/



}
