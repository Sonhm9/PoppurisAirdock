using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StructItemTextController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] StructItemManager.ItemType itemType; // 아이템을 정의할 변수
    [SerializeField] int itemCount; // 아이템 생산 필요갯수

    private void OnEnable()
    {
        if (StructItemManager.instance.itemCount[itemType] < itemCount)
        {
            textMesh.color = new Color32(229, 40, 40, 255);
        }
        textMesh.text = StructItemManager.instance.itemCount[itemType].ToString() + '/' + itemCount.ToString();
        StructItemManager.instance.OnItemChanged += UpdateStructItemText;
    }

    
    private void UpdateStructItemText(StructItemManager.ItemType type, int newValue)
    {
        if(StructItemManager.instance.itemCount[itemType]< itemCount)
        {
            textMesh.color = new Color32(229, 40, 40, 255);
        }
        else
        {
            textMesh.color = new Color32(53, 53, 53, 255);
        }
        textMesh.text = StructItemManager.instance.itemCount[itemType].ToString() + '/' + itemCount.ToString();
    }
}
