using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StructItemTextController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] StructManager.ItemType itemType; // �������� ������ ����
    [SerializeField] int itemCount; // ������ ���� �ʿ䰹��

    private void OnEnable()
    {
        if (StructManager.instance.itemCount[itemType] < itemCount)
        {
            textMesh.color = new Color32(229, 40, 40, 255);
        }
        textMesh.text = StructManager.instance.itemCount[itemType].ToString() + '/' + itemCount.ToString();
        StructManager.instance.OnItemChanged += UpdateStructItemText;
    }

    
    private void UpdateStructItemText(StructManager.ItemType type, int newValue)
    {
        if(StructManager.instance.itemCount[itemType]< itemCount)
        {
            textMesh.color = new Color32(229, 40, 40, 255);
        }
        else
        {
            textMesh.color = new Color32(53, 53, 53, 255);
        }
        textMesh.text = StructManager.instance.itemCount[itemType].ToString() + '/' + itemCount.ToString();
    }
}
