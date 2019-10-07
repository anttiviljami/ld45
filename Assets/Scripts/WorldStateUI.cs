using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldStateUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI seasonLabel;

    [SerializeField]
    private TextMeshProUGUI volumeLabel;

    LTDescr seasonAlphaTween;

    void OnEnable()
    {
        SeasonManager.SeasonChanged += SeasonManager_SeasonChanged;
        SequenceDetector.RawVolumeEvent += SequenceDetector_RawVolumeEvent;
    }

    private void SequenceDetector_RawVolumeEvent(float rawVolume)
    {
        volumeLabel.text = "Detected volume: " + Mathf.RoundToInt(rawVolume * 100);
    }

    void OnDisable()
    {
        SeasonManager.SeasonChanged -= SeasonManager_SeasonChanged;
        SequenceDetector.RawVolumeEvent -= SequenceDetector_RawVolumeEvent;
    }

    void SeasonManager_SeasonChanged(Season newSeason)
    {
        if (seasonAlphaTween != null)
        {
            LeanTween.cancel(seasonLabel.gameObject);
        }

        seasonLabel.text = newSeason.ToString().ToUpper();

        seasonLabel.alpha = 0;
        seasonAlphaTween = LeanTween
            .value(seasonLabel.gameObject, 0, 1, 0.8f)
            .setOnUpdate(value => seasonLabel.alpha = value)
            .setOnComplete(() =>
            {
                seasonAlphaTween = LeanTween
                    .value(seasonLabel.gameObject, 1, 0, 3)
                    .setOnUpdate(value => seasonLabel.alpha = value)
                    .setDelay(5);
            });
    }
}
