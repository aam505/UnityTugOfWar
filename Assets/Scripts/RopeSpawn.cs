using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class RopeSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject partPrefab, parentObject;

    [SerializeField]
    [Range(1, 1000)]
    int length = 1;

    [SerializeField]
    float partDistance = 0.21f;

    [SerializeField]
    bool reset, spawn, snapFirst, snapLast;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(reset)
        {
            foreach (GameObject tmp in GameObject.FindGameObjectsWithTag("RopeObject"))
            {
                Destroy(tmp);
            }
            reset = false;
        }
        if (spawn)
        {
            Spawn();
            spawn = false;
        }

        
    }

    public void Spawn()
    {

        List<GameObject> ropeJoints = new List<GameObject>();
        GameObject Previous;

        int count = (int)(length / partDistance);


        for (int x = 0; x < count; x++)
        {
            GameObject temp;
            temp = Instantiate(partPrefab, new Vector3(transform.position.x, transform.position.y + partDistance * (x + 1), transform.position.z), Quaternion.identity, parentObject.transform);
            temp.transform.eulerAngles = new Vector3(180, 0, 0);
            temp.name = parentObject.transform.childCount.ToString();

            ropeJoints.Add(temp);

            if (x == 0)
            {
                Destroy(temp.GetComponent<CharacterJoint>());
                if (snapFirst)
                {
                    temp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }

            }
            else
            {
                temp.GetComponent<CharacterJoint>().connectedBody = parentObject.transform.Find((parentObject.transform.childCount - 1).ToString()).GetComponent<Rigidbody>();
            }
        }
            if (snapLast)
            {
                parentObject.transform.Find((parentObject.transform.childCount).ToString()).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }

        GameObject First = ropeJoints[0];
        GameObject Last = ropeJoints[ropeJoints.Count-1];


        First.GetComponent<Rigidbody>().isKinematic = true;
        //ropeJoints[18].transform.localPosition = new Vector3(0, 0, 0);

       // Last.GetComponent<Rigidbody>().isKinematic = true;
        //ropeJoints[23].transform.localPosition = new Vector3(0, 0, 0);

        // Last.transform.localPosition = new Vector3(-0.026f, -1.565f, 1.563f);

        First.transform.eulerAngles = new Vector3 (20.375f, 41.733f, -89.98901f);
        //Last.transform.eulerAngles = new Vector3 (-90f, 0, 0);
        First.transform.localPosition = new Vector3(-0.566f, 0.195f, -2.889f);
        
    }
}
