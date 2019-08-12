using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
  
    public Gender gender;
    public GameObject myPrefab;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    bool trialOngoing;
    float startedTrial;
    float endedTrial;
    int countdownStarter = 1;
    int blackDuration = 2;

    public GameObject startText;
    public GameObject countdownSound;
    public GameObject startSound;

    public Dictionary<string, GameObject> avatarGameObjects = new Dictionary<string, GameObject>();

    Transform currentAvatar;
    public Transform avatarParent;

    float postTrialDelay = 60;
    float trialDuration = 10;

    private float startCounter;
    private bool counting;

    public GameObject maleArms;
    public GameObject femaleArms;

    public GameObject trackedLeftHand;
    public GameObject trackedRightHand;
    public GameObject trackedHead;

    bool connected = true;

    bool startedExperiment = false;
    public int currentAvatarIdx = -1;

    Image uiImage;

    [SerializeField]
    Canvas parentCanvas;

    float fadeTime = 2; // amount of time it takes to fade an image

    [SerializeField]
    float beforeStartCounter; // AMOUNT OF TIME BETWEEN SEEING THE AVATAR AND STARTING THE EXPERIMENT

    [SerializeField]
    float afterStopCounter = 10; // AMOUNT OF TIME BETWEEN stopping pulling and blacking in

    GameObject quizzCanvas;

    bool first = true;

    public float preTrialDuration = 30;
    Condition currentCondition;
    GameObject arms;
    IEnumerator DisplayImage(bool startWithImage)
    {

        if (startWithImage) //black out  -- going out of black screen
        {

            uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, 1);
            Writer.logData.splashScreen = "blackOut";
            //Fade out for loop
            for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime / fadeTime)
            {
                uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, alpha);

                yield return null; // Wait for frame then return to execution
            }
            uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, 0);
            Writer.logData.splashScreen = "";
            parentCanvas.gameObject.SetActive(false);
        }
        else //black in --- going into black screen
        {
            //uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, 0);
            //Fade out for loop
            parentCanvas.gameObject.SetActive(true);
            Writer.logData.splashScreen = "blackIn";
            for (float alpha = 0; alpha < 1; alpha += Time.deltaTime / fadeTime)
            {
                uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, alpha);

                yield return null; // Wait for frame then return to execution
            }
            uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, 1);
            Writer.logData.splashScreen = "";

        }

    }

    public void Start()
    {

        //Application.targetFrameRate = 60;
        ArmsrHand = GameObject.Find("ArmsrHand");
        ArmslHand = GameObject.Find("ArmslHand");

        lHand = GameObject.Find("lHand");
        rHand = GameObject.Find("rHand");

        currentAvatar = avatarParent.transform.Find("Avatar");

        quizzCanvas = GameObject.Find("QuizzCanvas");
        quizz = GameObject.Find("QUIZZ");

        setActiveQuizzPanel(false);

        uiImage = parentCanvas.GetComponentInChildren<Image>();
        uiImage.sprite = parentCanvas.transform.GetChild(0).GetComponent<Image>().sprite;

        Writer.participantId = ParticipantId;
        Writer.gender = gender.ToString();

        if (gender == Gender.Female)
        {
            maleArms.SetActive(false);   //female arms

            GameObject rht = GameObject.Find("RHT");
            rht.transform.localPosition = new Vector3(0.0268f, 0.0099f, 0.0271f);

            GameObject rhp = GameObject.Find("RHP");
            rhp.transform.localPosition = new Vector3(-0.0036f, -0.0135f, -0.0165f);

            GameObject lht = GameObject.Find("LHT");
            lht.transform.localPosition = new Vector3(-0.0124f, 0.0095f, 0.035f);

            GameObject lhp = GameObject.Find("LHP");
            lhp.transform.localPosition = new Vector3(-0.0077f, -0.0153f, -0.0162f);

            //lht.transform.parent = ArmslHand.transform;
            //lhp.transform.parent = ArmslHand.transform;

            //rht.transform.parent = ArmsrHand.transform;
            //rhp.transform.parent = ArmsrHand.transform;


          //  moveArms = true;
        }
        else
        {
            femaleArms.SetActive(false); //male arms

            GameObject lht = GameObject.Find("LHT");
            lht.transform.localPosition = new Vector3(-0.0176f, 0.0292f, 0.0137f);

            GameObject lhp = GameObject.Find("LHP");
            lhp.transform.localPosition = new Vector3(-0.0105f, -0.0181f, -0.023f);


            GameObject rht = GameObject.Find("RHT");
            rht.transform.localPosition = new Vector3(0.0237f, 0.0129f, 0.0276f);

            GameObject rhp = GameObject.Find("RHP");
            rhp.transform.localPosition = new Vector3(0.0225f, -0.0145f, -0.013f);

            //lht.transform.parent = ArmslHand.transform;
            //lhp.transform.parent = ArmslHand.transform;

            //rht.transform.parent = ArmsrHand.transform;
            //rhp.transform.parent = ArmsrHand.transform;

            //moveArms = true;
        }

        if (currentAvatar != null)
        {
            initialPosition = currentAvatar.transform.position;
            initialRotation = currentAvatar.transform.rotation;
        }

        trialOngoing = false;

    }

    bool moveArms = false;
    void setActiveQuizzPanel(bool active)
    {
        quizzCanvas.SetActive(active);
        quizz.SetActive(active);
    }
    IEnumerator SpawnAvatar()
    {
        if (!parentCanvas.gameObject.activeSelf)
            StartCoroutine(DisplayImage(false)); //blacking in

        yield return new WaitForSeconds(blackDuration);

        if (quizzCanvas.activeSelf == true)  //disable canvas
            setActiveQuizzPanel(false);
        
        Writer.logData.action = "start_trial";

        if (currentAvatarIdx < 5)
        {
            if (!first)
            {

                currentAvatar = Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity).transform;
                currentAvatar.parent = avatarParent;
                currentAvatar.position = initialPosition;
                currentAvatar.rotation = initialRotation;
                currentAvatar.name = "Avatar";
                currentAvatar.localScale = new Vector3(1, 1, 1);
            }

  
             Writer.logData.conditon = originalAvatarsList[currentAvatarIdx].condition.ToString()+ " " + originalAvatarsList[currentAvatarIdx].name.ToString();

            yield return new WaitForSeconds(blackDuration);
            StartCoroutine(DisplayImage(true)); //blacking out


            Debug.Log("Starting countdown in " + beforeStartCounter);
            currentAvatar.GetComponent<Animator>().SetTrigger("Start");
            Writer.logData.action = "pending_pulling";

            yield return new WaitForSeconds(beforeStartCounter);

            Debug.Log("Starting countdown...");
            startCounter = Time.time;
           counting = true;
            Writer.logData.action = "start_counter";
        }
        else
        {
            Writer.logData.conditon = "";
            Writer.logData.action = "end";

            yield return new WaitForSeconds(blackDuration);
            StartCoroutine(DisplayImage(true)); //blacking out

            startText.GetComponent<TMPro.TextMeshProUGUI>().text = "THE END!";
            Writer.outputfile.Close();

        }

        yield return null;
    }

    IEnumerator SpawnQuizz()
    {
        //blacking in
        StartCoroutine(DisplayImage(false));
        yield return new WaitForSeconds(blackDuration);

        parentHandles();

        if(currentAvatar!=null)
            Destroy(avatarParent.transform.Find("Avatar").gameObject);  //destroy current avatar

        Writer.logData.action = "quizz_active";

        yield return new WaitForSeconds(blackDuration);
        StartCoroutine(DisplayImage(true)); //blacking out


        if (quizzCanvas.activeSelf == false)
            setActiveQuizzPanel(true);
        else
            Debug.LogError("Canvas already active.");
        yield return null;

    }


    IEnumerator SpawnQuizzDup()
    {
  
        yield return new WaitForSeconds(blackDuration);

        parentHandles();

        if (currentAvatar != null)
            Destroy(avatarParent.transform.Find("Avatar").gameObject);  //destroy current avatar

        Writer.logData.action = "quizz_active";

        yield return new WaitForSeconds(blackDuration);
        StartCoroutine(DisplayImage(true)); //blacking out


        if (quizzCanvas.activeSelf == false)
            setActiveQuizzPanel(true);
        else
            Debug.LogError("Canvas already active.");


        Debug.Log("Waiting to black out..");
        yield return new WaitForSeconds(preTrialDuration);
        StartCoroutine(DisplayImage(false)); //blacking in
        //
        yield return null;

    }

    public List<UmaSettings.Avatar> originalAvatarsList;

    bool pressed = false;
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
            Writer.logData.action = "start_pulling";

            startSound.GetComponent<AudioSource>().Play();

            currentAvatar.GetComponent<Animator>().SetTrigger("Pull"); //start pulling

            string umaName = originalAvatarsList[currentAvatarIdx].name;
           
            if (umaName.Equals("UMA_M6") || umaName.Equals("UMA_M7") || umaName.Equals("UMA_F3") || umaName.Equals("UMA_F4")) //if o3n race
            {
           
                currentAvatar.GetComponent<UmaSettings>().setMood(2);
            }
            else
                currentAvatar.GetComponent<UmaSettings>().setMood(1);


            currentAvatar.GetComponent<Animator>().SetTrigger("PushBack");
            Writer.logData.action = "push_back";

        }

    }

    bool experimentStarted = false;
    private GameObject quizz;
    private bool nullCondition=false;
    GameObject lHand, rHand, ArmsrHand, ArmslHand;
    void Update()
    {
       
        if (first && !experimentStarted)
        {
            if (currentAvatar != null)
            {
                experimentStarted = true;
                startExperiment = true;
                Writer.logging = true;
                pressed = true;
            }
            else
            {
                if (!nullCondition)
                {
                    nullCondition = true;                 
                    StartCoroutine(SpawnQuizzDup());
                }
            }
        }
        if (Input.GetKeyDown("space"))
        {
            if (!pressed)
            {
                startExperiment = true;
                pressed = true;

            }
            else
            {
                pressed = false;
                startExperiment = false;
            }

        }

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
            StartCoroutine(SpawnAvatar()); //black out and then start experiment after 30 seconds
            startedExperiment = true;
        }

        if (!startExperiment)
        {
            endedTrial = 0;
            startedExperiment = false;
            countdownStarter = 1;
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
                //Writer.logData.action = "";

            }
            if (seconds > trialDuration)
            {

                currentAvatar.GetComponent<Animator>().SetTrigger("Reverse");
                currentAvatar.GetComponent<UmaSettings>().setMood(0);


                startText.GetComponent<TMPro.TextMeshProUGUI>().text = "STOP";
                Writer.logData.action = "stop";

                startSound.GetComponent<AudioSource>().Play();
                endedTrial = Time.time;
                trialOngoing = false;
                Debug.Log("Blacking out in 10s");

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
                    Writer.logData.action = "pending_quizz";
                }
                if (seconds > 10)
                {
                    endedTrial = 0;
                    if (first)
                        first = false;
                    currentAvatarIdx++;
                    StartCoroutine(SpawnQuizz());
                }
            }
        }
    }

    void parentHandles()
    {


        Transform pinkyHandleL = GameObject.Find("ObiHandleAvatarPinkyL").transform;
        Transform thumbHandleL = GameObject.Find("ObiHandleAvatarThumbL").transform;

        Transform thumbHandleR = GameObject.Find("ObiHandleAvatarThumbR").transform;
        //Transform pinkyHandleR = GameObject.Find("ObiHandleAvatarPinkyR").transform;

        thumbHandleL.transform.parent = avatarParent;
        pinkyHandleL.transform.parent = avatarParent;

        thumbHandleR.transform.parent = avatarParent;
        //pinkyHandleR.transform.parent = avatarParent;

        thumbHandleL.transform.localPosition = new Vector3(-0.239f, 0.805f, 0.548f);
        pinkyHandleL.transform.localPosition = new Vector3(-0.34f, 0.86f, 0.539f);
        thumbHandleR.transform.localPosition = new Vector3(-0.012f, 0.844f, 0.55f);


        //pinkyHandleR.transform.localPosition = new Vector3(-0.205f, 0.093f, 0.688f);
    }


    float getSeconds(float startOfTime)
    {
        float t = Time.time - startOfTime;
        float minutes = (int)t / 60;
        float seconds = (t % 60) + 1;
        return seconds;
    }
}