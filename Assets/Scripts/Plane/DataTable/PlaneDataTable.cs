using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlaneDataTable")]
public class PlaneDataTable : ScriptableObject
{
    public string planeName; // 비행체 이름
    public GameObject planePrefab; // 비행체 프리팹
    public int productTime; // 생산 시간
}
