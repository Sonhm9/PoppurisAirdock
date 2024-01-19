using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactTouchBehaviour : BuildingInteractable
{
    public override void OnBuildingClick()
    {
        base.OnBuildingClick();
        ArtifactController artifactController = GetComponentInChildren<ArtifactController>();
        artifactController.AddEnergyValue();
    }
}
