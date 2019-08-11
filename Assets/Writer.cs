using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = System.Object;

public class Writer : MonoBehaviour
{

    public static LogData logData;
    public static int participantId = -1;
    private static string fileName = "ExLog";
    private static object locker = new Object();
    public static bool logging = true;
    bool first = true;
    string initTimestamp;
    public static string gender;
    private float sampleRate = 60f;

    void Start()
    {
        logData = new LogData();
        initTimestamp = DateTime.Now.ToString("s").Replace(':','-');
       // StartCoroutine(LoggingCoroutine());
     }

    void Update()
    {
        // if (logging) Log();

    }


    public void Log()
    {
        logData.timestamp = DateTime.Now;
       // Debug.Log(logData.timestamp);
   
        using (StreamWriter outputfile = new StreamWriter(fileName + "_p" + participantId + "_" + gender + "_"+initTimestamp+".csv", true))
        {
            if (first)
            {
                outputfile.WriteLine(logData.getHeader());
                first = false;
            }
            if (Input.GetKeyDown("space"))
            {
                logData.space = "pressed";
                //Debug.Log(logData.toStringShort());
                outputfile.WriteLine(logData.toString());
                logData.space = "";

            }
            outputfile.WriteLine(logData.toString());
        }
        //Debug.Log(logData.toStringShort());
    }

    IEnumerator LoggingCoroutine()
    {
        while (true)
        {
            while (logging)
            {
                Log();
                yield return new WaitForSeconds(1f / sampleRate);
            }
            yield return new WaitUntil(() => logging);
        }
    }
}
