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
        this.achievementText.GetComponent<TextMeshProUGUI>().color = Color.white;
        Invoke("FadeOut", 3f);
    }

    private void FadeOut()
    {
        LeanTween
            .value(achievementText, 1f, 0f, .5f)
            .setOnUpdate(value => this.achievementText.GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, value));
    }
}
