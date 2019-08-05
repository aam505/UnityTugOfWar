using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAxes : MonoBehaviour
{

    public string xRotation="x";
    public string yRotation="y";
    public string zRotation="z";

    float xxRotation;
    float yyRotation;
    float zzRotation;


    public string xPosition="x";
    public string yPosition="y";
    public string zPosition="z";

    float xxPosition;
    float yyPosition;
    float zzPosition;

    public Transform hand;

    // Start is called before the first frame update
    void Start()
    {
          xRotation = "x";
          yRotation = "y";
        zRotation = "z";

         xPosition = "x";
        yPosition = "y";
        zPosition = "z";
}

float swapRotation(string axisName)
    {
        if (axisName.Equals("x"))
        {
            return hand.rotation.x;
        }
        else if (axisName.Equals("-x"))
        {

            return -hand.rotation.x;
        }
        else if (axisName.Equals("y"))
        {
            return hand.rotation.y;
        }
        else if (axisName.Equals("-y"))
        {

            return -hand.rotation.z;
        }
        else if (axisName.Equals("z"))
        {

            return hand.rotation.z;
        }
        else if (axisName.Equals("-z"))
        {

            return -hand.rotation.z;
        }
        else
        {
            return 0;
            Debug.Log("invalid axis. Axis are x,y,z.");
        }
    }


float swapPosition(string axisName)
    {
        if (axisName.Equals("x"))
        {
            Debug.Log("Swapping x_");
            return (hand.position.x);
        }
        else if (axisName.Equals("-x"))
        {
            return -(hand.position.x);
        }
        else if (axisName.Equals("y"))
        {
            return (hand.position.y);
        }
        else if (axisName.Equals("-y"))
        {
            return -(hand.position.y);
        }
        else if (axisName.Equals("z")) { 

            return hand.position.z;
        }else if (axisName.Equals("-z"))
        {

            return -hand.position.z;
        }
        else
        {
            return 0;
            Debug.Log("invalid axis. Axis are x,y,z.");
        }
    }

    // Update is called once per frame
    void Update()
    {

        xxRotation = swapRotation(xRotation);
        yyRotation= swapRotation(yRotation);
        zzRotation = swapRotation(zRotation);

        xxPosition= swapPosition(xPosition);
        yyPosition = swapPosition(yPosition);
        zzPosition = swapPosition(zPosition);

        
        this.transform.rotation = new Quaternion(xxRotation,yyRotation,zzRotation,transform.rotation.w);
        this.transform.position = new Vector3(xxPosition, yyPosition, zzPosition);
    }
}
