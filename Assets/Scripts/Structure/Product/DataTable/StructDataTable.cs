using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "ScriptableObjects/StructDataTable")]
public class StructDataTable : ScriptableObject
{
    public string buildingType; // 건물 이름
    public string itemName; // 재료 이름
    public Sprite buildingimage; // 건물 이미지
    public int animationState; // 애니메이션 상태
    public float productTime; // 생산 시간
    public int number; // 재료 넘버

}
