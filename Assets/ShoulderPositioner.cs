using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShoulderPositioner : MonoBehaviour
{

    public Transform Head;
    public Vector3 Offset;
    private Transform shoulder;
    
    // Start is called before the first frame update
    void Start()
    {
        shoulder = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        shoulder.position = Head.position + Offset;
        shoulder.rotation = Head.rotation;
    }
}
