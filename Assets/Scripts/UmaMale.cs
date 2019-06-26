using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA.CharacterSystem;
using System;
using UMA.PoseTools;
using UMA;

public class UmaMale : MonoBehaviour
{
    private DynamicCharacterAvatar avatar;
    private Dictionary<string, DnaSetter> dna;
    // Start is called before the first frame update
    private string umaName;


    private ExpressionPlayer expression;
    private bool connected = false;

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

        umaName= transform.gameObject.name;

    }

    // Update is called once per frame
    void Update()
    {

        if (connected)
        {
            expression.mouthUp_Down = -0.35f;
            expression.browsIn = 1f;
            expression.noseSneer = 0f;
            expression.leftBrowUp_Down = 0.4f;
            expression.rightBrowUp_Down = 0.4f;
        }

        dna = avatar.GetDNA(); //takes couple of frames 
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    avatar.SetSlot("Chest", "MaleHoodie_Recipe");
        //    avatar.BuildCharacter();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    avatar.ClearSlot("Chest");
        //    avatar.BuildCharacter();
        //}
        if (Input.GetKeyDown(KeyCode.Alpha1)) //load features from file
        {
           
            avatar.LoadFromTextFile(name);
            avatar.BuildCharacter();

         
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) //adjust and normalize buff 
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

        if (Input.GetKeyDown(KeyCode.Alpha3)) //normal/mid
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
        if (Input.GetKeyDown(KeyCode.Alpha4))  //weak condition
        {
            if (dna == null)
            {
                dna = avatar.GetDNA();
            }

            if (name.Equals("UMA_M6") || name.Equals("UMA_M7"))
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
    }
}
