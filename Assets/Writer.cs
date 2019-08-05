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
    public static string gender;

    void Start()
    {
        logData = new LogData();


    }

    void Update()
    {
        if (logging)
            Log();

    }

    public void Log()
    {
        logData.timestamp = DateTime.Now;
        // Debug.Log(logData.timestamp);
        using (StreamWriter outputfile = new StreamWriter(fileName + "_p" + participantId + "_" + gender +".csv", true))
        {
            if (first)
            {
                outputfile.WriteLine(logData.getHeader());
                first = false;
            }
            if (Input.GetKeyDown("space"))
            {
                logData.space = "pressed";
                Debug.Log(logData.toStringShort());
              //  outputfile.WriteLine(logData.toString());
                logData.space = "";

            }
           //outputfile.WriteLine(logData.toString());
        }
       Debug.Log(logData.toStringShort());
    }
}
