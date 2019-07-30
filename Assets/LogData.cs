using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogData
{



    public DateTime timestamp;

    public string conditon = "";

    public float headx;
    public float heady;
    public float headz;
    public float headrx;
    public float headry;
    public float headrz;

    public string action = ""; //start pulling,pulling, stop pulling

    public float leftHandx;
    public float leftHandrx;
    public float leftHandy;
    public float leftHandry;
    public float leftHandz;
    public float leftHandrz;

    public float rightHandx;
    public float rightHandrx;
    public float rightHandy;
    public float rightHandry;
    public float rightHandz;
    public float rightHandrz;

    public string gazeTarget = "";
     string s = ",";

    //todo log looking at avatar?

    public LogData()
    {

    }

    public void setLeftHand(float x, float y, float z, float rx, float ry, float rz)
    {
        leftHandx = x;
        leftHandrx = rx;
        leftHandy = y;
        leftHandry = ry;
        leftHandz = z;
        leftHandrz = rz;
    }

    public void setRighttHand(float x, float y, float z, float rx, float ry, float rz)
    {
        rightHandx = x;
        rightHandrx = rx;
        rightHandy = y;
        rightHandry = ry;
        rightHandz = z;
        rightHandrz = rz;
    }

    public void setHead(float x, float y, float z, float rx, float ry, float rz)
    {
        headx = x;
        headrx = rx;
        heady = y;
        headry = ry;
        headz = z;
        headrz = rz;
    }
    public string getHeader()
    {
      
        return "Timestamp,HeadX,HeadY,HeadZ, HeadRX, HeadRY, HeadRZ, RightHandx,RightHandy,RightHandz,RightHandRX,RightHandRY,RightHandRZ" +
            "LeftHandx,LeftHandy,LeftHandz,LeftHandRX,LeftHandRY,LeftHandRZ,Action" +
           "Points,Action,GazeTarget";
    }
    public string toString()
    {

        return timestamp.ToString("o").Remove(26, 6) + s + headx + s + heady + s + headz + s + headrx + s + headry + s + headrz + s + rightHandx + s + rightHandy + s + rightHandz + s +
            rightHandrx + s + rightHandry + s + rightHandrz + s + leftHandx + s + leftHandy + s + leftHandz + s + leftHandrx + s + leftHandry + s + leftHandrz + s + action+s+gazeTarget;
    }

}