using System.Collections.Generic;
using UnityEngine;
using UMA.CharacterSystem;
using UMA.PoseTools;
using UMA;
using RootMotion.FinalIK;
using static ExperimentController;

public class UmaSettings : MonoBehaviour
{

    class Avatar
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

    private void setFemaleNames()
    {
        umaAverageFemales.Add("UMA_F1");
        umaAverageFemales.Add("UMA_F1");
        umaAverageFemales.Add("UMA_F3");
        umaNameFemaleStrong = "UMA_F4";
        umaNameFemaleWeak = "UMA_F4";
    }


    private void setMaleNames()
    {
        umaAverageMales.Add("UMA_M6");
        umaAverageMales.Add("UMA_M7");
        umaAverageMales.Add("UMA_M6");
        umaNameMaleStrong = "UMA_M5";
        umaNameMaleWeak = "UMA_M7";
    }

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
    private List<string> umaAverageFemales = new List<string>();
    private List<string> umaAverageMales = new List<string>();
    private string umaNameFemaleStrong;
    private string umaNameFemaleWeak;
    private string umaNameMaleStrong;
    private string umaNameMaleWeak;
    private List<Avatar> avatars;


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
    void Start()
    {
        avatar = GetComponent<DynamicCharacterAvatar>();
        controller = GameObject.Find("ExperimentController").GetComponent<ExperimentController>();

        setFemaleNames();
        setMaleNames();
        avatars = new List<Avatar>();

        int j = 0;
        for (int i = 1; i <= controller.totalConditions; i++)
        {

            if (i % 2 == 1)
            {
                if (controller.gender == ExperimentController.Gender.Female)
                    avatars.Add(new Avatar(Condition.Average, umaAverageFemales[j]));
                else
                    avatars.Add(new Avatar(Condition.Average, umaAverageMales[j]));
                j++;
            }
            else
            {
                avatars.Add(new Avatar(Condition.Average, ""));
            }

        }

        if (controller.condition == Condition.Strong)
        {
            avatars[1].setCondition(Condition.Strong);
            avatars[3].setCondition(Condition.Weak);

            if (controller.gender == Gender.Female)
            {
                avatars[1].setName(umaNameFemaleStrong);
                avatars[3].setName(umaNameFemaleWeak);
            }
            else
            {
                avatars[1].setName(umaNameMaleStrong);
                avatars[3].setName(umaNameMaleWeak);
            }


        }
        else
        {
            avatars[1].setCondition(Condition.Weak);
            avatars[3].setCondition(Condition.Strong);

            if (controller.gender == Gender.Female)
            {
                avatars[1].setName(umaNameFemaleWeak);
                avatars[3].setName(umaNameFemaleStrong);
            }
            else
            {
                avatars[1].setName(umaNameMaleWeak);
                avatars[3].setName(umaNameMaleStrong);
            }


        }

        umaName = avatars[controller.currentAvatarIdx].name;
        Debug.Log("Current idx " + controller.currentAvatarIdx);
        Debug.Log("Current Uma " + umaName + " c:" + avatars[controller.currentAvatarIdx].condition);

        avatar.LoadFromTextFile(umaName);


    }

    // Update is called once per frame
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
            Debug.Log("Loading dna for female.." + avatars[controller.currentAvatarIdx].condition);
            switch (avatars[controller.currentAvatarIdx].condition)
            {
                case ExperimentController.Condition.Average:
                    Debug.Log("In average switch case");
                    //averageConditionFemale();
                    averageConditionFemale();
                    break;
                case ExperimentController.Condition.Weak:
                    Debug.Log("In weak switch case");
                    weakConditionFemale();
                    break;
                case ExperimentController.Condition.Strong:
                    Debug.Log("In strong switch case");
                    strongConditionFemale();
                    break;
                default:
                    Debug.LogError("Default switch case");
                    break;
            }
        }
        else
        {
            Debug.Log("Loading dna for male.." + avatars[controller.currentAvatarIdx].condition);
            switch (avatars[controller.currentAvatarIdx].condition)
            {
                case ExperimentController.Condition.Average:
                    strongConditionMaleGreyShirt();
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

    private void strongConditionMaleGreyShirt()
    {
        if (dna == null)
        {
            dna = avatar.GetDNA();
        }

        if (umaName.Equals("UMA_M6") || umaName.Equals("UMA_M7"))
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
        Debug.Log("In weak female condition");

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
        Debug.Log("In average female condition");
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
        Debug.Log("In strong female condition");

        Debug.Log(dna);
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


    public void ParentHandles()
    {

        foreach (Transform part in transform.GetComponentsInChildren<Transform>())
        {

            Transform pinkyHandleL = GameObject.Find("ObiHandleAvatarPinkyL").transform;
            Transform thumbHandleL = GameObject.Find("ObiHandleAvatarThumbL").transform;

            Transform thumbHandleR = GameObject.Find("ObiHandleAvatarThumbR").transform;
            Transform pinkyHandleR = GameObject.Find("ObiHandleAvatarPinkyR").transform;


            ////////////// UMA
            if (part.gameObject.name.Equals("RightHandFinger05_02")) //THUMB RIGHT
            {

                thumbHandleR.parent = part;

                thumbHandleR.localPosition = new Vector3(0, 0, 0);
                Debug.Log("Parented right hand finger");
            }

            if (part.gameObject.name.Equals("RightHandFinger01_02"))  //PINKY RIGHT
            {

                pinkyHandleR.parent = part;

                pinkyHandleR.localPosition = new Vector3(0, 0f, 0);
            }


            if (part.gameObject.name.Equals("LeftHandFinger05_02")) //THUMB LEFT
            {

                thumbHandleL.parent = part;

                thumbHandleL.localPosition = new Vector3(0f, 0f, 0f);
            }

            if (part.gameObject.name.Equals("LeftHandFinger01_02"))  //pinky left
            {

                pinkyHandleL.parent = part;

                pinkyHandleL.localPosition = new Vector3(0f, 0f, 0f);
            }



            ////////////// O3N
            if (part.gameObject.name.Equals("thumb_002_R"))
            {

                thumbHandleR.parent = part;

                thumbHandleR.localPosition = new Vector3(0f, 0f, 0f);
            }

            if (part.gameObject.name.Equals("pinky_001_R"))
            {

                pinkyHandleR.parent = part;

                pinkyHandleR.localPosition = new Vector3(0f, 0f, 0f);
            }


            if (part.gameObject.name.Equals("thumb_002_L"))
            {

                thumbHandleL.parent = part;

                thumbHandleL.localPosition = new Vector3(0f, 0f, 0f);
            }

            if (part.gameObject.name.Equals("pinky_001_L"))
            {

                pinkyHandleL.parent = part;

                pinkyHandleL.localPosition = new Vector3(0f,0f, 0f);
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