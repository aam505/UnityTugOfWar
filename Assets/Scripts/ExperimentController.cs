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

    TrialController TrialController;

    public Gender gender;
    public Condition condition;



    bool trialOngoing;
    float startedTrial;
    float endedTrial;

    public bool startExperiment;
    [SerializeField]
    public GameObject setting;
    [SerializeField]
    public GameObject FemaleStrong;
    [SerializeField]
    public GameObject FemaleAverage;
    [SerializeField]
    public GameObject FemaleWeak;


    public GameObject startText;
    public GameObject countdownSound;
    public GameObject startSound;

    public Dictionary<string, GameObject> avatarGameObjects = new Dictionary<string, GameObject>();

    Transform currentAvatar;
    ExperimentController experimentController;

    float postTrialDelay = 60;
    float trialDuration = 10;

    private float startCounter;
    private bool counting;



    //todo add none condition

    public void OnCreated(UMAData data)
    {
        
        

        if (gender == Gender.Female)
        {
            if (data.transform.GetComponent<UmaFemale>().condition.ToString().Equals(condition.ToString()))
            {
                currentAvatar = data.transform;
                currentAvatar.GetComponent<UmaFemale>().ParentLastPiece();
                currentAvatar.GetComponent<UmaFemale>().ParentSecondToLastPiece();
            }
            else
            {
                data.gameObject.SetActive(false);
            }
        }
      //  else if (data.transform.GetComponent<UmaMale>().condition.ToString().Equals(condition.ToString()))
      //  {
      //      currentAvatar = data.transform;
      //  }
    }

    bool startedExperiment = false;

    public void Start()
    {
      
        pid = 0;

        leftHandHandle = GameObject.Find("ObiHandleLeftHand");
        rightHandHandle = GameObject.Find("ObiHandleRightHand");

        trialOngoing = false;
        experimentController = transform.GetComponent<ExperimentController>();
        avatarGameObjects.Add("FemaleStrong", FemaleStrong);
        avatarGameObjects.Add("FemaleAverage", FemaleAverage);
        avatarGameObjects.Add("FemaleWeak", FemaleWeak);

        foreach (GameObject avatar in avatarGameObjects.Values)
        {
            DynamicCharacterAvatar uma = avatar.transform.GetComponent<DynamicCharacterAvatar>();

            uma.CharacterCreated.AddListener(OnCreated);

          
        }
        //todomales

      
    }
 

    int countdownStarter = 1;

    private void countToStart()
    {
        float t = Time.time - startCounter;
        float minutes = (int)t / 60;
        float seconds = (t % 60) + 1;

        if (seconds < 4)
        {

            startText.GetComponent<TMPro.TextMeshProUGUI>().text = ((int)seconds).ToString();
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


            if (seconds > 1)
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
                    //GameObject.Find("ObiHandleLeft").transform.parent = transform;
                    //GameObject.Find("ObiHandleRight").transform.parent = transform;
                    //currentAvatar.SetActive(false);
                    endedTrial = 0;
                    Debug.Log("---Onto next trial");
                    //experimentController.nextTrial();
                }
            }
        }
    }
}