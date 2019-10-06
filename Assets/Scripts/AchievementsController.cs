using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsController
{
    private static AchievementsController instance;
    public static AchievementsController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AchievementsController();
            }
            return instance;
        }
    }


    public AchievementsController()
    {
        SequenceDetector.NoteSequenceDetected += OnNoteSequenceDetected;
    }

    public enum Achievement
    {
        LetThereBeLight,
        WolfAttack,
        OffTheEdge,
        Lightning,
        FourSeasons,
    }

    public Dictionary<Achievement, string> achievements = new Dictionary<Achievement, string>() {
        { Achievement.LetThereBeLight, "Let there be light" },
        { Achievement.WolfAttack, "Wolf Attack" },
        { Achievement.OffTheEdge, "Fall off the edge" },
        { Achievement.Lightning, "Lightning strike" },
        { Achievement.FourSeasons, "Four seasons" },
    };

    public List<Achievement> triggeredAchievements = new List<Achievement>();

    public static event Action<Achievement> AchievementTriggered;

    public void TriggerAchivement(Achievement achievement)
    {
        triggeredAchievements.Add(achievement);
        AchievementTriggered?.Invoke(achievement);
    }

    public bool IsAchieved(Achievement achievement)
    {
        return triggeredAchievements.Contains(achievement);
    }

    public void OnNoteSequenceDetected(NoteSequence _)
    {
        TriggerAchivement(Achievement.LetThereBeLight);
    }
}
