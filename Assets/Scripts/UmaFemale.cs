using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA.CharacterSystem;
public class UmaFemale : MonoBehaviour
{
    private DynamicCharacterAvatar avatar;
    private Dictionary<string, DnaSetter> dna;
    // Start is called before the first frame update
    private string umaName;
    void Start()
    {
        avatar = GetComponent<DynamicCharacterAvatar>();

        umaName = transform.gameObject.name;

    }

    // Update is called once per frame
    void Update()
    {
        dna = avatar.GetDNA(); //takes couple of frames 
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            avatar.SetSlot("Chest", "MaleHoodie_Recipe");
            avatar.BuildCharacter();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            avatar.ClearSlot("Chest");
            avatar.BuildCharacter();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            avatar.LoadFromTextFile(name);
            avatar.characterColors.SetColor("Skin", new UnityEngine.Color(236f / 255f, 232f / 255f, 232f / 255f));
            avatar.characterColors.SetColor("Eyes", new UnityEngine.Color(99f / 255f, 67f / 255f, 67f / 255f));
            avatar.BuildCharacter();


        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (dna == null)
            {
                dna = avatar.GetDNA();
            }
            avatar.characterColors.SetColor("Shirt", UnityEngine.Color.black);
            avatar.characterColors.SetColor("Eyes", new UnityEngine.Color(99f / 255f, 67f / 255f, 67f / 255f));
            avatar.characterColors.SetColor("Skin", new UnityEngine.Color(236f / 255f, 232f / 255f, 232f / 255f));
            avatar.characterColors.SetColor("Pants1", new UnityEngine.Color(20f / 255f, 18f / 255f, 36f / 255f));

            dna["armWidth"].Set(1f);
            dna["forearmWidth"].Set(1f);
            dna["lowerWeight"].Set(1f);
            dna["neckThickness"].Set(0.8f);
            dna["chinPronounced"].Set(1f);
            dna["chinSize"].Set(1f);
            dna["upperMuscle"].Set(0.7f);
            dna["upperWeight"].Set(0.7f);
            avatar.BuildCharacter();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (dna == null)
            {
                dna = avatar.GetDNA();
            }
            avatar.characterColors.SetColor("Skin", new UnityEngine.Color(236f / 255f, 232f / 255f, 232f / 255f));
            avatar.characterColors.SetColor("Shirt", UnityEngine.Color.white);
            avatar.characterColors.SetColor("Eyes", new UnityEngine.Color(99f / 255f, 67f / 255f, 67f / 255f));
            avatar.characterColors.SetColor("Pants1", new UnityEngine.Color(120f / 255f, 141f / 255f, 183f / 255f));
            dna["armWidth"].Set(0.5f);
            dna["forearmWidth"].Set(0.5f);
            dna["lowerWeight"].Set(0.5f);
            dna["neckThickness"].Set(0.5f);
            dna["chinPronounced"].Set(0.5f);
            dna["chinSize"].Set(0.5f);
            dna["upperMuscle"].Set(0.5f);
            dna["upperWeight"].Set(0.5f);
            avatar.BuildCharacter();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (dna == null)
            {
                dna = avatar.GetDNA();
            }
            avatar.characterColors.SetColor("Skin", new UnityEngine.Color(236f / 255f, 232f / 255f, 232f / 255f));
            avatar.characterColors.SetColor("Shirt", UnityEngine.Color.white);
            avatar.characterColors.SetColor("Eyes", new UnityEngine.Color(99f / 255f, 67f / 255f, 67f / 255f));
            avatar.characterColors.SetColor("Pants1", new UnityEngine.Color(120f / 255f, 141f / 255f, 183f / 255f));
            dna["armWidth"].Set(0.2f);
            dna["forearmWidth"].Set(0.2f);
            dna["lowerWeight"].Set(0.2f);
            dna["neckThickness"].Set(0.2f);
            dna["chinPronounced"].Set(0.2f);
            dna["chinSize"].Set(0.2f);
            dna["upperMuscle"].Set(0.2f);
            dna["upperWeight"].Set(0.2f);
            avatar.BuildCharacter();
        }
    }
}
