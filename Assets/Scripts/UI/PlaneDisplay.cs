using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaneDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeRemainingText; // UI Text 요소

    public void DisplayTime(float remainingTime)
    {
        string timeRemainingString = "";

        // 시간, 분, 초 단위로 분리하여 표시
        if (remainingTime >= 3600)
        {
            int hours = Mathf.FloorToInt(remainingTime / 3600);
            timeRemainingString += hours.ToString() + "시간 ";
            remainingTime %= 3600;
        }
        if (remainingTime >= 60)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            timeRemainingString += minutes.ToString() + "분 ";
            remainingTime %= 60;
        }
        if (remainingTime > 0)
        {
            timeRemainingString += remainingTime.ToString("F0") + "초";
        }

        timeRemainingText.text = "남은 시간: " + timeRemainingString;
    }
}
