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

    public const float MUSIC_VOLUME = .05f;
    public const float SFX_VOLUME = .1f;
    public static event Action<bool> RunningStateChanged;
    public static event Action<bool> ToggleMute;
    public static event Action GameReset;

    [SerializeField]
    private AudioClip tickSound = default;

    [SerializeField]
    private AudioClip music;

    [SerializeField]
    private AudioClip themeJingle;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private MicrophoneFeed microphoneFeed;

    private SequenceDetector sequenceDetector;

    [SerializeField]
    private GameObject onboardingSequence = default;

    private bool isRunning = false;
    public bool IsRunning
    {
        get
        {
            return this.isRunning;
        }
        set
        {
            this.isRunning = value;
            RunningStateChanged?.Invoke(value);
            Time.timeScale = IsRunning ? 1 : 0;
        }
    } // pause / unpause game

    private bool isMuted = false;
    public bool IsMuted
    {
        get
        {
            return this.isMuted;
        }
        set
        {
            this.isMuted = value;
            ToggleMute?.Invoke(value);
            Time.timeScale = isMuted ? 1 : 0;
        }
    } // pause / unpause game

    //private bool IsRecording = true; // stop recording

    void Awake()
    {
        instance = this;

        ToggleMute += OnToggleMute;

        microphoneFeed = gameObject.AddComponent<MicrophoneFeed>();
        sequenceDetector = SequenceDetector.Instance;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.volume = SFX_VOLUME;
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.volume = MUSIC_VOLUME;

        // start music
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();

        // send running state change
        this.IsRunning = true;

        Onboard();
        Invoke("StartGameLoop", 5f);
    }

    public void Onboard()
    {
        // start onboarding
        Instantiate(onboardingSequence, Vector3.zero, Quaternion.identity);
        onboardingSequence.transform.localScale = Vector3.one;
        onboardingSequence.SetActive(true);
    }

    public void StartGameLoop()
    {
        microphoneFeed.IsRecording = true; // start recording
        InvokeRepeating("Beat", SequenceDetector.BEAT_INTERVAL, SequenceDetector.BEAT_INTERVAL);
        if (!IsMuted)
            sfxSource.PlayOneShot(themeJingle);
    }

    void Beat()
    {
        sequenceDetector.Beat(); // triggers beat
        if (!IsMuted)
            sfxSource.PlayOneShot(tickSound, .25f);
    }

    private void Update()
    {
        // Esc maps to back button on Android
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        this.IsRunning = !IsRunning;
    }

    public void ResetGame()
    {
        this.IsRunning = false;
        GameReset?.Invoke();
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
        this.IsRunning = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnToggleMute(bool mute)
    {
        if (mute)
            musicSource.Stop();
        else
            musicSource.Play();
    }
}
