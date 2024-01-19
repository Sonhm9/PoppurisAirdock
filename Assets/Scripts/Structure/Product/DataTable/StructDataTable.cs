using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/StructDataTable")]
public class StructDataTable : ScriptableObject
{
    public string buildingType; // �ǹ� �̸�
    public int buildingLevel; // �ǹ� ����
    public int animationState; // �ִϸ��̼� ����
    public float productTime; // ���� �ð�
}