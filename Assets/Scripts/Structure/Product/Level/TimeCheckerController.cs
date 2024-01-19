using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCheckerController : MonoBehaviour
{
    public Transform timePicker; // ������ ������Ʈ (���� ������)

    public void UpdateGauge(float produceTime)
    {
        // ������ ȸ�� ���� ��� (0��: ����, 360��: �Ϸ�)
        float angle = 360f * produceTime;
        // ������ ������Ʈ�� ���� ȸ�� ���� ������Ʈ
        timePicker.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
