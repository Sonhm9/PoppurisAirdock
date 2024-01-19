using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISwitchButtonController : MonoBehaviour
{
    public GameObject[] contentPanels; // 버튼에 대응하는 컨텐츠 패널들
    public Button[] buttons; // 버튼 배열
    public GameObject[] buttonsTexts; // 버튼에 해당하는 글자

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
        // 모든 컨텐츠를 비활성화하고 선택한 컨텐츠만 활성화
        foreach (GameObject contentPanel in contentPanels)
        {
            contentPanel.SetActive(false);
        }
        contentPanels[index].SetActive(true);

        // 모든 버튼의 색상을 원래 색상으로 초기화하고 선택한 버튼만 색상 변경
        // 모든 버튼을 활성화하고 선택한 버튼만 비활성화
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
