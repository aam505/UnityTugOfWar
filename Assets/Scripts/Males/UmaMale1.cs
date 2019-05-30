using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;
public class UmaMale1 : MonoBehaviour
{
    private DynamicCharacterAvatar avatar;
    private Dictionary<string, DnaSetter> dna;
    // Start is called before the first frame update
    void Start()
    {
        avatar = GetComponent<DynamicCharacterAvatar>();
        dna = avatar.GetDNA(); //takes couple of frames 
        

    }

    // Update is called once per frame
    void Update()
    {
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
     
            avatar.LoadFromTextFile("male1");
            avatar.BuildCharacter();
        }
    }
}
