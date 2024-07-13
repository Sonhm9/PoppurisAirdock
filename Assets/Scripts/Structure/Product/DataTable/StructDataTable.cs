using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "ScriptableObjects/StructDataTable")]
public class StructDataTable : ScriptableObject
{
    public string buildingType; // �ǹ� �̸�
    public string itemName; // ��� �̸�
    public Sprite buildingimage; // �ǹ� �̹���
    public int animationState; // �ִϸ��̼� ����
    public float productTime; // ���� �ð�
    public int number; // ��� �ѹ�

}
