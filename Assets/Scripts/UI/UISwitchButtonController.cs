using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISwitchButtonController : MonoBehaviour
{
    public GameObject[] contentPanels; // ��ư�� �����ϴ� ������ �гε�
    public Button[] buttons; // ��ư �迭
    public GameObject[] buttonsTexts; // ��ư�� �ش��ϴ� ����

    public ScrollbarController scrollbarController;

    private void Start()
    {
        scrollbarController = GetComponentInChildren<ScrollbarController>();

    }
    private void OnEnable()
    {
        OpenContent(0);
    }
    public void OpenContent(int index)
    {
        // ��� �������� ��Ȱ��ȭ�ϰ� ������ �������� Ȱ��ȭ
        foreach (GameObject contentPanel in contentPanels)
        {
            contentPanel.SetActive(false);
        }
        contentPanels[index].SetActive(true);

        // ��� ��ư�� ������ ���� �������� �ʱ�ȭ�ϰ� ������ ��ư�� ���� ����
        // ��� ��ư�� Ȱ��ȭ�ϰ� ������ ��ư�� ��Ȱ��ȭ
        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = Color.white;
            button.interactable = true;
        }
        buttons[index].GetComponent<Image>().color = Color.red;
        buttons[index].interactable = false;

        foreach (GameObject buttonsText in buttonsTexts)
        {
            buttonsText.SetActive(false);
        }
        buttonsTexts[index].SetActive(true);
    }
    public void FirstOpen()
    {
        scrollbarController.ScrollbarReset();
        OpenContent(0);
    }
    public void SecondOpen()
    {
        scrollbarController.ScrollbarReset();
        OpenContent(1);
    }

    public void ThirdOpen()
    {
        scrollbarController.ScrollbarReset();
        OpenContent(2);
    }

}
