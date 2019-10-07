using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class PostProcessingController : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve distortionByVolume = default;

    private PostProcessVolume volume;

    private LensDistortion distortion;

    void OnEnable()
    {
        volume = GetComponent<PostProcessVolume>();
        MicrophoneFeed.OutputAnalyzed += MicrophoneFeed_OutputAnalyzed;
    }

    void OnDisable()
    {
        MicrophoneFeed.OutputAnalyzed -= MicrophoneFeed_OutputAnalyzed;
    }

    void MicrophoneFeed_OutputAnalyzed(MicrophoneFeed.MicrophoneOutput output)
    {
        var sensitivity = SequenceDetector.Instance.sensitivityValue;
        var range = 1 - Mathf.Clamp01(sensitivity) * 0.5f;
        var volumePercentage = output.volume / range;

        var distortionAmount = distortionByVolume.Evaluate(volumePercentage);

        if (volume.profile.TryGetSettings(out distortion))
        {
            if (distortionAmount > 0)
            {
                distortion.enabled.value = true;
                distortion.intensity.value = -distortionAmount;
            }
            else
            {
                distortion.enabled.value = false;
            }
        }
    }
}
