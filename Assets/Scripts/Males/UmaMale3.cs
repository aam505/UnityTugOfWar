﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA.CharacterSystem;
public class UmaMale3 : MonoBehaviour
{
    private DynamicCharacterAvatar avatar;
    private Dictionary<string, DnaSetter> dna;
    // Start is called before the first frame update
    void Start()
    {
        avatar = GetComponent<DynamicCharacterAvatar>();
        try { dna = avatar.GetDNA(); }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }//takes couple of frames 


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
           
            avatar.LoadFromTextFile("male3");
            avatar.BuildCharacter();
        }
    }
}
