using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMesh;
    public StructItemManager.ItemType itemType; // �������� ������ ����

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
