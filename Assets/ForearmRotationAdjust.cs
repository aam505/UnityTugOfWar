using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForearmRotationAdjust : MonoBehaviour
{

    public float twistOffset;
    public Vector3 twistAxis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
        transform.localRotation = Quaternion.AngleAxis(twistOffset, twistAxis) * transform.localRotation;
    }

}
