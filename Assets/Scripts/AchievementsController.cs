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
        HostileBehaviour.EntityKilled += OnEntityKilled;
    }

    public enum Achievement
    {
        LetThereBeLight,
        WolfAttack,
        OffTheEdge,
        Murder,
        Tornado,
        Vainamoinen,
        FourSeasons,
    }

    public Dictionary<Achievement, string> achievements = new Dictionary<Achievement, string>() {
        { Achievement.LetThereBeLight, "Let there be light" },
        { Achievement.WolfAttack, "Wolf Attack" },
        { Achievement.Tornado, "Tornado season" },
        { Achievement.OffTheEdge, "Fall off the edge" },
        { Achievement.Murder, "A murder most foul" },
        { Achievement.Vainamoinen, "Vainamoinen" },
        { Achievement.FourSeasons, "Four seasons" },
    };

    public List<Achievement> triggeredAchievements = new List<Achievement>();

    public static event Action<Achievement> AchievementTriggered;

    public void TriggerAchivement(Achievement achievement)
    {
        if (!IsAchieved(achievement))
        {
            triggeredAchievements.Add(achievement);
            AchievementTriggered?.Invoke(achievement);
        }
    }

    public bool IsAchieved(Achievement achievement)
    {
        return triggeredAchievements.Contains(achievement);
    }

    public void OnNoteSequenceDetected(NoteSequence noteSequence)
    {
        TriggerAchivement(Achievement.LetThereBeLight);

        // check swamp
        if (noteSequence.note1.NoteName == Note.Name.Earth
        && noteSequence.note2.NoteName == Note.Name.Weather
        && noteSequence.note3.NoteName == Note.Name.Weather)
        {
            TriggerAchivement(Achievement.Vainamoinen);
        }

        // check tornado
        if (noteSequence.note1.NoteName == Note.Name.Weather
        && noteSequence.note2.NoteName == Note.Name.Weather
        && noteSequence.note3.NoteName == Note.Name.Weather)
        {
            TriggerAchivement(Achievement.Tornado);
        }
    }

    public void OnEntityKilled(KillEventArgs args)
    {
        // check wolf
        if (args.killer.recipe.note1.NoteName == Note.Name.Animals
        && args.killer.recipe.note2.NoteName == Note.Name.Earth
        && args.killer.recipe.note3.NoteName == Note.Name.Weather
        )
        {
            TriggerAchivement(Achievement.WolfAttack);
        }

        // check barbarian
        if (args.killer.recipe.note1.NoteName == Note.Name.Animals
        && args.killer.recipe.note2.NoteName == Note.Name.Animals
        && args.killer.recipe.note3.NoteName == Note.Name.Weather
        // check victim is human
        && args.victim.recipe.note1.NoteName == Note.Name.Animals
        && args.victim.recipe.note2.NoteName == Note.Name.Animals
        )
        {
            TriggerAchivement(Achievement.Murder);
        }
    }
}
