using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCheckerController : MonoBehaviour
{
    public Transform timePicker; // 게이지 오브젝트 (원형 게이지)

    public void UpdateGauge(float produceTime)
    {
        // 게이지 회전 각도 계산 (0도: 시작, 360도: 완료)
        float angle = 360f * produceTime;
        // 게이지 오브젝트의 로컬 회전 값을 업데이트
        timePicker.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
