using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirType : MonoBehaviour
{
    private float moveSpeed = 0.25f; // 이동 속도
    private float maxHeight = 1.5f; // 최대 높이
    private float minHeight = 0f; // 최소 높이

    private float currentHeight = 0.0f;
    private int moveDirection = 1; // 이동 방향 (1 또는 -1)

    void Update()
    {
        // 이동 방향을 변경하고 최대 높이 또는 최소 높이에 도달하면 방향을 반대로 변경
        currentHeight += moveSpeed * Time.deltaTime * moveDirection;

        if (currentHeight >= maxHeight || currentHeight <= minHeight)
        {
            moveDirection *= -1;
        }

        // 물체를 새로운 높이로 이동
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
    }
}
