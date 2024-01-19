using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPositionManager : MonoBehaviour
{
    public GameObject buildCancleButton; // �Ǽ� ��� ��ư
    public MonoBehaviour cameraMove; // ī�޶� ����
    public GameObject buildScrollView; // ���� ��ũ�Ѻ�
    public GameObject structurePrefab; // �ǹ� ������
    public SpriteRenderer mainMap; // ���� ��
    public GameObject clickRayout;

    public List<GameObject> currentPlacePositions = new List<GameObject>(); // ���� ��ġ�� ������
    public List<GameObject> EmptyPositions = new List<GameObject>(); // �� ������

    private int widthOrHeight = 0; // ����0 ����1

    public List<GameObject> buildPosition = new List<GameObject>(); // �⺻ ������


    [HideInInspector]
    private int buildCount = 6; // Ȯ�� ����

    [HideInInspector]
    public bool buildModeState = false; // ���� ���

    private static object _lock = new object();
    private static BuildPositionManager _instance = null;
    public static BuildPositionManager instance
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
                    GameObject obj = new GameObject("BuildPositionManager ");
                    obj.AddComponent<BuildPositionManager>();
                    _instance = obj.GetComponent<BuildPositionManager>();
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

    private void Awake()
    {
        _instance = this;
    }
    private void OnDestroy()
    {
        applicationQuitting = true;
    }
    public void PlaceBuildStructure(GameObject clickObject)
    {
        // Ŭ���� ������Ʈ�� �ڽ����� ������ ��ġ
        if (EmptyPositions.Contains(clickObject))
        {
            GameObject structure = Instantiate(structurePrefab, clickObject.transform);
            currentPlacePositions.Add(clickObject);
            EmptyPositions.Remove(clickObject);
            // Ŭ���� ������Ʈ�� ��ġ�� ���������� ����

            CancleBuildMode();
            UIManager.instance.CloseUI();
            // ��ġ��� ���� �� UI �ݱ�
        }
        if (EmptyPositions.Count == 0)
        {
            // �� �������� ������ Ȯ��
            UpgradeBuildPosition();
        }


    }
    private void UpgradeBuildPosition()
    {
        // �ܰ迡 ���� ������ Ȯ��
        if (buildPosition.Count > 0)
        {
            MainmapScaleController mainmapScaleController = mainMap.GetComponent<MainmapScaleController>();
            switch (widthOrHeight)
            {
                case (0):
                    mainmapScaleController.WidthUpgrade();
                    break;

                case (1):
                    mainmapScaleController.HeightUpgrade();
                    break;
            }

            for (int i = 0; i < buildCount; i++)
            {
                Debug.Log(EmptyPositions.Count);
                EmptyPositions.Add(buildPosition[0]);
                buildPosition.Remove(buildPosition[0]);
                Debug.Log(EmptyPositions.Count);

            }

            widthOrHeight += 1;
            if (widthOrHeight > 1) widthOrHeight = 0;
        } 
    }
    public void SetBuildMode()
    {
        UIManager.instance.CloseUI(); // UI �ݱ� �� ī�޶� ���� ���ɻ���
        UIManager.instance.HideButtons(); // ��ư ����
        buildCancleButton.SetActive(true); // ���� ��� ��ư Ȱ��ȭ

        foreach (GameObject position in EmptyPositions)
        {
            // �� ������ ���̾ƿ� Ȱ��ȭ
            SpriteRenderer rayout = position.GetComponent<SpriteRenderer>();
            rayout.enabled = true;
        }
        buildModeState = true; // ������ Ȱ��ȭ
    }

    public void CancleBuildMode()
    {
        buildScrollView.SetActive(true); // ���� ��ũ�Ѻ� �ٽ� Ȱ��ȭ
        cameraMove.enabled = false; // ī�޶� ��Ȱ��ȭ
        UIManager.instance.popUpBackground.SetActive(true); // ��׶��� �̹��� Ȱ��ȭ
        UIManager.instance.cancleButton.SetActive(true);
        UIManager.instance.activeUI = buildScrollView; // Ȱ��ȭ UI�� ���� ��ũ�Ѻ�

        foreach (GameObject position in EmptyPositions)
        {
            // �� ������ ���̾ƿ� ��Ȱ��ȭ
            SpriteRenderer rayout = position.GetComponent<SpriteRenderer>();
            rayout.enabled = false;
        }

        buildModeState = false; // ������ ��Ȱ��ȭ

        buildCancleButton.SetActive(false); // ���� ��� ��ư ��Ȱ��ȭ
    }




}
