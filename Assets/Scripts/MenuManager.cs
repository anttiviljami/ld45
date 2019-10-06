using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject overlay;

    public void Awake()
    {
        overlay.SetActive(false);
        GameManager.RunningStateChanged += OnRunningStateChanged;
    }

    public void OnRunningStateChanged(bool isRunning)
    {
        Debug.Log(isRunning);
        overlay.SetActive(!isRunning);
    }
}
