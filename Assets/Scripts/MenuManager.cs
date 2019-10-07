using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject overlay = default;

    [SerializeField]
    private GameObject MenuScreen = default;

    [SerializeField]
    private GameObject achievementsList = default;

    [SerializeField]
    private GameObject achievementNames = default;
    [SerializeField]
    private GameObject achievementTicks = default;

    [SerializeField]
    private GameObject sensitivitySlider = default;

    [SerializeField]
    private GameObject sensitivityText = default;

    [SerializeField]
    private GameObject muteButtonOn = default;

    [SerializeField]
    private GameObject muteButtonOff = default;

    private GameManager gameManager;
    private SequenceDetector sequenceDetector;
    private AchievementsController achievementsController;

    private bool achievementsOpen = false;

    public void Awake()
    {
        gameManager = GameManager.Instance;
        sequenceDetector = SequenceDetector.Instance;
        achievementsController = AchievementsController.Instance;

        GameManager.RunningStateChanged += OnRunningStateChanged;
        GameManager.ToggleMute += UpdateMuteButtons;

        SetAchievementsOpen(false);
        UpdateMuteButtons(gameManager.IsMuted);

        overlay.SetActive(false);

        sensitivitySlider.GetComponent<Slider>().value = SequenceDetector.INITIAL_SENSITIVITY;
        SetMicSensitivity(SequenceDetector.INITIAL_SENSITIVITY);

    }

    public void OnRunningStateChanged(bool isRunning)
    {
        if (overlay != null) overlay.SetActive(!isRunning);
        SetAchievementsOpen(false);
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

    public void SetAchievementsOpen(bool open)
    {
        if (MenuScreen && achievementsList)
        {
            achievementsOpen = open;
            this.MenuScreen.SetActive(!achievementsOpen);
            this.achievementsList.SetActive(achievementsOpen);
            UpdateAchievementsText();
        }
    }

    public void UpdateAchievementsText()
    {
        string achievementNamesText = "";
        string achievementTicksText = "";
        foreach (var a in achievementsController.achievements)
        {
            achievementNamesText += a.Value + "\n";
            achievementTicksText += achievementsController.IsAchieved(a.Key) ? "X\n" : "?\n";
        }
        this.achievementNames.GetComponent<TextMeshProUGUI>().text = achievementNamesText;
        this.achievementTicks.GetComponent<TextMeshProUGUI>().text = achievementTicksText;
    }

    private void UpdateMuteButtons(bool isMuted)
    {
        this.muteButtonOn.SetActive(!isMuted);
        this.muteButtonOff.SetActive(isMuted);
    }
}
