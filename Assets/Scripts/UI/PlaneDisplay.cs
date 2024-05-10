using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaneDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeRemainingText; // UI Text ���

    public void DisplayTime(float remainingTime)
    {
        string timeRemainingString = "";

        // �ð�, ��, �� ������ �и��Ͽ� ǥ��
        if (remainingTime >= 3600)
        {
            int hours = Mathf.FloorToInt(remainingTime / 3600);
            timeRemainingString += hours.ToString() + "�ð� ";
            remainingTime %= 3600;
        }
        if (remainingTime >= 60)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            timeRemainingString += minutes.ToString() + "�� ";
            remainingTime %= 60;
        }
        if (remainingTime > 0)
        {
            timeRemainingString += remainingTime.ToString("F0") + "��";
        }

        timeRemainingText.text = "���� �ð�: " + timeRemainingString;
    }
}
