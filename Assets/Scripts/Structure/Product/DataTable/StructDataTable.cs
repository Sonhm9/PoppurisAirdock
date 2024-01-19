using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/StructDataTable")]
public class StructDataTable : ScriptableObject
{
    public string buildingType; // 건물 이름
    public int buildingLevel; // 건물 레벨
    public int animationState; // 애니메이션 상태
    public float productTime; // 생산 시간
}