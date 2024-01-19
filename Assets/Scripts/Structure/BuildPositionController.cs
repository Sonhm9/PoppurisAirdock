using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPositionController : MonoBehaviour
{
    public GameObject structureType;
    public void setStructureType()
    {
        BuildPositionManager.instance.structurePrefab = structureType;
    }
    private void EnterStructureMode()
    {
        setStructureType();
        BuildPositionManager.instance.SetBuildMode();
    }
}
