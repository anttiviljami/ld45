using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject overlay;

    public void Awake()
    {
        GameManager.RunningStateChanged += OnRunningStateChanged;
    }

    public void OnRunningStateChanged(bool IsRunning)
    {
        overlay.SetActive(!IsRunning);
    }
}
