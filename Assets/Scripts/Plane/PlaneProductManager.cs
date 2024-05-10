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
    // �̱���

    [SerializeField] public ScrollRect standByContent; // ��⿭ ������
    [SerializeField] GameObject HangerContent; // �ݳ��� ������

    [SerializeField] TextMeshProUGUI goldText; // �ǸŰ��� �ؽ�Ʈ

    [HideInInspector] public int MaxPlaneQueueIndex = 7; // �ִ� ����ü ��⿭ �ε���
    [HideInInspector] public int MaxHangerIndex = 25; // �ִ� �㳳�� ��⿭ �ε���


    private QueueController queueController; // ť ��Ʈ�ѷ�
    private PlanePrice price; // ����ü ����ǥ ��ũ��Ʈ

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
    public Queue<PlaneDataTable> planeQueue = new Queue<PlaneDataTable>(); // ����ü ť
    public Queue<PlaneDataTable> hangerQueue = new Queue<PlaneDataTable>(); // �ݳ��� ť

    private Dictionary<planeType, InfVal> planePrice = new Dictionary<planeType, InfVal>(); // ����ü ���� ��ųʸ�


    private InfVal sumPrice = 0; // ����ü �� �ǸŰ���

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
        // ������ ����
    }
    public void HangerAddElement(PlaneDataTable planeType)
    {
        // �ݳ��� ��� �߰�
        hangerQueue.Enqueue(planeType); // �ݳ��� ť ����
        HangerDisplayIcon(planeType.planeIconPrefab); // �ݳ��� ���÷��� ������

        SumPlanePrice(planeType); // �ǸŰ��� �ջ�
        Debug.Log(hangerQueue.Count);
    }
    public void HangerClearElement()
    {
        // �ݳ��� ��� Ŭ����
        ResourceManager.instance.AddResource(ResourceManager.ResourceType.Gold, sumPrice); // ��� ȹ��
        sumPrice = 0; // �ǸŰ��� �ʱ�ȭ
        goldText.text = sumPrice.ToString(); // �ǸŰ��� �ؽ�Ʈ ����

        hangerQueue.Clear(); // ť Ŭ����

        foreach (Transform child in HangerContent.transform)
        {
            // �ݳ��� ��� ����
            Destroy(child.gameObject);
        }

        if (queueController.ProcessAbailable())
        {
            // ��� ť Ȱ��ȭ
            queueController.StartCoroutine("DescreaseTimeCoroutine");
        }
    }

    public void HangerDisplayIcon(GameObject hangerIcon)
    {
        //�ݳ��� ������ ���÷���
        GameObject icon = Instantiate(hangerIcon);
        icon.transform.SetParent(HangerContent.transform, false);
    }
    
    public void SumPlanePrice(PlaneDataTable planeData)
    {
        // ����ü �ǸŰ��� ���ϱ�
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
        // �ִ� ��⿭ ĭ �ø�
        GameObject standBy = Instantiate(standByPrefab, standByContent.content);
        standBy.transform.SetSiblingIndex(MaxContentIndex);
        MaxContentIndex++;

    }*/



}
