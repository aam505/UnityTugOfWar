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
    int countdownStarter = 1;

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
    float trialDuration = 6;

    private float startCounter;
    private bool counting;

    public GameObject maleArms;
    public GameObject femaleArms;

    public GameObject trackedLeftHand;
    public GameObject trackedRightHand;
    public GameObject trackedHead;

    bool connected = true;
    //todo add none condition

    bool startedExperiment = false;

    Image uiImage;

   [SerializeField]
    Canvas parentCanvas;
   
    float fadeTime=2; // amount of time it takes to fade an image

    [SerializeField]
    float beforeStartCounter=30; // AMOUNT OF TIME BETWEEN SEEING THE AVATAR AND STARTING THE EXPERIMENT

    [SerializeField]
    float afterStopCounter=10; // AMOUNT OF TIME BETWEEN stopping pulling and blacking in


    bool proceded = false;


    IEnumerator DisplayImage(bool startWithImage)
    {
        

        if (startWithImage)
        {
            
            uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, 1);
            //Fade out for loop
            for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime / fadeTime)
            {
                uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, alpha);

                yield return null; // Wait for frame then return to execution
            }

            Debug.Log("Waiting for 30 seconds..");
            yield return new WaitForSeconds(beforeStartCounter);

            Debug.Log("Starting experiment");
            currentAvatar.GetComponent<Animator>().SetTrigger("Start");
            startCounter = Time.time;
            counting = true;
            Writer.logData.action = "start_experiment";


        }
        else
        {
            //uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, 0);
            //Fade out for loop
            Debug.Log("Waiting 10 seconds...");
            yield return new WaitForSeconds(afterStopCounter);

            Debug.Log("Blacking out...");

            for (float alpha = 0; alpha < 1; alpha += Time.deltaTime / fadeTime)
            {
                uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, alpha);

                yield return null; // Wait for frame then return to execution
            }
            uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, 1);
        }

    }

    public void Start()
    {
        ParticipantId = 0; //todo: remove

        trialOngoing = false;

        Writer.participantId = ParticipantId;
        Writer.condition = gender.ToString()+condition.ToString();

        if (gender == Gender.Female)
            maleArms.SetActive(false);
        else
            femaleArms.SetActive(false);

      

        if (parentCanvas.worldCamera != Camera.main)
            parentCanvas.worldCamera = Camera.main;

        uiImage = parentCanvas.GetComponentInChildren<Image>();

        uiImage.sprite = parentCanvas.transform.GetChild(0).GetComponent<Image>().sprite;

        //StartCoroutine(FadeInImage());

    }

    private void countToStart()
    {
        float seconds = getSeconds(startCounter);

        if (seconds < 4)
        {
            string secondsString = (4 - (int)seconds).ToString(); 
            startText.GetComponent<TMPro.TextMeshProUGUI>().text = secondsString;

            Writer.logData.action = "counting " + secondsString;

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
            Writer.logData.action = "start";

            startSound.GetComponent<AudioSource>().Play();
            currentAvatar.GetComponent<Animator>().SetTrigger("Pull"); //start pulling
            currentAvatar.GetComponent<UmaSettings>().setMood(1);
           // currentAvatar.GetComponent<UmaSettings>().setIKTargetHand();
            
        }

    }
    void Update()
    {
        
       

        if (trackedHead != null)
        {
            Writer.logData.setHead(trackedHead.transform.position.x, trackedHead.transform.position.y, trackedHead.transform.position.z, 
                trackedHead.transform.eulerAngles.x, trackedHead.transform.eulerAngles.y, trackedHead.transform.eulerAngles.z);
        }

        if (trackedLeftHand != null)
        {
            Writer.logData.setLeftHand(trackedLeftHand.transform.position.x, trackedLeftHand.transform.position.y, trackedLeftHand.transform.position.z,
                trackedLeftHand.transform.eulerAngles.x, trackedLeftHand.transform.eulerAngles.y, trackedLeftHand.transform.eulerAngles.z);
        }

        if (trackedRightHand != null)
        {
            Writer.logData.setRighttHand(trackedRightHand.transform.position.x, trackedRightHand.transform.position.y, trackedRightHand.transform.position.z,
                trackedRightHand.transform.eulerAngles.x, trackedRightHand.transform.eulerAngles.y, trackedRightHand.transform.eulerAngles.z);
        }
        if (startExperiment && !startedExperiment)
        {
            StartCoroutine(DisplayImage(true));
            startedExperiment = true;
        }
        if (!startExperiment)
        {
            endedTrial = 0;
            startedExperiment = false;
            countdownStarter = 1;
            trialOngoing = false;
            startedTrial = 0;
        }
        if (counting)
            countToStart();

        if (trialOngoing)
        {
            float seconds = getSeconds(startedTrial);
           
            if (seconds > 2)
            {
                startText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                Writer.logData.action = "";

            }
            if (seconds > trialDuration)
            {
                currentAvatar.GetComponent<Animator>().SetTrigger("Reverse");
                currentAvatar.GetComponent<UmaSettings>().setMood(0);
               // currentAvatar.GetComponent<UmaSettings>().setIKTargetHead();
                
                startText.GetComponent<TMPro.TextMeshProUGUI>().text = "STOP";
                Writer.logData.action = "stop";

                startSound.GetComponent<AudioSource>().Play();
                endedTrial = Time.time;
                trialOngoing = false;
                StartCoroutine(DisplayImage(false));
            }
        }
        else
        {
            if (endedTrial != 0)
            {
                float seconds = getSeconds(endedTrial);
                if (seconds > 2)
                {
                    startText.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                    Writer.logData.action = "";
                }
                if (seconds > postTrialDelay)
                {
                    endedTrial = 0;
                    Debug.Log("---Onto next trial");
                }
            }
        }
    }


    float getSeconds(float startOfTime)
    {
        float t = Time.time - startOfTime;
        float minutes = (int)t / 60;
        float seconds = (t % 60) + 1;
        return seconds;
    }
}