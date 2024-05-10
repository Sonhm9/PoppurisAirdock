using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlaneDataTable")]
public class PlaneDataTable : ScriptableObject
{
    public string planeName; // ����ü �̸�
    public GameObject planePrefab; // ����ü ������
    public GameObject planeIconPrefab; // ����ü ������
    public int productTime; // ���� �ð�
}
