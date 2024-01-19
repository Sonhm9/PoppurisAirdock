using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonController : MonoBehaviour
{
    public GameObject uiPanelToOpen;
    public UIManager uiManager;

    public void OpenUI()
    {
        uiManager.OpenUI(uiPanelToOpen);
    }
}
