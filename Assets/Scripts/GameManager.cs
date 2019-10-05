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
                var go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
                Debug.Log("Created new GameManager");
            }

            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    private MicrophoneFeed microphoneFeed;

    private SequenceDetector sequenceDetector;

    void Awake()
    {
        microphoneFeed = gameObject.AddComponent<MicrophoneFeed>();
        sequenceDetector = new SequenceDetector();
    }
}
