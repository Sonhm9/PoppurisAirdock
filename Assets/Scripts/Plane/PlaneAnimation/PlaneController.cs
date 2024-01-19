using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public float speed = 1f; // 비행체 속도
    private void Start()
    {
        float randomPosition = Random.Range(3f, 7f); // y축 난수

        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 spawnPosition = new Vector3(cameraPosition.x+5f, randomPosition, 10f);
        transform.position = spawnPosition;  // 현재 카메라 기준 오른쪽에 생성

    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        // 왼쪽으로 이동

        if (transform.position.x < -BackgroundLoop.backgroundWidth / 2 - 2f)
        {
            Destroy(gameObject);
        }
        // 백그라운드 범위 벗어나면 파괴
    }
}
