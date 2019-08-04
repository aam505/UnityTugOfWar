using System.Collections.Generic;
using UnityEngine;
using UMA.CharacterSystem;
using UMA.PoseTools;
using UMA;
using RootMotion.FinalIK;
using static ExperimentController;
using System.Security.Cryptography;
using System;

public class UmaSettings : MonoBehaviour
{
    private DynamicCharacterAvatar avatar;
    private Dictionary<string, DnaSetter> dna;
    // Start is called before the first frame update
    private string umaName;
    bool loadedText = false;
    GameObject rope;
    Transform[] umaBodyParts;

    AimIK aimIKHead;
    // LimbIK aimIKLeftArm;
    //LimbIK aimIKRightArm;

    private Transform hand;
    GameObject parent;
    private bool setUp = true;
    private ExpressionPlayer expression;
    private bool connected = false;
    private UmaMoodSlider moodSetting;
    private ExperimentController controller;

    public class Avatar
    {
        public Condition condition;
        public string name;

        public Avatar(Condition c, string n)
        {
            condition = c;
            name = n;
        }

        public void setName(string n)
        {
            this.name = n;
        }
        public void setCondition(Condition c)
        {
            this.condition = c;
        }
    }

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
        connected = true;
    }

    private List<Avatar> avatars;

    private void Shuffle<T>(IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    void Start()
    {
        avatar = GetComponent<DynamicCharacterAvatar>();
        controller = GameObject.Find("ExperimentController").GetComponent<ExperimentController>();

        if (controller.originalAvatarsList == null)
        {
            controller.originalAvatarsList = new List<Avatar>();

            if (controller.gender == Gender.Female)
                setFemaleNames();
            else
                setMaleNames();

            Shuffle<Avatar>(controller.originalAvatarsList);

        }

        avatars = controller.originalAvatarsList;

        umaName = avatars[controller.currentAvatarIdx].name;
        Debug.Log("CURRENT IDX: " + controller.currentAvatarIdx + " - " + umaName + " " + avatars[controller.currentAvatarIdx].condition);

        avatar.LoadFromTextFile(umaName);
    }


    private void setFemaleNames()
    {

        controller.originalAvatarsList.Add(new Avatar(Condition.Weak, "UMA_F4"));
        controller.originalAvatarsList.Add(new Avatar(Condition.Average, "UMA_F2"));
        controller.originalAvatarsList.Add(new Avatar(Condition.Average, "UMA_F3"));
        controller.originalAvatarsList.Add(new Avatar(Condition.Strong, "UMA_F1"));
        controller.originalAvatarsList.Add(new Avatar(Condition.Strong, "UMA_F4"));
    }


    private void setMaleNames()
    {
        controller.originalAvatarsList.Add(new Avatar(Condition.Weak, "UMA_M7"));
        controller.originalAvatarsList.Add(new Avatar(Condition.Average, "UMA_M7"));
        controller.originalAvatarsList.Add(new Avatar(Condition.Strong, "UMA_M6"));
        controller.originalAvatarsList.Add(new Avatar(Condition.Average, "UMA_M1"));
        controller.originalAvatarsList.Add(new Avatar(Condition.Strong, "UMA_M5"));

    }

    void Update()
    {


        if (connected && setUp)
        {
            setUp = false;

            parent = transform.parent.gameObject;
            transform.GetComponent<Animator>().applyRootMotion = true;
            umaBodyParts = parent.GetComponentsInChildren<Transform>();

            dna = avatar.GetDNA(); //takes couple of frames 

            loadConditionCharacteristics();

            aimIKHead = transform.gameObject.AddComponent<AimIK>();
            //aimIKLeftArm = transform.gameObject.AddComponent<LimbIK>();
            //aimIKRightArm = transform.gameObject.AddComponent<LimbIK>();

            moodSetting = gameObject.GetComponent<UmaMoodSlider>();

            Transform[] heirarchy = new Transform[2];
            heirarchy[0] = null;
            heirarchy[1] = null;
            Transform root = null;

            Transform[] heirarchyLeftArm = new Transform[4];
            Transform[] heirarchyRightArm = new Transform[4];

            for (int i = 0; i < 3; i++)
            {
                heirarchyLeftArm[i] = null;
                heirarchyRightArm[i] = null;
            }

            foreach (Transform bodyPart in umaBodyParts)
            {

                if (bodyPart.gameObject.name.Equals("Head"))
                {
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
                if (bodyPart.gameObject.name.Equals("LeftShoulder") ||
                    bodyPart.gameObject.name.Equals("Clavicle_L"))
                {
                    heirarchyLeftArm[0] = bodyPart;
                }
                if (bodyPart.gameObject.name.Equals("RightShoulder") ||
                    bodyPart.gameObject.name.Equals("Clavicle_R"))
                {
                    heirarchyRightArm[0] = bodyPart;
                }
                if (bodyPart.gameObject.name.Equals("LeftArm") ||
                    bodyPart.gameObject.name.Equals("Upperarm_L"))
                {
                    heirarchyLeftArm[1] = bodyPart;
                }
                if (bodyPart.gameObject.name.Equals("RightArm") ||
                    bodyPart.gameObject.name.Equals("Upperarm_R"))
                {
                    heirarchyRightArm[1] = bodyPart;
                }
                if (bodyPart.gameObject.name.Equals("LeftForeArm") ||
                    bodyPart.gameObject.name.Equals("Lowerarm_L"))
                {
                    heirarchyLeftArm[2] = bodyPart;
                }
                if (bodyPart.gameObject.name.Equals("RightForeArm") ||
                    bodyPart.gameObject.name.Equals("Lowerarm_R"))
                {
                    heirarchyRightArm[2] = bodyPart;
                }
                if (bodyPart.gameObject.name.Equals("LeftHand") ||
                    bodyPart.gameObject.name.Equals("hand_L"))
                {
                    heirarchyLeftArm[3] = bodyPart;
                }
                if (bodyPart.gameObject.name.Equals("RightHand") ||
                    bodyPart.gameObject.name.Equals("hand_R"))
                {
                    heirarchyRightArm[3] = bodyPart;
                }
            }

            aimIKHead.solver.SetChain(heirarchy, root);

            // aimIKLeftArm.solver.SetChain(heirarchyLeftArm[1], heirarchyLeftArm[2], heirarchyLeftArm[3], root);
            // aimIKRightArm.solver.SetChain(heirarchyRightArm[1], heirarchyRightArm[2], heirarchyRightArm[3], root);

            setIKTargetHead();
            //aimIKLeftArm.solver.target = GameObject.Find("Target").transform;
            //aimIKRightArm.solver.target = GameObject.Find("Target").transform;

            aimIKHead.solver.transform = heirarchy[1];
            // aimIKLeftArm.solver.bendModifier = IKSolverLimb.BendModifier.Animation;
            // aimIKLeftArm.solver.SetIKRotationWeight(0);
            //aimIKRightArm.solver.bendModifier = IKSolverLimb.BendModifier.Animation;
            //aimIKRightArm.solver.SetIKRotationWeight(0);

            //loadConditionCharacteristics();
            ParentHandles();
        }

    }


    private void loadConditionCharacteristics()
    {
        if (controller.gender == ExperimentController.Gender.Female)
        {
            switch (avatars[controller.currentAvatarIdx].condition)
            {
                case ExperimentController.Condition.Average:
                    //averageConditionFemale();
                    averageConditionFemale();
                    break;
                case ExperimentController.Condition.Weak:
                    weakConditionFemale();
                    break;
                case ExperimentController.Condition.Strong:
                    strongConditionFemale();
                    break;
                default:
                    Debug.LogError("Default switch case");
                    break;
            }
        }
        else
        {

            switch (avatars[controller.currentAvatarIdx].condition)
            {
                case ExperimentController.Condition.Average:
                    averageConditionMale();
                    break;
                case ExperimentController.Condition.Weak:
                    weakConditionMale();
                    break;
                case ExperimentController.Condition.Strong:
                    strongConditionMale();
                    break;
                default:
                    Debug.LogError("Default switch case");
                    break;
            }
        }

        avatar.BuildCharacter();
    }

    public void setMood(int m)
    {
        moodSetting.mood = m;

    }

    private void strongConditionMale()
    {
        if (dna == null)
        {
            dna = avatar.GetDNA();
        }

        avatar.characterColors.SetColor("Shirt", Color.black);


        if (name.Equals("UMA_M6") || name.Equals("UMA_M7"))
        {
            avatar.characterColors.SetColor("ClothingTop01", Color.black);

            avatar.characterColors.SetColor("ClothingBottom01", new Color(20f / 255f, 18f / 255f, 36f / 255f));
        }
        else
        {
            avatar.characterColors.SetColor("Shirt", Color.black);
            avatar.characterColors.SetColor("Pants1", new Color(20f / 255f, 18f / 255f, 36f / 255f));
        }

        avatar.characterColors.SetColor("Eyes", new Color(99f / 255f, 67f / 255f, 67f / 255f));


        dna["height"].Set(0.6f);

        dna["chinPosition"].Set(1f);
        dna["chinPronounced"].Set(1f);
        dna["chinSize"].Set(1f);
        dna["headWidth"].Set(0.850f);
        dna["jawsPosition"].Set(0f);
        dna["jawsSize"].Set(0.65f);
        dna["noseSize"].Set(0.38f);
        dna["noseWidth"].Set(0.4f);
        dna["lipsSize"].Set(0.250f);

        dna["armWidth"].Set(1f);
        dna["forearmWidth"].Set(1f);
        dna["lowerWeight"].Set(1f);
        dna["neckThickness"].Set(1f);
        dna["upperMuscle"].Set(0.7f);
        dna["upperWeight"].Set(0.7f);

        dna["eyeSize"].Set(0.5f);

        dna["cheekPosition"].Set(0.5f);
        dna["lowCheekPosition"].Set(0.5f);
        //dna["lowCheekPronounced"].Set(0.5f);
        dna["cheekSize"].Set(0.5f);
        dna["mouthSize"].Set(0.5f);
        dna["lowerMuscle"].Set(0.5f);


        avatar.BuildCharacter();
    }

    private void averageConditionMale()
    {
        if (dna == null)
        {
            dna = avatar.GetDNA();
        }

        if (name.Equals("UMA_M6") || name.Equals("UMA_M7"))
        {
            avatar.characterColors.SetColor("ClothingTop01", Color.gray);

            avatar.characterColors.SetColor("ClothingBottom01", new Color(120f / 255f, 141f / 255f, 183f / 255f));
        }
        else
        {
            avatar.characterColors.SetColor("Pants1", new Color(120f / 255f, 141f / 255f, 183f / 255f));
            avatar.characterColors.SetColor("Shirt", Color.gray);
        }

        avatar.characterColors.SetColor("Eyes", new Color(99f / 255f, 67f / 255f, 67f / 255f));

        dna["height"].Set(0.55f);


        dna["chinPosition"].Set(0.5f);
        dna["chinPronounced"].Set(0.5f);
        dna["chinSize"].Set(0.5f);
        dna["headWidth"].Set(0.5f);
        dna["jawsPosition"].Set(0.5f);
        dna["jawsSize"].Set(0.5f);
        dna["noseSize"].Set(0.5f);
        dna["noseWidth"].Set(0.5f);
        dna["lipsSize"].Set(0.5f);

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

        avatar.BuildCharacter();
    }

    private void weakConditionMale()
    {
        if (dna == null)
        {
            dna = avatar.GetDNA();
        }

        if (umaName.Equals("UMA_M6") || umaName.Equals("UMA_M7"))
        {
            avatar.characterColors.SetColor("ClothingTop01", Color.white);

            avatar.characterColors.SetColor("ClothingBottom01", new Color(120f / 255f, 141f / 255f, 183f / 255f));
        }
        else
        {

            avatar.characterColors.SetColor("Pants1", new Color(120f / 255f, 141f / 255f, 183f / 255f));
            avatar.characterColors.SetColor("Shirt", Color.white);
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
        dna["lipsSize"].Set(0.7f);

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


        avatar.BuildCharacter();
    }

    private void weakConditionFemale()
    {
        if (dna == null)
        {
            dna = avatar.GetDNA();
        }

        if (umaName.Equals("UMA_F3") || umaName.Equals("UMA_F4"))
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

    private void averageConditionFemale()
    {
        if (dna == null)
        {
            dna = avatar.GetDNA();
        }

        if (umaName.Equals("UMA_F3") || umaName.Equals("UMA_F4"))
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


        dna["height"].Set(0.55f);
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

    private void strongConditionFemale()
    {

        if (dna == null)
        {
            dna = avatar.GetDNA();
        }

        avatar.characterColors.SetColor("Shirt", Color.black);


        if (umaName.Equals("UMA_F3") || umaName.Equals("UMA_F4"))
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

        dna["height"].Set(0.6f);

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



    /// <summary>
    /// F4 WEAK O3N 
    /// ObiHandleAvatarPinkyL  -0.025 0.002 0.014
    /// ObiHandleAvatarThumbL -0.017 -0.029 0.005
    /// ObiHandleAvatarThumbR -0.0211 0.0407 0.017
    /// 
    /// F2 UM
    /// ObiHandleAvatarThumbR -0.014f, -0.028f, -0.022f
    /// ObiHandleAvatarThumbL -0.0042f, 0.0142f, -0.0246f
    /// ObiHandleAvatarPinkyL -0.0256f, 0.0072f, -0.0107f
    /// 
    /// M7 O3N
    /// ObiHandleAvatarThumbR -0.022f, 0.023f, -0.015f
    /// ObiHandleAvatarThumbL  -0.013f, -0.024f, 0.016f
    /// ObiHandleAvatarPinkyL -0.0155f, 0.0035f, 0.0125f
    /// 
    /// M1 UMA
    /// ObiHandleAvatarThumbR  -0.02F, -0.029f,-0.015f
    /// ObiHandleAvatarThumbL -0.004f, 0.022f, -0.013f
    /// ObiHandleAvatarPinkyL -0.005f,  0.008f,  -0.029f
    /// 
    /// M5 UMA STRONG
    /// ObiHandleAvatarThumbR  -0.016f, -0.04f, 0.006f
    /// ObiHandleAvatarThumbL  -0.013f, 0.024f, 0.008f
    /// ObiHandleAvatarPinkyL -0.007f,  0.007f,  -0.028f
    /// </summary>
    public void ParentHandles()
    {

        foreach (Transform part in transform.GetComponentsInChildren<Transform>())
        {

            Transform pinkyHandleL = GameObject.Find("ObiHandleAvatarPinkyL").transform;
            Transform thumbHandleL = GameObject.Find("ObiHandleAvatarThumbL").transform;
            Transform thumbHandleR = GameObject.Find("ObiHandleAvatarThumbR").transform;


            ////////////// UMA
            if (part.gameObject.name.Equals("RightHandFinger05_02")) //THUMB RIGHT
            {
                thumbHandleR.parent = part;
                if (umaName.Equals("UMA_F2") || umaName.Equals("UMA_F1"))
                    thumbHandleR.localPosition = new Vector3(-0.014f, -0.028f, -0.022f);
                else
                {
                    if (umaName.Equals("UMA_M5"))
                        thumbHandleR.localPosition = new Vector3(-0.016f, -0.04f, 0.006f);
                    else

                        thumbHandleR.localPosition = new Vector3(-0.02f, -0.029f, -0.015f);
                }
            }

            if (part.gameObject.name.Equals("LeftHandFinger05_02")) //THUMB LEFT
            {
                thumbHandleL.parent = part;

                if (umaName.Equals("UMA_F2") || umaName.Equals("UMA_F1"))
                    thumbHandleL.localPosition = new Vector3(-0.0042f, 0.0142f, -0.0246f);
                else
                {
                    if (umaName.Equals("UMA_M5"))

                        thumbHandleL.localPosition = new Vector3(-0.013f, 0.024f, 0.008f);
                    else
                        thumbHandleL.localPosition = new Vector3(-0.004f, 0.022f, -0.013f);
                }
            }

            if (part.gameObject.name.Equals("LeftHandFinger01_02"))  //pinky left
            {
                pinkyHandleL.parent = part;

                if (umaName.Equals("UMA_F2") || umaName.Equals("UMA_F1"))
                    pinkyHandleL.localPosition = new Vector3(-0.0256f, 0.0072f, -0.0107f);
                else
                {
                    if (umaName.Equals("UMA_M5"))
                        pinkyHandleL.localPosition = new Vector3(-0.007f, 0.007f, -0.028f);
                    else
                        pinkyHandleL.localPosition = new Vector3(-0.005f, 0.008f, -0.029f);
                }
            }



            ////////////// O3N
            if (part.gameObject.name.Equals("thumb_002_R"))
            {
                thumbHandleR.parent = part;

                if (umaName.Equals("UMA_F4") || umaName.Equals("UMA_F3"))
                    thumbHandleR.localPosition = new Vector3(-0.0211f, 0.0407f, 0.017f);
                else

                    thumbHandleR.localPosition = new Vector3(-0.022f, 0.023f, -0.015f);

            }

            if (part.gameObject.name.Equals("thumb_002_L"))
            {

                thumbHandleL.parent = part;
                if (controller.gender == Gender.Female)
                    thumbHandleL.localPosition = new Vector3(-0.017f, -0.029f, 0.005f);
                else
                    thumbHandleL.localPosition = new Vector3(-0.013f, -0.024f, 0.016f);
            }

            if (part.gameObject.name.Equals("pinky_001_L"))
            {

                pinkyHandleL.parent = part;
                if (controller.gender == Gender.Female)
                    pinkyHandleL.localPosition = new Vector3(-0.025f, 0.002f, 0.014f);
                else
                    pinkyHandleL.localPosition = new Vector3(-0.0155f, 0.0035f, 0.0125f);
            }

        }

    }


    public void setIKTargetHead()
    {
        aimIKHead.solver.target = GameObject.Find("TrackedHead").transform;
    }

    public void setIKTargetHand()
    {
        aimIKHead.solver.target = GameObject.Find("ObiHandleRight").transform;
    }
}