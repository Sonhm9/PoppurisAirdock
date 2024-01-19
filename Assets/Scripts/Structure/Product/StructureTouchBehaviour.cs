using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureTouchBehaviour : BuildingInteractable
{
    public override void OnBuildingClick()
    {
        base.OnBuildingClick();
        BuildPositionManager.instance.clickRayout.SetActive(true);
        BuildPositionManager.instance.clickRayout.transform.position = gameObject.transform.position;
    }
    
}
