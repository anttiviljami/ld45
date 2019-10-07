using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public enum Season
{
    Undefined,
    Summer,
    Spring,
    Autumn,
    Winter
}

public class SeasonManager : MonoBehaviour
{
    public static event System.Action<Season> SeasonChanged;

    private static SeasonManager instance;
    public static SeasonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SeasonManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Renderer groundRenderer = default;

    private Season CurrentSeason = default;

    [SerializeField]
    private PostProcessVolume postProcessingVolumeSpring = default;
    [SerializeField]
    private PostProcessVolume postProcessingVolumeSummer = default;
    [SerializeField]
    private PostProcessVolume postProcessingVolumeAutumn = default;
    [SerializeField]
    private PostProcessVolume postProcessingVolumeWinter = default;

    private PostProcessVolume currentPostProcessingVolume;

    private Dictionary<Season, Material> materials;

    [SerializeField]
    private Material materialSpring = default;
    [SerializeField]
    private Material materialSummer = default;
    [SerializeField]
    private Material materialAutumn = default;
    [SerializeField]
    private Material materialWinter = default;

    void Awake()
    {
        materials = new Dictionary<Season, Material>(){
            {Season.Summer, materialSummer},
            {Season.Autumn, materialAutumn},
            {Season.Spring, materialSpring},
            {Season.Winter, materialWinter}
        };

        SetSeason(Season.Spring);
    }

    public void SetSeason(Season newSeason)
    {
        if (CurrentSeason != newSeason)
        {
            var oldPost = PostProcessingVolumeForSeason(CurrentSeason);
            var newPost = PostProcessingVolumeForSeason(newSeason);

            CurrentSeason = newSeason;
            newPost.gameObject.SetActive(true);

            LeanTween
                .value(gameObject, 0, 1, 1.5f)
                .setOnUpdate(value =>
                {
                    if (oldPost != null) oldPost.weight = 1 - value;
                    newPost.weight = value;
                })
                .setOnComplete(() =>
                {
                    if (oldPost != null) oldPost.gameObject.SetActive(false);
                });

            groundRenderer.material = materials[newSeason];

            SeasonChanged?.Invoke(CurrentSeason);
        }
    }

    private PostProcessVolume PostProcessingVolumeForSeason(Season season)
    {
        switch (season)
        {
            case Season.Summer:
                return postProcessingVolumeSummer;
            case Season.Spring:
                return postProcessingVolumeSpring;
            case Season.Autumn:
                return postProcessingVolumeAutumn;
            case Season.Winter:
                return postProcessingVolumeWinter;
            default:
                return null;
        }
    }
}
