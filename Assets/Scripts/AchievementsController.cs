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
        EntitySpawner.LightningStruck += OnLightningStruck;
    }

    public enum Achievement
    {
        LetThereBeLight,
        Zeus,
        ApexPredator,
        WolfAttack,
        OffTheEdge,
        Murder,
        Tornado,
        Vainamoinen,
        FourSeasons,
    }

    public Dictionary<Achievement, string> achievements = new Dictionary<Achievement, string>() {
        { Achievement.LetThereBeLight, "Let there be light" },
        { Achievement.Zeus, "Zeus" },
        { Achievement.ApexPredator, "Apex predator" },
        { Achievement.WolfAttack, "Wolf Attack" },
        { Achievement.Tornado, "Tornado season" },
        { Achievement.OffTheEdge, "Edge of the World" },
        { Achievement.Murder, "A murder most foul" },
        { Achievement.Vainamoinen, "Vainamoinen" },
        { Achievement.FourSeasons, "Four seasons" },
    };

    private bool springSummoned = false;
    private bool autumnSummoned = false;
    private bool winterSummoned = false;
    private bool summerSummoned = false;

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

        // check human
        if (noteSequence.note1.NoteName == Note.Name.Animals
        && noteSequence.note2.NoteName == Note.Name.Animals)
        {
            TriggerAchivement(Achievement.ApexPredator);
        }

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
        && (noteSequence.note3.NoteName == Note.Name.Plants || noteSequence.note3.NoteName == Note.Name.Earth))
        {
            TriggerAchivement(Achievement.Tornado);
        }

        // check seasons
        if (noteSequence.note1.NoteName == Note.Name.Weather
        && noteSequence.note2.NoteName == Note.Name.Earth)
        {
            switch (noteSequence.note3.NoteName)
            {
                case Note.Name.Animals:
                    // Summer
                    summerSummoned = true;
                    break;
                case Note.Name.Plants:
                    // Spring
                    springSummoned = true;
                    break;
                case Note.Name.Earth:
                    // Autumn
                    autumnSummoned = true;
                    break;
                case Note.Name.Weather:
                    // Winter
                    winterSummoned = true;
                    break;
            }
            if (springSummoned && summerSummoned && autumnSummoned && winterSummoned)
            {
                TriggerAchivement(Achievement.FourSeasons);
            }
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

    void OnLightningStruck(NoteSequence _)
    {
        TriggerAchivement(Achievement.Zeus);
    }
}
