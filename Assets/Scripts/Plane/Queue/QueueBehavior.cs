using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QueueBehavior : MonoBehaviour
{
    [SerializeField] private PlaneDataTable planeData;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI cloneText;
    private bool isProcessing = false;

    public void StartProductQueue()
    {
        planeData = PlaneProductManager.instance.planeQueue.Peek();
        DisplayTime();
        UpdateTimeText(planeData.productTime);
        // 큐의 첫번째 요소를 가져와서 시간 표시

        StartCoroutine("DescreaseTimeCoroutine");
        // 시간을 감소하는 코루틴

    }

    public void UpdateTimeText(int seconds)
    {
        // 시간을 시분초로 계산
        int hours = seconds / 3600;
        int minutes = (seconds % 3600) / 60;
        int remainingSeconds = seconds % 60;

        string timeString = "";

        if (hours > 0)
        {
            timeString += hours + "시간 ";
        }

        if (minutes > 0 || hours > 0)
        {
            timeString += minutes + "분 ";
        }

        timeString += remainingSeconds + "초";

        cloneText.text = timeString;
    }

    public void DisplayTime()
    {
        cloneText = Instantiate(timeText, PlaneProductManager.instance.standByContent.content);
        cloneText.transform.SetParent(PlaneProductManager.instance.standByContent.content.GetChild(0));
        cloneText.transform.localPosition = new Vector3(0, -45, 0);
        // 프리팹의 복제본 삽입
    }

    private IEnumerator DescreaseTimeCoroutine()
    {
        if (PlaneProductManager.instance.hangerQueue.Count < PlaneProductManager.instance.MaxHangerIndex)
        {
            // 시간 감소 코루틴
            isProcessing = true;
            int timeTemp = planeData.productTime;

            while (timeTemp > 0)
            {
                // 코루틴 지연에 의한 시간표시
                yield return new WaitForSeconds(1.0f);
                timeTemp -= 1;

                UpdateTimeText(timeTemp);
            }
            GameObject flightPlane = Instantiate(planeData.planePrefab);
            PlaneProductManager.instance.HangerAddElement(planeData);
            PlaneProductManager.instance.DequeuePlane();
            // 비행체 생산 후 Dequeue

            yield return new WaitForSeconds(0.01f);

            if (PlaneProductManager.instance.planeQueue.Count > 0)
            {
                StartProductQueue();
            }
            // 다음 생산 시작
        }
        else isProcessing = false;
    }

    public bool ProcessAbailable()
    {
        // 생산상태 확인 함수
        if (isProcessing) return false; // 생산중 - false
        else return true; // 생산아님 - true
    }
}
