using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class PostProcessingController : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve distortionByVolume;

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
        var distortionAmount = distortionByVolume.Evaluate(output.volume);

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
