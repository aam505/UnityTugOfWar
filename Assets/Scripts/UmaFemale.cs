using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA.CharacterSystem;
using System;
using UMA.PoseTools;
using UMA;
using RootMotion.FinalIK;
using RootMotion;

public class UmaFemale : MonoBehaviour
{
    private DynamicCharacterAvatar avatar;
    private Dictionary<string, DnaSetter> dna;
    // Start is called before the first frame update
    private string umaName;
    bool setSize = false;

    public enum Condition
    {
        Weak,
        Average,
        Strong
    };

    public Condition condition;

    GameObject parent;
    private ExpressionPlayer expression;
    private bool connected = false;
    GameObject rope;
    private bool setUp = true;
    Transform[] umaBodyParts;
     AimIK aimIK;
    private Transform hand;

    void OnEnable()
    {
        avatar = GetComponent<DynamicCharacterAvatar>();
        avatar.CharacterCreated.AddListener(OnCreated);
    }

    void OnDisable()
    {
        avatar.CharacterCreated.RemoveListener(OnCreated);
    }

    public void OnCreated(UMAData data)
    {
        expression = GetComponent<ExpressionPlayer>();
        expression.enableBlinking = false;
        expression.enableSaccades = false;
        connected = true;
    }



    void Start()
    {
        avatar = GetComponent<DynamicCharacterAvatar>();

        umaName = transform.gameObject.name;
       

    }

    bool loadedText=false;
    // Update is called once per frame
    void Update()
    {

        if (!loadedText)
        {
            avatar.LoadFromTextFile(name);
            loadedText = true;
        }
        if (connected && setUp)
        {
            
            setUp = false;

           

            expression.mouthUp_Down = 0.4f;
            expression.browsIn = 1f;
            expression.leftBrowUp_Down = 0.4f;
            expression.rightBrowUp_Down = 0.4f;
            expression.midBrowUp_Down = 0f;
            expression.enableBlinking = true;
            expression.maxBlinkDelay = 2;
            //if (setSize == false)
            //{

            //parent = transform.parent.gameObject;
            // parent.transform.localScale = new Vector3(2, 2, 2);
            //parent.transform.localPosition = new Vector3(-0.28f, 3.905f, -3.033f);
            // setSize = true;
            //}
            parent = transform.parent.gameObject;
            parent.transform.GetChild(0).GetComponent<Animator>().applyRootMotion = true;
            umaBodyParts = parent.GetComponentsInChildren<Transform>();

            dna = avatar.GetDNA(); //takes couple of frames 


            aimIK = transform.gameObject.AddComponent<AimIK>();

      
            Transform[] heirarchy=new Transform[2];
            heirarchy[0] = null;
            heirarchy[1] = null;
            Transform root = null;

            foreach (Transform bodyPart in umaBodyParts)
            {
                if (bodyPart.gameObject.name.Equals("Head")){
                    heirarchy[1] = bodyPart;
                }
                if (bodyPart.gameObject.name.Equals("Neck"))
                {
                    heirarchy[0] = bodyPart;
                }
                if (bodyPart.gameObject.name.Equals("Root"))
                {
                    root = bodyPart;
                }
            }



            aimIK.solver.SetChain(heirarchy, root);

            aimIK.solver.target = GameObject.Find("TrackedHead").transform;
            aimIK.solver.transform = heirarchy[1];

         
            if (condition == Condition.Weak)
                weakCondition();
            else if (condition == Condition.Strong)
                strongCondition();
            else if (condition == Condition.Average)
                averageCondition();

            avatar.BuildCharacter();
        }

       


    }


    private void weakCondition()
    {
        if (dna == null)
        {
            dna = avatar.GetDNA();
        }

        if (name.Equals("UMA_F3") || name.Equals("UMA_F4"))
        {
            avatar.characterColors.SetColor("Footwear01", new Color(27f / 255f, 27f / 255f, 27f / 255f));
            avatar.characterColors.SetColor("ClothingBottom01", new Color(120f / 255f, 141f / 255f, 183f / 255f));
            avatar.characterColors.SetColor("ClothingTop01", Color.white);
        }
        else
        {
            avatar.characterColors.SetColor("Shirt", Color.white);
            avatar.characterColors.SetColor("Pants1", new Color(120f / 255f, 141f / 255f, 183f / 255f));
        }


        avatar.characterColors.SetColor("Eyes", new UnityEngine.Color(99f / 255f, 67f / 255f, 67f / 255f));

        dna["chinPosition"].Set(0.5f);
        dna["chinPronounced"].Set(0.3f);
        dna["chinSize"].Set(0.3f);
        dna["headWidth"].Set(0.5f);
        dna["jawsPosition"].Set(0.5f);
        dna["jawsSize"].Set(0.3f);
        dna["noseSize"].Set(0.3f);
        dna["noseWidth"].Set(0.3f);
        dna["lipsSize"].Set(0.6f);

        dna["armWidth"].Set(0.3f);
        dna["forearmWidth"].Set(0.3f);
        dna["lowerWeight"].Set(0.3f);
        dna["neckThickness"].Set(0.3f);
        dna["upperMuscle"].Set(0.3f);
        dna["upperWeight"].Set(0.3f);

        dna["eyeSize"].Set(0.1f);


        dna["cheekPosition"].Set(0.5f);
        dna["lowCheekPosition"].Set(0.5f);
        //dna["lowCheekPronounced"].Set(0.5f);
        dna["cheekSize"].Set(0.5f);
        dna["mouthSize"].Set(0.5f);
        dna["lowerMuscle"].Set(0.5f);

        dna["noseInclination"].Set(0.47f);
        dna["nosePosition"].Set(0.362f);
        dna["noseSize"].Set(1);
        dna["noseSize"].Set(0.594f);
        dna["noseWidth"].Set(0.4f);
        dna["foreheadSize"].Set(1);
        dna["breastSize"].Set(0.3f);
        dna["breastCleavage"].Set(0.7f);

        avatar.BuildCharacter();
    }


    private void averageCondition()
    {
        if (dna == null)
        {
            dna = avatar.GetDNA();
        }

        if (name.Equals("UMA_F3") || name.Equals("UMA_F4"))
        {
            avatar.characterColors.SetColor("Footwear01", new Color(27f / 255f, 27f / 255f, 27f / 255f));
            avatar.characterColors.SetColor("ClothingBottom01", new Color(120f / 255f, 141f / 255f, 183f / 255f));
            avatar.characterColors.SetColor("ClothingTop01", new Color(128f / 255f, 128f / 255f, 128f / 255f));
        }
        else
        {
            avatar.characterColors.SetColor("Shirt", Color.grey);
            avatar.characterColors.SetColor("Pants1", new Color(120f / 255f, 141f / 255f, 183f / 255f));
        }

        avatar.characterColors.SetColor("Eyes", new Color(99f / 255f, 67f / 255f, 67f / 255f));

        dna["chinPosition"].Set(0.5f);
        dna["chinPronounced"].Set(0.5f);
        dna["chinSize"].Set(0.5f);
        dna["headWidth"].Set(0.5f);
        dna["jawsPosition"].Set(0.5f);
        dna["jawsSize"].Set(0.5f);
        dna["noseSize"].Set(0.5f);
        dna["noseWidth"].Set(0.5f);
        dna["lipsSize"].Set(0.4f);

        dna["armWidth"].Set(0.5f);
        dna["forearmWidth"].Set(0.5f);
        dna["lowerWeight"].Set(0.5f);
        dna["neckThickness"].Set(0.5f);
        dna["upperMuscle"].Set(0.5f);
        dna["upperWeight"].Set(0.5f);

        dna["eyeSize"].Set(0.5f);


        dna["cheekPosition"].Set(0.5f);
        dna["lowCheekPosition"].Set(0.5f);
        //dna["lowCheekPronounced"].Set(0.5f);
        dna["cheekSize"].Set(0.5f);
        dna["mouthSize"].Set(0.5f);
        dna["lowerMuscle"].Set(0.5f);

        dna["noseInclination"].Set(0.47f);
        dna["nosePosition"].Set(0.362f);
        dna["noseSize"].Set(1);
        dna["noseSize"].Set(0.594f);
        dna["noseWidth"].Set(0.4f);
        dna["foreheadSize"].Set(1);
        dna["breastSize"].Set(0.3f);
        dna["breastCleavage"].Set(0.7f);



        avatar.BuildCharacter();

    }

    private void strongCondition()
    {
        if (dna == null)
        {
            dna = avatar.GetDNA();
        }



        avatar.characterColors.SetColor("Shirt", Color.black);


        if (name.Equals("UMA_F3") || name.Equals("UMA_F4"))
        {
            avatar.characterColors.SetColor("Footwear01", new Color(27f / 255f, 27f / 255f, 27f / 255f));
            avatar.characterColors.SetColor("ClothingBottom01", new Color(53f / 255f, 53f / 255f, 77f / 255f));
            avatar.characterColors.SetColor("ClothingTop01", new Color(29f / 255f, 28f / 255f, 36f / 255f));
        }
        else
        {
            avatar.characterColors.SetColor("Shirt", Color.black);
            avatar.characterColors.SetColor("Pants1", new Color(20f / 255f, 18f / 255f, 36f / 255f));
        }

        avatar.characterColors.SetColor("Eyes", new Color(99f / 255f, 67f / 255f, 67f / 255f));

        dna["chinPosition"].Set(1f);
        dna["chinPronounced"].Set(1f);
        dna["chinSize"].Set(1f);
        dna["headWidth"].Set(0.850f);
        dna["jawsPosition"].Set(0f);
        dna["jawsSize"].Set(1f);
        dna["noseSize"].Set(0.38f);
        dna["noseWidth"].Set(0.4f);
        dna["lipsSize"].Set(0);



        dna["armWidth"].Set(1f);
        dna["forearmWidth"].Set(1f);
        dna["lowerWeight"].Set(1f);
        dna["neckThickness"].Set(1f);
        dna["upperMuscle"].Set(1f);
        if (name.Equals("UMA_F3") || name.Equals("UMA_F4"))
            dna["upperWeight"].Set(0.53f);
        else
            dna["upperWeight"].Set(0.7f);

        dna["eyeSize"].Set(0.4f);

        dna["cheekPosition"].Set(0.5f);
        dna["lowCheekPosition"].Set(0.5f);
        //dna["lowCheekPronounced"].Set(0.5f);
        dna["cheekSize"].Set(0.5f);
        dna["mouthSize"].Set(0.5f);
        dna["lowerMuscle"].Set(0.3f);

        dna["noseInclination"].Set(0.47f);
        dna["nosePosition"].Set(0.362f);
        dna["noseSize"].Set(0.594f);
        dna["noseWidth"].Set(0.4f);
        dna["foreheadSize"].Set(1);
        dna["breastSize"].Set(0.3f);
        dna["breastCleavage"].Set(0.7f);

        avatar.BuildCharacter();
    }


    public void ParentLastPiece()
    {
        Debug.Log(transform.gameObject.name);
        foreach (Transform part in transform.GetComponentsInChildren<Transform>())
        {

            if (part.gameObject.name.Equals("LeftHand"))
            {
                Transform ropeHandleLow = GameObject.Find("ObiHandleLeft").transform;
                ropeHandleLow.parent = part;
                ropeHandleLow.localPosition = new Vector3(-0.0921f, -0.0346f, -0.0361f);                
                Debug.Log("Parenting to left hand");

            }
            if (part.gameObject.name.Equals("hand_L"))
            {
                Transform ropeHandleLow = GameObject.Find("ObiHandleLeft").transform;
                ropeHandleLow.parent = part;
                ropeHandleLow.localPosition = new Vector3(-0.0863f, 0.046f, 0.0242f);
                Debug.Log("Parenting to left hand");

            }
        }

    }

    public void ParentSecondToLastPiece()
    {


        foreach (Transform part in transform.GetComponentsInChildren<Transform>())
        {

            if (part.gameObject.name.Equals("RightHand") )
            {
                Transform ropeHandleLow = GameObject.Find("ObiHandleRight").transform;
                ropeHandleLow.parent = part;
                ropeHandleLow.localPosition = new Vector3(-0.0694f, 0.0318f, -0.0563f);    
                //ropeHandleLow.localRotation = new Quaternion(31.866f, 99f, -55.678f,1);    
                Debug.Log("Parenting to right hand");

            }

            if(part.gameObject.name.Equals("hand_R"))
            {
                Transform ropeHandleLow = GameObject.Find("ObiHandleRight").transform;
                ropeHandleLow.parent = part;
                ropeHandleLow.localPosition = new Vector3(-0.0716f, -0.0437f, 0.0208f);
                //ropeHandleLow.localRotation = new Quaternion(31.866f, 99f, -55.678f,1);    
                Debug.Log("Parenting to right hand");
            }
        }
    }
}
