using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderType : MonoBehaviour
{
    private float rotationSpeed = 5.0f; // 회전 속도 (degrees per second)
    private float maxRotationAngle = 35.0f; // 최대 회전 각도 (양쪽으로)

    private float currentRotationAngle = 0.0f;
    private int rotationDirection = 1; // 회전 방향 (-1 또는 1)

    void Update()
    {
        // 회전 방향을 변경하고 최대 회전 각도에 도달하면 방향을 반대로 변경
        currentRotationAngle += rotationSpeed * Time.deltaTime * rotationDirection;

        if (Mathf.Abs(currentRotationAngle) >= maxRotationAngle)
        {
            rotationDirection *= -1;
        }

        // X 축을 중심으로 회전
        transform.rotation = Quaternion.Euler(currentRotationAngle, 0, 0);
    }
}
