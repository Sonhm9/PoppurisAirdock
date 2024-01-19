using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    public Button[] buttons; // 버튼 배열

    public GameObject activeUI; // 활성화 된 UI

    public GameObject popUpBackground; // UI 백그라운드 이미지
    public GameObject cancleButton;
    public  MonoBehaviour cameraMove; // 카메라 제어

    private static object _lock = new object();
    private static UIManager _instance = null;
    public static UIManager instance
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
                    GameObject obj = new GameObject("UIManager ");
                    obj.AddComponent<UIManager>();
                    _instance = obj.GetComponent<UIManager>();
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
    private void Start()
    {
        CloseUI();
    }

    public void OpenUI(GameObject UIPanel)
    {
        if (activeUI != null)
        {
            activeUI.SetActive(false);
        }
        activeUI = UIPanel;
        UIPanel.SetActive(true);

        HideButtons();
        popUpBackground.SetActive(true); // 백그라운드 활성화
        cancleButton.SetActive(true);
        cameraMove.enabled = false; // 카메라 비활성화

    }
    // UI를 여는 메서드
    public void CloseUI()
    {
        if (activeUI != null)
        {
            activeUI.SetActive(false);
            activeUI = null;

            DisplayButtons();
            popUpBackground.SetActive(false); // 백그라운드 비활성화
            cancleButton.SetActive(false);
            cameraMove.enabled = true; // 카메라 비활성화

        }
    }
    // UI를 닫는 메서드

    public void NotInteractableButtons()
    {
        foreach(Button button in buttons)
        {
            button.interactable = false;
        }
    }
    // 버튼들을 비활성화

    public void InteractableButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
    // 버튼들을 활성화

    public void HideButtons()
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }
    // 버튼들을 숨김

    public void DisplayButtons()
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }
    // 버튼들을 보임
}
