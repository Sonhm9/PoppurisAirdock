using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTextController : MonoBehaviour
{

    [SerializeField] private TextMeshPro textMesh;
    public StructManager.ItemType itemType; // 아이템을 정의할 변수

    private void OnEnable()
    {
        textMesh = GetComponent<TextMeshPro>();

        SetLevelText(itemType, StructManager.instance.structLevel[itemType]); // 레벨 텍스트 갱신
        StructManager.instance.OnLevelChanged += SetLevelText; // 레벨 변화 이벤트 구독
    }

    public void SetLevelText(StructManager.ItemType type, int newValue)
    {
        // 레벨 텍스트 갱신
        textMesh.text = StructManager.instance.structLevel[itemType].ToString();
    }

}
