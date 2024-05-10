using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] GameObject rayout;

    [SerializeField] Camera cam; // ī�޶�


    /* Swipe ���� ���� */
    [Header("Swipe Properties")]

    [Tooltip("���������� ���� ī�޶� ������ �ӵ�")]
    private float swipeMoveSpeed = 0.005f;

    [Tooltip("�ּ� �������� �Ÿ�")]
    private  float minSwipeDistance = 1f;


    private float minPosX = -20f; // �ּ� x ��ǥ
    private float maxPosX = 20f; // �ִ� x ��ǥ

    private float minPosZ = -70f; // �ּ� z ��ǥ
    private float maxPosZ = 19f; // �ִ� z ��ǥ

    private float minPosY = 15f;



    private Vector2 swipeStartPosition;
    private bool isSwiping = false;

    private float CamHeightSize;
    private float CamHalfWidthsize;


    /* Pinch ���� ���� */
    [Header("Pinch Properties")]

    [Tooltip("ī�޶� �ּ� ������")]
    private float minScale = 4f;

    [Tooltip("ī�޶� �ִ� ������")]
    private float maxScale = 10f;

    [Tooltip("�� �ӵ�")]
    private float orthoZoomSpeed = 0.005f;

    private bool isPinch = false;

    float setCameraSize = 7f; // ī�޶� �ʱ� ������
    Vector3 setCameraPosition = new Vector3(0, 15, -28); // ī�޶� �ʱ� ��ġ


    void Awake()
    {
        Application.targetFrameRate = 30; // 30 ������ ����
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
    /// �������� �޼���
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
    /// �� �� �ƿ� �޼���
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

                //ī�޶� ������ ������
                CamHeightSize = cam.orthographicSize;
                CamHalfWidthsize = cam.aspect * CamHeightSize;

                if (touch0.phase == TouchPhase.Ended && touch1.phase == TouchPhase.Ended) isPinch = false;
            }
        }
    }


    /// <summary>
    /// ��ġ �޼���
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
                // �ǹ��� Ŭ��������
                BuildingInteractable interactable = clickedObject.GetComponent<BuildingInteractable>();
                if (interactable != null)
                {
                    interactable.OnBuildingClick();
                }
            }
            else if(clickedObject.CompareTag("BuildPosition") && BuildPositionManager.instance.buildModeState == true)
            {
                Debug.Log("buildposition");
                // ������ ��Ȳ�϶�
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
    /// ī�޶� �Ѱ� �޼���
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


