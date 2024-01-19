using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



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

    public ScrollRect standByContent; // ��⿭ ������
    //public GameObject standByPrefab;

    [HideInInspector]
    public int MaxContentIndex = 7; // �ִ� ��⿭ �ε���

    private QueueController queueController;

    private void Awake()
    {
        queueController = GetComponent<QueueController>();
        _instance = this;
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
    public Queue<PlaneDataTable> planeQueue = new Queue<PlaneDataTable>();

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
            //GameObject standBy = Instantiate(standByPrefab, standByContent.content);
            //standBy.transform.SetSiblingIndex(MaxContentIndex);
            // ��⿭ 0���� �����ϰ� �������� �� ��⿭�߰�
        }
    }

    public void EnqueueDisplay(TextMeshProUGUI timetext,GameObject buttonPrefab)
    {
        GameObject planeButton = Instantiate(buttonPrefab, standByContent.content);
        planeButton.transform.SetParent(standByContent.content.GetChild(planeQueue.Count));
        planeButton.transform.localPosition = new Vector3(0, 0, 0);
        // ������ ����
    }

    
    /*public void MaxContentUpgrade()
    {
        // �ִ� ��⿭ ĭ �ø�
        GameObject standBy = Instantiate(standByPrefab, standByContent.content);
        standBy.transform.SetSiblingIndex(MaxContentIndex);
        MaxContentIndex++;

    }*/



}
