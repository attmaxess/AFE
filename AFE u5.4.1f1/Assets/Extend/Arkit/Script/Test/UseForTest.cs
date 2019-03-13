using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseForTest : MonoBehaviour
{
    private static UseForTest instance;
    public static UseForTest Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<UseForTest>();
            }
            return instance;
        }
    }

    [System.NonSerialized]
    public bool IsTestOragon1 = true;
    public string NameDevice = "MARKSTAM";
    public float TimeEditor = 1;

    public bool IsTest = false;

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log(SystemInfo.deviceName);
        if (SystemInfo.deviceName != NameDevice)
        {
            IsTest = false;
        }
    }
}
