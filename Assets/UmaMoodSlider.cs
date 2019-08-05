using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using UMA.PoseTools;

public class UmaMoodSlider : MonoBehaviour
{

    private DynamicCharacterAvatar avatar;
    private ExpressionPlayer expression;
    private bool connected = false;


    [Range(0, 2)]
    public int mood;

    void OnEnable()
    {
        avatar = GetComponent<DynamicCharacterAvatar>();
        avatar.CharacterCreated.AddListener(OnCreated);
    }

    void OnDisable()
    {
        avatar.CharacterCreated.RemoveListener(OnCreated);
    }

    public void OnCreated(UMAData data)
    {
        expression = GetComponent<ExpressionPlayer>();
        expression.enableBlinking = true;
        expression.enableSaccades = false;
        connected = true;
        expression.gazeMode = ExpressionPlayer.GazeMode.Acquiring;

    }

    void Update()
    {
        if (connected)
        {
            float delta = 5 * Time.deltaTime;
            switch (mood)
            {

                //todo add special for o3n 
                case 0:

                    expression.leftMouthSmile_Frown = Mathf.Lerp(expression.leftMouthSmile_Frown, 0, delta);
                    expression.rightMouthSmile_Frown = Mathf.Lerp(expression.rightMouthSmile_Frown, 0, delta);

                    expression.midBrowUp_Down = Mathf.Lerp(expression.midBrowUp_Down, -1f, delta);
                    expression.leftBrowUp_Down = Mathf.Lerp(expression.leftBrowUp_Down, 0.3f, delta);
                    expression.rightBrowUp_Down = Mathf.Lerp(expression.rightBrowUp_Down, 0.3f, delta);


                    expression.rightUpperLipUp_Down = Mathf.Lerp(expression.rightUpperLipUp_Down, 0, delta);
                    expression.leftUpperLipUp_Down = Mathf.Lerp(expression.leftUpperLipUp_Down, 0, delta);
                    expression.rightLowerLipUp_Down = Mathf.Lerp(expression.rightLowerLipUp_Down, 0.4f, delta);
                    expression.leftLowerLipUp_Down = Mathf.Lerp(expression.leftLowerLipUp_Down, 0.4f, delta);


                    expression.mouthUp_Down = Mathf.Lerp(expression.mouthUp_Down, -0.4f, delta);

                    expression.mouthNarrow_Pucker = Mathf.Lerp(expression.mouthNarrow_Pucker, 0, delta);
                    expression.jawOpen_Close = Mathf.Lerp(expression.jawOpen_Close, 0, delta);
                    expression.noseSneer = Mathf.Lerp(expression.noseSneer, 0, delta);
                    expression.leftEyeOpen_Close = Mathf.Lerp(expression.leftEyeOpen_Close, 0, delta);
                    expression.rightEyeOpen_Close = Mathf.Lerp(expression.rightEyeOpen_Close, 0, delta);

                    break;

                case 1:
                    expression.rightMouthSmile_Frown = Mathf.Lerp(expression.rightMouthSmile_Frown, 0.3f, delta);
                    expression.leftMouthSmile_Frown = Mathf.Lerp(expression.leftMouthSmile_Frown, 0.3f, delta);

                    expression.midBrowUp_Down = Mathf.Lerp(expression.midBrowUp_Down, -1f, delta);
                    expression.leftBrowUp_Down = Mathf.Lerp(expression.leftBrowUp_Down, 1f, delta);
                    expression.rightBrowUp_Down = Mathf.Lerp(expression.rightBrowUp_Down, 1f, delta);
                    expression.rightUpperLipUp_Down = Mathf.Lerp(expression.rightUpperLipUp_Down, 0.7f, delta);
                    expression.leftUpperLipUp_Down = Mathf.Lerp(expression.leftUpperLipUp_Down, 0.7f, delta);
                    expression.rightLowerLipUp_Down = Mathf.Lerp(expression.rightLowerLipUp_Down, -0.7f, delta);
                    expression.leftLowerLipUp_Down = Mathf.Lerp(expression.leftLowerLipUp_Down, -0.7f, delta);
                    expression.mouthNarrow_Pucker = Mathf.Lerp(expression.mouthNarrow_Pucker, 0.7f, delta);
                    expression.jawOpen_Close = Mathf.Lerp(expression.jawOpen_Close, -0.3f, delta);
                    expression.noseSneer = Mathf.Lerp(expression.noseSneer, 0.3f, delta);
                    expression.leftEyeOpen_Close = Mathf.Lerp(expression.leftEyeOpen_Close, -0.2f, delta);
                    expression.rightEyeOpen_Close = Mathf.Lerp(expression.rightEyeOpen_Close, -0.2f, delta);
                     break;

                case 3:
                    break;

                default:
                    break;
            }
        }
    }
}