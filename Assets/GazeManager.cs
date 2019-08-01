using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeManager : MonoBehaviour
{
    public float sightlength = 100.0f;
    public GameObject selectedObj;
    void FixedUpdate()
    {
        RaycastHit seen;
        Ray raydirection = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(raydirection, out seen, sightlength))
        {
            //Debug.Log(seen.transform.name);
                if(seen.transform!=null)
                    Writer.logData.gazeTarget = seen.transform.name;
        }
    }
}