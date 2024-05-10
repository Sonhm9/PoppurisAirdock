using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonController : MonoBehaviour
{
    [SerializeField] GameObject uiPanelToOpen;
    [SerializeField] UIManager uiManager;

    public void OpenUI()
    {
        uiManager.OpenUI(uiPanelToOpen);
    }
}
