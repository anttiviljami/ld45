using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject overlay;

    [SerializeField]
    private GameObject sensitivitySlider;

    [SerializeField]
    private GameObject sensitivityText;

    [SerializeField]
    private GameObject muteButtonOn;

    [SerializeField]
    private GameObject muteButtonOff;

    private GameManager gameManager;
    private SequenceDetector sequenceDetector;

    public void Awake()
    {
        gameManager = GameManager.Instance;
        sequenceDetector = SequenceDetector.Instance;

        GameManager.RunningStateChanged += OnRunningStateChanged;
        GameManager.ToggleMute += UpdateMuteButtons;

        UpdateMuteButtons(gameManager.IsMuted);

        overlay.SetActive(false);

        sensitivitySlider.GetComponent<Slider>().value = SequenceDetector.INITIAL_SENSITIVITY;
        SetMicSensitivity(SequenceDetector.INITIAL_SENSITIVITY);
    }

    public void OnRunningStateChanged(bool isRunning)
    {
        Debug.Log(isRunning);
        if (overlay != null)
        {
            overlay.SetActive(!isRunning);
        }
    }

    public void SetMicSensitivity(float sensitivity)
    {
        Debug.Log(new { sensitivity });
        sensitivityText.GetComponent<TextMeshProUGUI>().text = Mathf.Round(sensitivity * 100f) + " %";
        sequenceDetector.sensitivityValue = sensitivity;
    }

    public void ToggleMute()
    {
        gameManager.IsMuted = !gameManager.IsMuted;
    }

    private void UpdateMuteButtons(bool isMuted)
    {
        this.muteButtonOn.SetActive(!isMuted);
        this.muteButtonOff.SetActive(isMuted);
    }
}
