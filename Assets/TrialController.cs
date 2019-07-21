using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;

public class TrialController : MonoBehaviour
{
    bool trialOngoing;
    float startedTrial;
    float endedTrial;

    bool startedExperiment;
    [SerializeField]
    public GameObject setting;
    [SerializeField]
    public GameObject FemaleStrong;
    [SerializeField]
    public GameObject FemaleAverage;
    [SerializeField]
    public GameObject FemaleWeak;

    public Dictionary<string, GameObject> avatarGameObjects = new Dictionary<string, GameObject>();

    GameObject currentAvatar;
    ExperimentController experimentController;

    float postTrialDelay=10;
    float trialDuration = 20;

    public void OnCreated(UMAData data)
    {
        data.gameObject.SetActive(false);

    }


    // Start is called before the first frame update
    void Start()
    {
        trialOngoing = false;
        experimentController = transform.GetComponent<ExperimentController>();
        avatarGameObjects.Add("FemaleStrong", FemaleStrong);
        avatarGameObjects.Add("FemaleAverage", FemaleAverage);
        avatarGameObjects.Add("FemaleWeak", FemaleWeak);

        foreach (GameObject avatar in avatarGameObjects.Values)
        {
            DynamicCharacterAvatar uma = avatar.transform.GetComponent<DynamicCharacterAvatar>();

            uma.CharacterCreated.AddListener(OnCreated);

            //avatar.SetActive(false);
        }
        //todomales
    }

    // Update is called once per frame
    void Update()
    {

        if (trialOngoing)
        {
            float t = Time.time - startedTrial;
            float minutes = (int)t / 60;
            float seconds = (t % 60) + 1;
          
            if (seconds > trialDuration)
            {
                
                currentAvatar.GetComponent<Animator>().SetTrigger("Exit");
                endedTrial = Time.time;
                trialOngoing = false;
                Debug.Log("---Ending trial in trial controller "+" after " + postTrialDelay + "s");

            }
        }
        else
        {
            if (endedTrial != 0)
            {
                float t = Time.time - endedTrial;
                float minutes = (int)t / 60;
                float seconds = (t % 60) + 1;
                if (seconds > postTrialDelay)
                {
                    currentAvatar.SetActive(false);
                    endedTrial = 0;
                    Debug.Log("---Onto next trial");
                    experimentController.nextTrial();
                }
            }
        }
    }


    public void startTrial(string genderCondition)
    {
        
        trialOngoing = true;

        Debug.Log("Starting trial in trial controller " + genderCondition);

        startedTrial = Time.time;

        currentAvatar = avatarGameObjects[genderCondition];
        currentAvatar.SetActive(true);
        currentAvatar.GetComponent<Animator>().SetTrigger("Start");
    }
}
