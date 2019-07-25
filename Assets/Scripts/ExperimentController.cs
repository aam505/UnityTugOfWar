using System;
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

    
    public int ParticipantId;
    public bool startExperiment;
    private bool started = false;
    float startedTrialTime;
    bool finishedExperiment = false;
    public GameObject participantLeftHand;
    public GameObject participantRightHand;
    public Gender gender;
    public Condition condition;
    bool trialOngoing;
    float startedTrial;
    float endedTrial;
    

   // [SerializeField]
   // public GameObject FemaleAvatar;
   // [SerializeField]
   // public GameObject MaleAvatars;
   
   
    public GameObject startText;
    public GameObject countdownSound;
    public GameObject startSound;

    public Dictionary<string, GameObject> avatarGameObjects = new Dictionary<string, GameObject>();

    public Transform currentAvatar;

    float postTrialDelay = 60;
    float trialDuration = 10;

    private float startCounter;
    private bool counting;

    public GameObject maleArms;
    public GameObject femaleArms;

    bool connected = true;
    //todo add none condition

 

    bool startedExperiment = false;

    public void Start()
    {

        ParticipantId = 0;
        trialOngoing = false;

        if (gender == Gender.Female)
            maleArms.SetActive(false);
        else
            femaleArms.SetActive(false);
      

        if (gender == Gender.Female) {
            maleArms.SetActive(false);
       
        }
        else {
            femaleArms.SetActive(false);
           
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

            currentAvatar.GetComponent<UmaSettings>().setMood(1);
           // currentAvatar.GetComponent<UmaSettings>().setIKTargetHand();
          
                
            
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
                currentAvatar.GetComponent<UmaSettings>().setMood(0);
               // currentAvatar.GetComponent<UmaSettings>().setIKTargetHead();
                
                startText.GetComponent<TMPro.TextMeshProUGUI>().text = "STOP";

                startSound.GetComponent<AudioSource>().Play();
                endedTrial = Time.time;
                trialOngoing = false;
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