using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBehavior : MonoBehaviour
{
    public StructManager.ItemType upgradeItemType;
    public void OnUpgradeButtonClick()
    {
        // 업그레이드 버튼을 누르면
        if (ResourceManager.instance.GetResourceValue(ResourceManager.ResourceType.Gold)>=UpgradeManager.instance.upgradeCost[upgradeItemType])
        {
            // 자원 소모가 가능할 때
            StructManager.instance.StructLevelUP(upgradeItemType); // 재료 레벨업
            UpgradeManager.instance.MultiplyUpgradeCost(upgradeItemType); // 업그레이드 비용 증가
            ResourceManager.instance.RemoveResource(ResourceManager.ResourceType.Gold, UpgradeManager.instance.upgradeCost[upgradeItemType]); // 자원 소모
            UpgradeManager.instance.FindMaxUpgradeCost(); // 재탐색
        }
    }
    public void GetUpgradeItem(StructManager.ItemType itemType)
    {
        upgradeItemType = itemType;
    }

}
