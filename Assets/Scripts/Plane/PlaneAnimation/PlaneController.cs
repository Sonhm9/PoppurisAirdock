using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public float speed = 1f; // ����ü �ӵ�
    private void Start()
    {
        float randomPosition = Random.Range(3f, 7f); // y�� ����

        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 spawnPosition = new Vector3(cameraPosition.x+5f, randomPosition, 10f);
        transform.position = spawnPosition;  // ���� ī�޶� ���� �����ʿ� ����

    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        // �������� �̵�

        if (transform.position.x < -BackgroundLoop.backgroundWidth / 2 - 2f)
        {
            Destroy(gameObject);
        }
        // ��׶��� ���� ����� �ı�
    }
}
