using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTextController : MonoBehaviour
{

    [SerializeField] private TextMeshPro textMesh;
    public StructManager.ItemType itemType; // �������� ������ ����

    private void OnEnable()
    {
        textMesh = GetComponent<TextMeshPro>();

        SetLevelText(itemType, StructManager.instance.structLevel[itemType]); // ���� �ؽ�Ʈ ����
        StructManager.instance.OnLevelChanged += SetLevelText; // ���� ��ȭ �̺�Ʈ ����
    }

    public void SetLevelText(StructManager.ItemType type, int newValue)
    {
        // ���� �ؽ�Ʈ ����
        textMesh.text = StructManager.instance.structLevel[itemType].ToString();
    }

}
