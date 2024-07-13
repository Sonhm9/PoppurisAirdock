using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveDatas
{
    // 저장할 자원목록
    public string gold = "0";
    public string energy = "0";
    public string diamond = "0";
}
public class DataSaveManager : MonoBehaviour
{
    private static object _lock = new object();
    private static DataSaveManager _instance = null;
    public static DataSaveManager instance
    {
        get
        {
            if (applicationQuitting)
            {
                return null;
            }
            lock (_lock)
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("DataSaveManager ");
                    obj.AddComponent<DataSaveManager>();
                    _instance = obj.GetComponent<DataSaveManager>();
                }
                return _instance;
            }
        }
        set
        {
            _instance = value;
        }
    }
    private static bool applicationQuitting = false;

    public SaveDatas saveDatas; // 저장 데이터
    private string fileName = "SaveFile.es3";

    private void Awake()
    {
        _instance = this;
        ResourceManager.instance.LoadResourceData(); // 자원 저장

    }
    private void OnDestroy()
    {
        applicationQuitting = true;
    }

    void Start()
    {
        // 시작 시 데이터 로드
    }

    public void DataSave(string keyName)
    {

        ES3.Save(keyName, saveDatas);
        Debug.Log("Save Data");

    }

    public void LoadData(string keyName)
    {
        Debug.Log("Load Data");
        if (ES3.FileExists(fileName))
        {
            Debug.Log("Data Loading");
            ES3.LoadInto(keyName, saveDatas);
        }
        else
        {
            Debug.Log("No File");
            DataSave(keyName);
        }
    }

    private void OnApplicationPause()
    {
        // 종료 시 데이터 저장
        ResourceManager.instance.SaveResourceData();
    }
}
