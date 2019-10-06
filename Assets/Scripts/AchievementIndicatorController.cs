using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementIndicatorController : MonoBehaviour
{
    [SerializeField]
    private GameObject achievementText;

    private AchievementsController achievementsController;

    void Awake()
    {
        achievementsController = AchievementsController.Instance;
        AchievementsController.AchievementTriggered += IndicateAchievement;
        achievementText.GetComponent<TextMeshProUGUI>().text = "";
    }

    void IndicateAchievement(AchievementsController.Achievement achievement)
    {
        achievementText.GetComponent<TextMeshProUGUI>().text = "Achievement unlocked!\n \"" + achievementsController.achievements[achievement] + "\"";
        LeanTween
            .value(gameObject, 1f, 0f, .5f)
            .setDelay(3f)
            .setOnUpdate(value => this.achievementText.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value))
            .setDestroyOnComplete(false);
    }
}
