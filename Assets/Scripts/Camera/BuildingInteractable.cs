using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInteractable : MonoBehaviour
{
    public virtual void OnBuildingClick()
    {
        Debug.Log("Building Clicked: " + gameObject.name);
        // TODO: �� �ǹ��� ���� ����
    }
}
