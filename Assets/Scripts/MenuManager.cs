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

    private SequenceDetector sequenceDetector;

    public void Awake()
    {
        overlay.SetActive(false);
        GameManager.RunningStateChanged += OnRunningStateChanged;
        sequenceDetector = SequenceDetector.Instance;
        sensitivitySlider.GetComponent<Slider>().value = SequenceDetector.INITIAL_SENSITIVITY;
        SetMicSensitivity(SequenceDetector.INITIAL_SENSITIVITY);
    }

    public void OnRunningStateChanged(bool isRunning)
    {
        Debug.Log(isRunning);
        overlay.SetActive(!isRunning);
    }

    public void SetMicSensitivity(float sensitivity)
    {
        Debug.Log(new { sensitivity });
        sensitivityText.GetComponent<TextMeshProUGUI>().text = Mathf.Round(sensitivity * 100f) + " %";
        sequenceDetector.sensitivityValue = sensitivity;
    }
}
