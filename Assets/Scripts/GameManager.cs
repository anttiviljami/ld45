using System;
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
                instance = UnityEngine.Object.FindObjectOfType<GameManager>();
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

    public static event Action<bool> RunningStateChanged;

    [SerializeField]
    private AudioClip tickSound;

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

        // send running state change
        RunningStateChanged?.Invoke(true);
    }

    void Beat()
    {
        sequenceDetector.Beat(); // triggers beat
        var audio = GetComponent<AudioSource>();
        audio.PlayOneShot(tickSound);
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
        RunningStateChanged?.Invoke(IsRunning);
        Time.timeScale = IsRunning ? 1 : 0;
    }
}
