using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Logger : MonoBehaviour
{
    private static Logger loggerInstance;

    string filename = Application.dataPath + "/SENDME_I-AM-WHAT-YOU-ARE-LOOKING-FOR.log";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (loggerInstance == null)
        {
            loggerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable() 
    {
        Application.logMessageReceived += Log; 
    }

    void OnDisable() 
    {
        Application.logMessageReceived -= Log; 
    }

    public void Log(string logString, string stackTrace, LogType type)
    {        
        try
        {
            System.IO.File.AppendAllText(filename, logString + "\n");
        }
        catch { }
    }
}

