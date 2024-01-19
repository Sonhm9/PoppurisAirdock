using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMesh;
    private StructDataTable structData;
    public int structLevel;

    void Start()
    {
        structData = GetComponentInParent<Consumable>().structDataTable;
        textMesh = GetComponent<TextMeshPro>();
        SetLevelText();
    }
    public void SetLevelText()
    {
        textMesh.text = structData.buildingLevel.ToString();
    }

}
