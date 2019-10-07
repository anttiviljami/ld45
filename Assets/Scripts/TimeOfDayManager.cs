using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeOfDay
{
    Undefined,
    Morning,
    Day,
    Evening,
    Night
}

public class TimeOfDayManager : MonoBehaviour
{
    private static TimeOfDayManager instance;
    public static TimeOfDayManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TimeOfDayManager>();
            }

            return instance;
        }
    }

    private TimeOfDay currentTime;
    public TimeOfDay CurrentTime
    {
        get => currentTime;
        set
        {
            if (currentTime != value)
            {
                currentTime = value;
                var targetRotation = 0;
                switch (currentTime)
                {
                    case TimeOfDay.Day: targetRotation = 90; break;
                    case TimeOfDay.Evening: targetRotation = 180; break;
                    case TimeOfDay.Night: targetRotation = 90; break;
                }
                LeanTween
                    .rotateZ(timeOfDayDisplay, targetRotation, 1.5f)
                    .setEaseInOutBounce();
            }
        }
    }

    [SerializeField]
    private GameObject timeOfDayDisplay = default;

    void Awake()
    {
        CurrentTime = TimeOfDay.Morning;
    }
}
