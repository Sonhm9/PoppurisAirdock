using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInteractable : MonoBehaviour
{
    public virtual void OnBuildingClick()
    {
        Debug.Log("Building Clicked: " + gameObject.name);
        // TODO: 각 건물의 동작 구현
    }
}
