using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentController : MonoBehaviour
{
    enum Gender // your custom enumeration
    {
        Male,
        Female
    };


    

    List<string> Conditions = new List<string>() { "Weak","Average","Strong" };

    [SerializeField]
    public InputField pidInput;
    int ParticipantId;



    [SerializeField]
    public Dropdown GenderDropdown;
    public Dropdown C1Dropdown;
    public Dropdown C2Dropdown;
    public Dropdown C3Dropdown;

    public List<Dropdown> ConditionDropdowns;
    public Button start;
    public int currentTrial = -1;
    private bool started = false;
    float startedTrialTime;
    public bool finishedExperiment =false;

    public GameObject participantLeftHand;
    public GameObject participantRightHand;
    private GameObject leftHandHandle;
    private GameObject rightHandHandle;

    TrialController TrialController;

    List<string> orderedConditions = new List<string>();

    public void StartExperiment()
    {
        if (System.Int32.TryParse(pidInput.text, out ParticipantId))
        {

            currentTrial = 0;
           
            Debug.Log("Starting experiment...");
            foreach(Dropdown dropdown in ConditionDropdowns)
            {
                orderedConditions.Add(GenderDropdown.options[GenderDropdown.value].text + dropdown.options[dropdown.value].text);
            }
            transform.GetChild(0).gameObject.SetActive(false); //setting canvas inactive 
            Debug.Log("--Conditions");
            foreach (string condition in orderedConditions)
            {
                Debug.Log(condition);
            }

            startTrial();
            startedTrialTime = Time.time;

        }
        else
            Debug.LogError("Wrong participant id.");
    }

    // Start is called before the first frame update
    void Start()
    {
        TrialController = transform.gameObject.GetComponent<TrialController>();
        pidInput.text = "0";
        ConditionDropdowns.Add(C1Dropdown);
        ConditionDropdowns.Add(C2Dropdown);
        ConditionDropdowns.Add(C3Dropdown);

        addOptions();

        leftHandHandle = GameObject.Find("ObiHandleLeftHand");
        rightHandHandle = GameObject.Find("ObiHandleRightHand");

       

        //todo delete
        ConditionDropdowns[1].value = 1;
        ConditionDropdowns[2].value = 2;
     

    }

    public void startTrial()
    {
        
        Debug.Log("Starting trial in experiment controller: " + (currentTrial + 1)   + "/" + Conditions.Count +" " +orderedConditions[currentTrial]);
        TrialController.startTrial(orderedConditions[currentTrial]);
    }
    public void nextTrial()
    {
        if (!finishedExperiment)
        {
            float t = Time.time - startedTrialTime;
            float minutes = (int)t / 60;
            float seconds = (t % 60) + 1;

            currentTrial++;
            if (currentTrial < Conditions.Count)
            {
                startedTrialTime = Time.time;
                Debug.Log("Finished trial in experiment controller " + (currentTrial + 1) + "; Seconds elapsed:" + seconds);
                startTrial();
            }
            else
            {
                finishedExperiment = true;
                Debug.Log("Experiment finished. Last condition:" + orderedConditions[currentTrial] + "; Seconds elapsed:" + seconds);
            }
        }

    }
    void addOptions()
    {
        for(int i = 0; i < ConditionDropdowns.Count; i++)
        {
            ConditionDropdowns[i].ClearOptions();
            ConditionDropdowns[i].AddOptions(Conditions);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
