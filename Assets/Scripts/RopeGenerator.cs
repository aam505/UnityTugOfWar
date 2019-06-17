using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class RopeGenerator : MonoBehaviour
{
    public int length;
    public GameObject RobePiece;
    GameObject Previous;
    List<GameObject> ropeJoints = new List<GameObject>();

    void Start()
    {
        GameObject ropePiece;
        for (int i = 0; i < length; i++)
        {
            //ropePiece = Instantiate(RobePiece, new Vector3(0, 0, i * 1.9f), Quaternion.identity);
            ropePiece = Instantiate(RobePiece, new Vector3(0, 0, i * 0.1f), Quaternion.identity);
            
            ropePiece.transform.SetParent(transform);
            
            ropeJoints.Add(ropePiece);
            if (Previous != null)
            {

                ropePiece.GetComponentInChildren<FixedJoint>().connectedBody = Previous.GetComponentInChildren<Rigidbody>();


            }
            else
            {
                Destroy(ropePiece.GetComponentInChildren<FixedJoint>());
                ropePiece.GetComponentInChildren<Rigidbody>().isKinematic = true;
            }
            Previous = ropePiece;
            if (i == length - 1)
            {
                ropePiece.transform.GetChild(0).GetComponentInChildren<Rigidbody>().isKinematic = true;
            }
        }

        Previous = ropeJoints[0];
        for (int i = 1; i < ropeJoints.Count; i++)
        {

            ConstraintSource constraintsource = new ConstraintSource();
            constraintsource.sourceTransform = Previous.gameObject.transform.GetChild(0).GetChild(0); //get bone transform
            constraintsource.weight = 1;

            //ropeJoints[i].gameObject.transform.GetChild(0).GetChild(1).gameObject.AddComponent<ParentConstraint>();
            ParentConstraint parentConstraint = ropeJoints[i].GetComponentInChildren<ParentConstraint>();


            parentConstraint.AddSource(constraintsource);
            parentConstraint.SetTranslationOffset(0, new Vector3(0, 0, -0.002f));
            Debug.Log(parentConstraint.translationOffsets[0]);
            Previous = ropeJoints[i];

        }

    }
    
}
