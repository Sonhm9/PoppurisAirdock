using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    public static float backgroundWidth; // 배경 가로 길이

    [SerializeField]
    private GameObject cloudPrefab; // 구름 프리팹
    [SerializeField]
    private Transform cloudPosition; // 구름 시작 위치


    private void Awake()
    {
        BoxCollider2D backgroundCollider = GetComponent<BoxCollider2D>();

        backgroundWidth = backgroundCollider.size.x;

        GameObject cloud = Instantiate(cloudPrefab,new Vector3(backgroundWidth,cloudPosition.transform.position.y, cloudPosition.transform.position.z),Quaternion.identity);
    }
}
