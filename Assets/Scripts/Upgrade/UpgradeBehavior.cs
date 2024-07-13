using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBehavior : MonoBehaviour
{
    public StructManager.ItemType upgradeItemType;
    public void OnUpgradeButtonClick()
    {
        // ���׷��̵� ��ư�� ������
        if (ResourceManager.instance.GetResourceValue(ResourceManager.ResourceType.Gold)>=UpgradeManager.instance.upgradeCost[upgradeItemType])
        {
            // �ڿ� �Ҹ� ������ ��
            StructManager.instance.StructLevelUP(upgradeItemType); // ��� ������
            UpgradeManager.instance.MultiplyUpgradeCost(upgradeItemType); // ���׷��̵� ��� ����
            ResourceManager.instance.RemoveResource(ResourceManager.ResourceType.Gold, UpgradeManager.instance.upgradeCost[upgradeItemType]); // �ڿ� �Ҹ�
            UpgradeManager.instance.FindMaxUpgradeCost(); // ��Ž��
        }
    }
    public void GetUpgradeItem(StructManager.ItemType itemType)
    {
        upgradeItemType = itemType;
    }

}
