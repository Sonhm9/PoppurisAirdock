using UnityEngine;
using InfiniteValue;

[CreateAssetMenu(menuName = "ScriptableObjects/ArtifactDataTable")]

public class ArtifactDataTable : ScriptableObject
{
    public string buildingType; // �ǹ� �̸�
    public int buildingLevel; // �ǹ� ����
    public InfVal productValue; // ���귮
}
