using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using InfiniteValue;


public class ResourceUIText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] ResourceManager.ResourceType resourceType;

    private void OnEnable()
    {
        ResourceManager.instance.OnResourceChanged += UpdateResourceText;
        UpdateResourceText(resourceType, ResourceManager.instance.GetResourceValue(resourceType));
    }


    private void UpdateResourceText(ResourceManager.ResourceType type, InfVal newValue)
    {
        if (type == resourceType)
        {
            textMesh.text = newValue.ToString();
        }
    }
}
