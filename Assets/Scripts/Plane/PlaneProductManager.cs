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


    private QueueBehavior queueController; // ť ��Ʈ�ѷ�

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
    public Queue<PlaneDataTable> planeQueue = new Queue<PlaneDataTable>(); // ����ü ť
    public Queue<PlaneDataTable> hangerQueue = new Queue<PlaneDataTable>(); // �ݳ��� ť

    public InfVal sumPrice = 0; // ����ü �� �ǸŰ���

    public void EnqueuePlane(PlaneDataTable planeType)
    {
        // ����ü ��ť
        planeQueue.Enqueue(planeType);
        if (planeQueue.Count == 1 ) queueController.StartProductQueue();
    }

    public void DequeuePlane()
    {
        if (planeQueue.Count > 0)
        {
            Destroy(standByContent.content.GetChild(0).gameObject); // ��� ť�� ù��° ������Ʈ �ı�
        }
        planeQueue.Dequeue(); // ����ü ��ť
    }

    public void EnqueueDisplay(GameObject buttonPrefab)
    {
        // ������ ����
        GameObject planeButton = Instantiate(buttonPrefab, standByContent.content);
    }
    public void HangerAddElement(PlaneDataTable planeType)
    {
        // �ݳ��� ��� �߰� �޼���
        hangerQueue.Enqueue(planeType); // �ݳ��� ť ����
        HangerDisplayIcon(planeType.planeIconPrefab); // �ݳ��� ���÷��� ������

        SumPlanePrice(planeType); // �ǸŰ��� �ջ�
    }
    public void HangerClearElement()
    {
        // �ݳ��� ��� Ŭ���� �޼���
        if (hangerQueue.Count > 0)
        {
            ResourceManager.instance.AddResource(ResourceManager.ResourceType.Gold, sumPrice); // ��� ȹ��
            sumPrice = 0; // �ǸŰ��� �ʱ�ȭ
            goldText.text = sumPrice.ToString(); // �ǸŰ��� �ؽ�Ʈ ����

            hangerQueue.Clear(); // �ݳ��� ť Ŭ����

            foreach (Transform child in HangerContent.transform)
            {
                // �ݳ��� ��� ����
                Destroy(child.gameObject);
            }

            if (queueController.ProcessAbailable())
            {
                // �ݳ��� �����ڸ��� ������
                queueController.StartCoroutine("DescreaseTimeCoroutine"); // �ٽ� ��� ť Ȱ��ȭ
            }
            ResourceManager.instance.SaveResourceData(); // �ڿ� ������ ����
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
        // �ݳ��� �ǸŰ��� �ջ�
        sumPrice += planeData.planeValue;
        goldText.text = sumPrice.ToString();
    }



}
