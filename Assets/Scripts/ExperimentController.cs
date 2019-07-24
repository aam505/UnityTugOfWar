﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UMA;
using UMA.CharacterSystem;

public class ExperimentController : MonoBehaviour
{
    public enum Gender // your custom enumeration
    {
        Male,
        Female
    };

    public enum Condition
    {
        Weak,
        Average,
        Strong
    };


    [SerializeField]
    public int pid;
    int ParticipantId;
    private bool started = false;
    float startedTrialTime;
    public bool finishedExperiment = false;
    public GameObject participantLeftHand;
    public GameObject participantRightHand;
    private GameObject leftHandHandle;
    private GameObject rightHandHandle;
    public Gender gender;
    public Condition condition;
    bool trialOngoing;
    float startedTrial;
    float endedTrial;
    public bool startExperiment;

    [SerializeField]
    public GameObject FemaleStrong;
    [SerializeField]
    public GameObject FemaleAverage;
    [SerializeField]
    public GameObject FemaleWeak;

    [SerializeField]
    public GameObject MaleStrong;
    [SerializeField]
    public GameObject MaleAverage;
    [SerializeField]
    public GameObject MaleWeak;
   
    public GameObject startText;
    public GameObject countdownSound;
    public GameObject startSound;

    public Dictionary<string, GameObject> avatarGameObjects = new Dictionary<string, GameObject>();

    Transform currentAvatar;

    float postTrialDelay = 60;
    float trialDuration = 10;

    private float startCounter;
    private bool counting;

    public GameObject maleArms;
    public GameObject femaleArms;
    //todo add none condition

    public void OnCreated(UMAData data)
    {
        //disable all
        data.gameObject.SetActive(false);

        //enable right one
        if (gender == Gender.Female)
        {
            if (data.transform.GetComponent<UmaFemale>().condition.ToString().Equals(condition.ToString()))
            {
                data.gameObject.SetActive(true);
                currentAvatar = data.transform;
                currentAvatar.GetComponent<UmaFemale>().ParentLastPiece();
                currentAvatar.GetComponent<UmaFemale>().ParentSecondToLastPiece();
              
            }
        }

        if (gender == Gender.Male)
        {
            if (data.transform.GetComponent<UmaMale>().condition.ToString().Equals(condition.ToString()))
            {
                data.gameObject.SetActive(true);
                currentAvatar = data.transform;
                currentAvatar.GetComponent<UmaMale>().ParentLastPiece();
                currentAvatar.GetComponent<UmaMale>().ParentSecondToLastPiece();
               
            }
        }
    }

    bool startedExperiment = false;

    public void Start()
    {

        pid = 0;
        trialOngoing = false;

        if (gender == Gender.Female)
            maleArms.SetActive(false);
        else
            femaleArms.SetActive(false);

        leftHandHandle = GameObject.Find("ObiHandleLeftHand");
        rightHandHandle = GameObject.Find("ObiHandleRightHand");
       
        avatarGameObjects.Add("FemaleStrong", FemaleStrong);
        avatarGameObjects.Add("FemaleAverage", FemaleAverage);
        avatarGameObjects.Add("FemaleWeak", FemaleWeak);

        avatarGameObjects.Add("MaleStrong", MaleStrong);
        avatarGameObjects.Add("MaleAverage", MaleAverage);
        avatarGameObjects.Add("MaleWeak", MaleWeak);

        foreach (GameObject avatar in avatarGameObjects.Values)
        {
            DynamicCharacterAvatar uma = avatar.transform.GetComponent<DynamicCharacterAvatar>();
            uma.CharacterCreated.AddListener(OnCreated);
        }      
    }
 

    int countdownStarter = 1;

    private void countToStart()
    {
        float t = Time.time - startCounter;
        float minutes = (int)t / 60;
        float seconds = (t % 60) + 1;

        if (seconds < 4)
        {

            startText.GetComponent<TMPro.TextMeshProUGUI>().text = (4-(int)seconds).ToString();
            if ((int)seconds == 1 && countdownStarter == 1)
            {
                countdownSound.GetComponent<AudioSource>().Play();
                countdownStarter = 2;
            }
            if ((int)seconds == 2 && countdownStarter == 2)
            {
                countdownSound.GetComponent<AudioSource>().Play();
                countdownStarter = 3;
            }
            if ((int)seconds == 3 && countdownStarter == 3)
            {
                countdownSound.GetComponent<AudioSource>().Play();
                countdownStarter = 4;
            }


        }
        else
        {
            counting = false;
            startedTrial = Time.time;
            trialOngoing = true;
            startText.GetComponent<TMPro.TextMeshProUGUI>().text = "START";
            startSound.GetComponent<AudioSource>().Play();
            currentAvatar.GetComponent<Animator>().SetTrigger("Pull"); //start pulling
        }

    }
    void Update()
    {
        if (startExperiment && !startedExperiment)
        {
            currentAvatar.GetComponent<Animator>().SetTrigger("Start");
            startCounter = Time.time;
            counting = true;
            startedExperiment = true;

        }
        if (counting)
            countToStart();

        if (trialOngoing)
        {
            float t = Time.time - startedTrial;
            float minutes = (int)t / 60;
            float seconds = (t % 60) + 1;
           
            if (seconds > 2)
            {
                startText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            }
            if (seconds > trialDuration)
            {
                currentAvatar.GetComponent<Animator>().SetTrigger("Reverse");
                startText.GetComponent<TMPro.TextMeshProUGUI>().text = "STOP";
                startSound.GetComponent<AudioSource>().Play();

                endedTrial = Time.time;
                trialOngoing = false;
                Debug.Log("---Ending trial in trial controller " + " after " + postTrialDelay + "s");
               
            }
        }
        else
        {
            if (endedTrial != 0)
            {
                float t = Time.time - endedTrial;
                float minutes = (int)t / 60;
                float seconds = (t % 60) + 1;
                if (seconds > 2)
                {
                    startText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                }
                if (seconds > postTrialDelay)
                {
                    endedTrial = 0;
                    Debug.Log("---Onto next trial");
                }
            }
        }
    }
}