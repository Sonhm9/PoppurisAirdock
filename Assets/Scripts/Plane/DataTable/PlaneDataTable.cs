using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlaneDataTable")]
public class PlaneDataTable : ScriptableObject
{
    public string planeName; // ����ü �̸�
    public GameObject planePrefab; // ����ü ������
    public int productTime; // ���� �ð�
}
