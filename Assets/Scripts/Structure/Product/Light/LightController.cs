using UnityEngine;

public class LightController : MonoBehaviour
{
    private static bool _isLightsOn=false;
    private void Start()
    {
        SetLightsState(_isLightsOn);
    }
    public static void ToggleLights(bool _nightState)
    {
        _isLightsOn = _nightState;
        LightController[] allInstance = FindObjectsOfType<LightController>();
        foreach(LightController instance in allInstance)
        {
            instance.SetLightsState(_isLightsOn);
        }
    }
    private void SetLightsState(bool lightsOn)
    {
        // �ڽ� ������Ʈ���� Ȱ��ȭ ���� ����
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(lightsOn);
        }
    }
}
