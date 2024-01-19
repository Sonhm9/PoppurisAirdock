using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPositionManager : MonoBehaviour
{
    public GameObject buildCancleButton; // 건설 취소 버튼
    public MonoBehaviour cameraMove; // 카메라 제어
    public GameObject buildScrollView; // 빌드 스크롤뷰
    public GameObject structurePrefab; // 건물 프리팹
    public SpriteRenderer mainMap; // 메인 맵
    public GameObject clickRayout;

    public List<GameObject> currentPlacePositions = new List<GameObject>(); // 현재 배치된 포지션
    public List<GameObject> EmptyPositions = new List<GameObject>(); // 빈 포지션

    private int widthOrHeight = 0; // 가로0 세로1

    public List<GameObject> buildPosition = new List<GameObject>(); // 기본 포지션


    [HideInInspector]
    private int buildCount = 6; // 확장 갯수

    [HideInInspector]
    public bool buildModeState = false; // 빌드 모드

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
    // 싱글턴

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
        // 클릭된 오브젝트의 자식으로 프리팹 배치
        if (EmptyPositions.Contains(clickObject))
        {
            GameObject structure = Instantiate(structurePrefab, clickObject.transform);
            currentPlacePositions.Add(clickObject);
            EmptyPositions.Remove(clickObject);
            // 클릭한 오브젝트로 배치된 포지션으로 변경

            CancleBuildMode();
            UIManager.instance.CloseUI();
            // 배치모드 종료 후 UI 닫기
        }
        if (EmptyPositions.Count == 0)
        {
            // 빈 포지션이 없을때 확장
            UpgradeBuildPosition();
        }


    }
    private void UpgradeBuildPosition()
    {
        // 단계에 따라 포지션 확장
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
        UIManager.instance.CloseUI(); // UI 닫기 및 카메라 제어 가능상태
        UIManager.instance.HideButtons(); // 버튼 숨김
        buildCancleButton.SetActive(true); // 빌드 취소 버튼 활성화

        foreach (GameObject position in EmptyPositions)
        {
            // 빈 포지션 레이아웃 활성화
            SpriteRenderer rayout = position.GetComponent<SpriteRenderer>();
            rayout.enabled = true;
        }
        buildModeState = true; // 빌드모드 활성화
    }

    public void CancleBuildMode()
    {
        buildScrollView.SetActive(true); // 빌드 스크롤뷰 다시 활성화
        cameraMove.enabled = false; // 카메라 비활성화
        UIManager.instance.popUpBackground.SetActive(true); // 백그라운드 이미지 활성화
        UIManager.instance.cancleButton.SetActive(true);
        UIManager.instance.activeUI = buildScrollView; // 활성화 UI에 빌드 스크롤뷰

        foreach (GameObject position in EmptyPositions)
        {
            // 빈 포지션 레이아웃 비활성화
            SpriteRenderer rayout = position.GetComponent<SpriteRenderer>();
            rayout.enabled = false;
        }

        buildModeState = false; // 빌드모드 비활성화

        buildCancleButton.SetActive(false); // 빌드 취소 버튼 비활성화
    }




}
