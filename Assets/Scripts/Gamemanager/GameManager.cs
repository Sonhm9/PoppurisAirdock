using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static object _lock = new object();
    private object itemLock = new object();

    private static GameManager _instance = null;
    public static GameManager instance
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
                    GameObject obj = new GameObject("GameManager ");
                    obj.AddComponent<GameManager>();
                    _instance = obj.GetComponent<GameManager>();
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
    // ½Ì±ÛÅæ
    private void Awake()
    {
        _instance = this;
    }
    private void OnDestroy()
    {
        applicationQuitting = true;
    }

    [HideInInspector] public int playerLevel = 0; // ÇÃ·¹ÀÌ¾î ·¹º§
}
