using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMesh;
    public StructItemManager.ItemType itemType; // 아이템을 정의할 변수

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        SetLevelText();
    }
    public void SetLevelText()
    {
        textMesh.text = StructItemManager.instance.structLevel[itemType].ToString();
    }

}
