using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopuriController : MonoBehaviour
{
    private Animator popuriAnimator;
    private StructDataTable structData;
    void Start()
    {
        structData = GetComponentInParent<Consumable>().structDataTable;
        popuriAnimator = GetComponent<Animator>();

        popuriAnimator.SetFloat("ActionNumber", structData.animationState); // 건물에 따른 애니메이션
    }
}
