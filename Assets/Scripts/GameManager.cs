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

    private AudioSource tickSound;

    void Awake()
    {
        instance = this;
        microphoneFeed = gameObject.AddComponent<MicrophoneFeed>();
        tickSound = GetComponent<AudioSource>();
        sequenceDetector = new SequenceDetector();
        InvokeRepeating("Beat", SequenceDetector.BEAT_INTERVAL, SequenceDetector.BEAT_INTERVAL);
    }

    void Beat()
    {
        sequenceDetector.Beat(); // triggers beat
        tickSound.PlayOneShot(tickSound.clip);
    }
}
