using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollbarController : MonoBehaviour
{
    [SerializeField] Scrollbar scrollbar;
    void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
    }
    private void OnEnable()
    {
        ScrollbarReset();
    }
    public void ScrollbarReset()
    {
        scrollbar.value = 1f;

    }
}
