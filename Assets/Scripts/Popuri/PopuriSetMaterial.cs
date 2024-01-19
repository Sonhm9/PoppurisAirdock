using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopuriSetMaterial : MonoBehaviour
{
    [SerializeField]
    private Material[] popuriUVs;
    void Awake()
    {
        RandomSetMaterial(popuriUVs);
    }

    void RandomSetMaterial(Material[] materials)
    {
        Renderer renderer = GetComponent<Renderer>();
        int result = Random.Range(0, materials.Length);
        renderer.material = materials[result];
    }

}
