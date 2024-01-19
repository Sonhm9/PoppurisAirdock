using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIManager : MonoBehaviour
{
    public Button[] buttons; // ��ư �迭

    public GameObject activeUI; // Ȱ��ȭ �� UI

    public GameObject popUpBackground; // UI ��׶��� �̹���
    public GameObject cancleButton;
    public  MonoBehaviour cameraMove; // ī�޶� ����

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
    // �̱���

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
        popUpBackground.SetActive(true); // ��׶��� Ȱ��ȭ
        cancleButton.SetActive(true);
        cameraMove.enabled = false; // ī�޶� ��Ȱ��ȭ

    }
    // UI�� ���� �޼���
    public void CloseUI()
    {
        if (activeUI != null)
        {
            activeUI.SetActive(false);
            activeUI = null;

            DisplayButtons();
            popUpBackground.SetActive(false); // ��׶��� ��Ȱ��ȭ
            cancleButton.SetActive(false);
            cameraMove.enabled = true; // ī�޶� ��Ȱ��ȭ

        }
    }
    // UI�� �ݴ� �޼���

    public void NotInteractableButtons()
    {
        foreach(Button button in buttons)
        {
            button.interactable = false;
        }
    }
    // ��ư���� ��Ȱ��ȭ

    public void InteractableButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
    // ��ư���� Ȱ��ȭ

    public void HideButtons()
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }
    // ��ư���� ����

    public void DisplayButtons()
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }
    // ��ư���� ����
}
