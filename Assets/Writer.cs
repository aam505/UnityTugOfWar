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
    private static string fileName = "ExperimentLog";
    private static object locker = new Object();
    public static bool logging = false;
    bool first = true;

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

        using (StreamWriter outputFile = new StreamWriter(fileName + "_p" + participantId + ".csv", true))
        {
            //todo
        }

        //Debug.Log(logData.toString());

    }
}
