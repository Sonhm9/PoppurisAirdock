using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public static float backgroundWidth; // ��� ���� ����

    [SerializeField]
    private GameObject cloudPrefab; // ���� ������
    [SerializeField]
    private Transform cloudPosition; // ���� ���� ��ġ


    private void Awake()
    {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();

        backgroundWidth = backgroundCollider.size.x;

        GameObject cloud = Instantiate(cloudPrefab,new Vector3(backgroundWidth,cloudPosition.transform.position.y, cloudPosition.transform.position.z),Quaternion.identity);
    }
}
