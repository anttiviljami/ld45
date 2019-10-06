using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindObjectOfType<GameManager>();
            }
            if (instance == null)
            {
                Debug.Log("Init");
                var go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
            }

            return instance;
        }
    }

    private MicrophoneFeed microphoneFeed;

    private SequenceDetector sequenceDetector;

    private bool IsRunning = true; // pause / unpause
    private bool IsRecording = true; // stop recording

    void Awake()
    {
        instance = this;
        microphoneFeed = gameObject.AddComponent<MicrophoneFeed>();
        microphoneFeed.IsRecording = true; // start recording
        sequenceDetector = new SequenceDetector();
        InvokeRepeating("Beat", SequenceDetector.BEAT_INTERVAL, SequenceDetector.BEAT_INTERVAL);
    }

    void Beat()
    {
        sequenceDetector.Beat(); // triggers beat
    }

    private void Update()
    {
        // Esc maps to back button on Android
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // TODO: Open Menu
            Application.Quit();
        }
    }

    public void ToggleMenu()
    {
        Debug.Log("TOGGLE MENU");
        IsRunning = !IsRunning;
        if (!IsRunning)
        {
            microphoneFeed.IsRecording = false;
        }
    }
}
