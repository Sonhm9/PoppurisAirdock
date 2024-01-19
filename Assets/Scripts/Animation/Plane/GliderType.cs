using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderType : MonoBehaviour
{
    private float rotationSpeed = 5.0f; // ȸ�� �ӵ� (degrees per second)
    private float maxRotationAngle = 35.0f; // �ִ� ȸ�� ���� (��������)

    private float currentRotationAngle = 0.0f;
    private int rotationDirection = 1; // ȸ�� ���� (-1 �Ǵ� 1)

    void Update()
    {
        // ȸ�� ������ �����ϰ� �ִ� ȸ�� ������ �����ϸ� ������ �ݴ�� ����
        currentRotationAngle += rotationSpeed * Time.deltaTime * rotationDirection;

        if (Mathf.Abs(currentRotationAngle) >= maxRotationAngle)
        {
            rotationDirection *= -1;
        }

        // X ���� �߽����� ȸ��
        transform.rotation = Quaternion.Euler(currentRotationAngle, 0, 0);
    }
}
