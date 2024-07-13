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
        // ť�� ù��° ��Ҹ� �����ͼ� �ð� ǥ��

        StartCoroutine("DescreaseTimeCoroutine");
        // �ð��� �����ϴ� �ڷ�ƾ

    }

    public void UpdateTimeText(int seconds)
    {
        // �ð��� �ú��ʷ� ���
        int hours = seconds / 3600;
        int minutes = (seconds % 3600) / 60;
        int remainingSeconds = seconds % 60;

        string timeString = "";

        if (hours > 0)
        {
            timeString += hours + "�ð� ";
        }

        if (minutes > 0 || hours > 0)
        {
            timeString += minutes + "�� ";
        }

        timeString += remainingSeconds + "��";

        cloneText.text = timeString;
    }

    public void DisplayTime()
    {
        cloneText = Instantiate(timeText, PlaneProductManager.instance.standByContent.content);
        cloneText.transform.SetParent(PlaneProductManager.instance.standByContent.content.GetChild(0));
        cloneText.transform.localPosition = new Vector3(0, -45, 0);
        // �������� ������ ����
    }

    private IEnumerator DescreaseTimeCoroutine()
    {
        if (PlaneProductManager.instance.hangerQueue.Count < PlaneProductManager.instance.MaxHangerIndex)
        {
            // �ð� ���� �ڷ�ƾ
            isProcessing = true;
            int timeTemp = planeData.productTime;

            while (timeTemp > 0)
            {
                // �ڷ�ƾ ������ ���� �ð�ǥ��
                yield return new WaitForSeconds(1.0f);
                timeTemp -= 1;

                UpdateTimeText(timeTemp);
            }
            GameObject flightPlane = Instantiate(planeData.planePrefab);
            PlaneProductManager.instance.HangerAddElement(planeData);
            PlaneProductManager.instance.DequeuePlane();
            // ����ü ���� �� Dequeue

            yield return new WaitForSeconds(0.01f);

            if (PlaneProductManager.instance.planeQueue.Count > 0)
            {
                StartProductQueue();
            }
            // ���� ���� ����
        }
        else isProcessing = false;
    }

    public bool ProcessAbailable()
    {
        // ������� Ȯ�� �Լ�
        if (isProcessing) return false; // ������ - false
        else return true; // ����ƴ� - true
    }
}
