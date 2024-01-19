using UnityEngine;
using InfiniteValue;

[CreateAssetMenu(menuName = "ScriptableObjects/ArtifactDataTable")]

public class ArtifactDataTable : ScriptableObject
{
    public string buildingType; // 건물 이름
    public int buildingLevel; // 건물 레벨
    public InfVal productValue; // 생산량
}
