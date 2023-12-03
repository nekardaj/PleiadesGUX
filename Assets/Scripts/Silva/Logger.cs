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
        string startMessage = "[" + System.DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss,f") + "]: GAME STARTS\n";

        try
        {
            System.IO.File.AppendAllText(filename, startMessage);
        }
        catch { }

    }

    void OnDisable() 
    {
        Application.logMessageReceived -= Log;
        string endMessage = "[" + System.DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss,f") + "]: GAME ENDS\n";

        try
        {
            System.IO.File.AppendAllText(filename, endMessage);
        }
        catch { }
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        if (!logString.StartsWith("[log] ")) return;
        else logString = logString.Substring("[log] ".Length);

        try
        {
            string finalMessage = "[" + System.DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss,f") + "]: " + logString + "\n";
            System.IO.File.AppendAllText(filename, finalMessage);
        }
        catch { }
    }
}

