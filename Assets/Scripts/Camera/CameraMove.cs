using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] GameObject rayout;

    [SerializeField] Camera cam; // 카메라


    /* Swipe 관련 변수 */
    [Header("Swipe Properties")]

    [Tooltip("스와이프에 따른 카메라 움직임 속도")]
    private float swipeMoveSpeed = 0.005f;

    [Tooltip("최소 스와이프 거리")]
    private  float minSwipeDistance = 1f;


    private float minPosX = -20f; // 최소 x 좌표
    private float maxPosX = 20f; // 최대 x 좌표

    private float minPosZ = -70f; // 최소 z 좌표
    private float maxPosZ = 19f; // 최대 z 좌표

    private float minPosY = 15f;



    private Vector2 swipeStartPosition;
    private bool isSwiping = false;

    private float CamHeightSize;
    private float CamHalfWidthsize;


    /* Pinch 관련 변수 */
    [Header("Pinch Properties")]

    [Tooltip("카메라 최소 사이즈")]
    private float minScale = 4f;

    [Tooltip("카메라 최대 사이즈")]
    private float maxScale = 10f;

    [Tooltip("줌 속도")]
    private float orthoZoomSpeed = 0.005f;

    private bool isPinch = false;

    float setCameraSize = 7f; // 카메라 초기 사이즈
    Vector3 setCameraPosition = new Vector3(0, 15, -28); // 카메라 초기 위치


    void Awake()
    {
        Application.targetFrameRate = 30; // 30 프레임 고정
        cam = GetComponent<Camera>();

        CamHeightSize = cam.orthographicSize;
        CamHalfWidthsize= cam.aspect * CamHeightSize;

        cam.transform.position = setCameraPosition;
        cam.orthographicSize = setCameraSize;

        rayout.SetActive(false);
    }


    private void Update()
    {
        CameraSwipePlayer();
        CameraScalePlayer();
        CameraDistanceLimit(CamHalfWidthsize, CamHeightSize);
    }

    /// <summary>
    /// 스와이프 메서드
    /// </summary>
    private void CameraSwipePlayer()
    {

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    swipeStartPosition = touch.position;
                    isPinch = false;
                    break;

                case TouchPhase.Moved:
                    Vector2 swipeDelta = touch.position - swipeStartPosition;
                    if (!isPinch && swipeDelta.magnitude > minSwipeDistance)
                    {
                        isSwiping = true;
                        Vector3 moveDirection = new Vector3(-swipeDelta.x * swipeMoveSpeed, 0, -swipeDelta.y * swipeMoveSpeed);
                        transform.position += moveDirection;
                    }
                    swipeStartPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    if (!isSwiping) TouchPlayer(touch);
                    isSwiping = false;
                    break;
            }
        }
    }


    /// <summary>
    /// 줌 인 아웃 메서드
    /// </summary>
    private void CameraScalePlayer()
    {
        if (Input.touchCount < 2)
        {
            return;
        }
        else
        {
            if (Input.touchCount == 2)
            {
                isPinch = true;
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                Vector2 touch0Prev = touch0.position - touch0.deltaPosition;
                Vector2 touch1Prev = touch1.position - touch1.deltaPosition;

                float prevTouchDeltaMag = (touch0Prev - touch1Prev).magnitude;
                float touchDeltaMag = (touch0.position - touch1.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                deltaMagnitudeDiff *= orthoZoomSpeed;

                float newOrthoSize = cam.orthographicSize + deltaMagnitudeDiff;
                newOrthoSize = Mathf.Clamp(newOrthoSize, minScale, maxScale);

                cam.orthographicSize = newOrthoSize;

                //카메라 사이즈 재조정
                CamHeightSize = cam.orthographicSize;
                CamHalfWidthsize = cam.aspect * CamHeightSize;

                if (touch0.phase == TouchPhase.Ended && touch1.phase == TouchPhase.Ended) isPinch = false;
            }
        }
    }


    /// <summary>
    /// 터치 메서드
    /// </summary>
    private void TouchPlayer(Touch touch)
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.transform.gameObject;
            if (clickedObject.CompareTag("BuildAble") && BuildPositionManager.instance.buildModeState == false)
            {
                // 건물을 클릭했을때
                BuildingInteractable interactable = clickedObject.GetComponent<BuildingInteractable>();
                if (interactable != null)
                {
                    interactable.OnBuildingClick();
                }
            }
            else if(clickedObject.CompareTag("BuildPosition") && BuildPositionManager.instance.buildModeState == true)
            {
                Debug.Log("buildposition");
                // 빌드모드 상황일때
                GameObject interactable = clickedObject;
                if (interactable != null)
                {
                    BuildPositionManager.instance.PlaceBuildStructure(interactable);
                }

            }
            else BuildPositionManager.instance.clickRayout.SetActive(false);

        }
    }

    /// <summary>
    /// 카메라 한계 메서드
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void CameraDistanceLimit(float width,float height)
    {
        Vector3 temp;
        temp.x = Mathf.Clamp(transform.position.x, minPosX+width, maxPosX-width);
        temp.y = minPosY;
        temp.z = Mathf.Clamp(transform.position.z,minPosZ+(height*2),maxPosZ-(height*2));

        transform.position = temp;
    }

}


