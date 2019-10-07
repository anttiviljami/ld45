using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnboardingSequenceController : MonoBehaviour
{
    [SerializeField]
    private GameObject overlay = default;

    [SerializeField]
    private GameObject title = default;

    [SerializeField]
    private GameObject credits = default;

    [SerializeField]
    private GameObject instruction1 = default;

    [SerializeField]
    private GameObject instruction2 = default;

    [SerializeField]
    private GameObject instruction3 = default;

    [SerializeField]
    private GameObject instruction4 = default;

    public enum OnboardingState
    {
        Initiate,
        TitleScreenIn,
        CreditsIn,
        FadeFromBlack,
        TitleScreenOut,
        Instruction1,
        Instruction2,
        Instruction3,
        Instruction4,
        Finished,
    }
    public OnboardingState state;

    private float timer = 0;

    // main method
    public void Awake()
    {
        timer = 0;
        state = OnboardingState.Initiate;
        nextState();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    void nextState()
    {
        Debug.Log(state);
        switch (state)
        {
            // setup
            case OnboardingState.Initiate:
                state = OnboardingState.TitleScreenIn;
                this.overlay.GetComponent<Image>().color = Color.black;
                this.title.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0f);
                this.credits.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0f);
                this.instruction1.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0f);
                this.instruction2.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0f);
                this.instruction3.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0f);
                this.instruction4.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 0f);
                timer = 0;
                nextState();
                break;

            case OnboardingState.TitleScreenIn:
                state = OnboardingState.CreditsIn;
                LeanTween
                    .value(gameObject, 0f, 1f, .5f)
                    .setDelay(.5f)
                    .setOnUpdate(value => this.title.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value))
                    .setOnComplete(() => nextState());
                break;

            case OnboardingState.CreditsIn:
                state = OnboardingState.FadeFromBlack;
                LeanTween
                    .value(gameObject, 0f, 1f, .5f)
                    .setDelay(1f)
                    .setOnUpdate(value => this.credits.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value))
                    .setOnComplete(() => nextState());
                break;

            case OnboardingState.FadeFromBlack:
                state = OnboardingState.TitleScreenOut;
                LeanTween
                    .value(gameObject, 1f, 0f, 4f)
                    .setDelay(3f)
                    .setOnUpdate(value => this.overlay.GetComponent<Image>().color = new Color(0f, 0f, 0f, value))
                    .setOnComplete(() => nextState());
                break;

            case OnboardingState.TitleScreenOut:
                state = OnboardingState.Instruction1;
                LeanTween
                    .value(gameObject, 1f, 0f, 2f)
                    .setOnUpdate(value => this.title.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value))
                    .setOnComplete(() => nextState());
                LeanTween
                    .value(gameObject, 1f, 0f, 2f)
                    .setOnUpdate(value => this.credits.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value));
                break;

            case OnboardingState.Instruction1:
                state = OnboardingState.Instruction2;
                LeanTween
                    .value(gameObject, 0f, 1f, .5f)
                    .setDelay(4f)
                    .setOnUpdate(value => this.instruction1.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value));
                LeanTween
                    .value(gameObject, 1f, 0f, .5f)
                    .setDelay(10f)
                    .setOnUpdate(value => this.instruction1.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value))
                    .setOnComplete(() => nextState());
                break;

            case OnboardingState.Instruction2:
                state = OnboardingState.Instruction3;
                LeanTween
                    .value(gameObject, 0f, 1f, .5f)
                    .setDelay(4f)
                    .setOnUpdate(value => this.instruction2.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value));
                LeanTween
                    .value(gameObject, 1f, 0f, .5f)
                    .setDelay(10f)
                    .setOnUpdate(value => this.instruction2.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value))
                    .setOnComplete(() => nextState());
                break;

            case OnboardingState.Instruction3:
                state = OnboardingState.Instruction4;
                LeanTween
                    .value(gameObject, 0f, 1f, .5f)
                    .setDelay(4f)
                    .setOnUpdate(value => this.instruction3.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value));
                LeanTween
                    .value(gameObject, 1f, 0f, .5f)
                    .setDelay(10f)
                    .setOnUpdate(value => this.instruction3.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value))
                    .setOnComplete(() => nextState());
                break;

            case OnboardingState.Instruction4:
                state = OnboardingState.Finished;
                LeanTween
                    .value(gameObject, 0f, 1f, .5f)
                    .setDelay(4f)
                    .setOnUpdate(value => this.instruction4.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value));
                LeanTween
                    .value(gameObject, 1f, 0f, .5f)
                    .setDelay(10f)
                    .setOnUpdate(value => this.instruction4.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value))
                    .setOnComplete(() => nextState());
                break;

            default:
                gameObject.SetActive(false);
                break;
        }

    }
}
