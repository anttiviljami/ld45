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

    private MicrophoneFeed microphoneFeed;

    private SequenceDetector sequenceDetector;

    void Awake()
    {
        Debug.Log("Game manager init");
        instance = this;
        microphoneFeed = gameObject.AddComponent<MicrophoneFeed>();
        sequenceDetector = new SequenceDetector();
    }
}
