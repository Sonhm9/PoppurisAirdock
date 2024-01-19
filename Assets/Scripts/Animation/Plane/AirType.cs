using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirType : MonoBehaviour
{
    private float moveSpeed = 0.25f; // �̵� �ӵ�
    private float maxHeight = 1.5f; // �ִ� ����
    private float minHeight = 0f; // �ּ� ����

    private float currentHeight = 0.0f;
    private int moveDirection = 1; // �̵� ���� (1 �Ǵ� -1)

    void Update()
    {
        // �̵� ������ �����ϰ� �ִ� ���� �Ǵ� �ּ� ���̿� �����ϸ� ������ �ݴ�� ����
        currentHeight += moveSpeed * Time.deltaTime * moveDirection;

        if (currentHeight >= maxHeight || currentHeight <= minHeight)
        {
            moveDirection *= -1;
        }

        // ��ü�� ���ο� ���̷� �̵�
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
    }
}
