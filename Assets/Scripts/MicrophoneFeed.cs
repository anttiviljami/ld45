using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MicrophoneFeed : MonoBehaviour
{
  private static MicrophoneFeed instance;
  public static  MicrophoneFeed Instance
  {
    get
    {
      if (instance == null)
      {
        var go = new GameObject("Microphone");
        instance = go.AddComponent<MicrophoneFeed>();
      }

      return instance;
    }
  }

  public static event Action ThresholdStart;
  public static event Action<MicrophoneOutput> OutputAnalyzed;

  public static event Action ThresholdEnd;

  public struct MicrophoneOutput
  {
    public float pitch;
    public float volume;
  }

  public float CurrentVolume { get; private set; }
  public float CurrentPitch { get; private set; }

  private AudioSource micAudioSource;

  AudioClip microphoneInput;

  void Awake()
  {
    // init microphone
    if (Microphone.devices.Length > 0)
      microphoneInput = Microphone.Start(Microphone.devices[0], true, 999, 44100);

      micAudioSource = gameObject.AddComponent<AudioSource>();
      micAudioSource.clip = microphoneInput;
  }

  /*
   * Gets mic level ranging 0-1
   *
   * Source: https://www.reddit.com/r/Unity3D/comments/49wuld/best_way_to_implement_microphone_input/
   */
  private float GetMicrophoneLevel(float time)
  {

    // get mic volume
    int dec = 128; // TODO factor in time
    float[] waveData = new float[dec];
    int micPosition = Microphone.GetPosition(null) - (dec + 1); // null means the first microphone
    microphoneInput.GetData(waveData, micPosition);
    micAudioSource.GetSpectrumData(waveData, 0, FFTWindow.BlackmanHarris);

    // TODO magic

    // get peak on the last 128 samples
    float levelMax = 0;
    for (int i = 0; i < dec; i++)
    {
      float wavePeak = waveData[i] * waveData[i];
      if (levelMax < wavePeak)
      {
        levelMax = wavePeak;
      }
    }
    float miclevel = Mathf.Sqrt(Mathf.Sqrt(levelMax));
    return miclevel;
  }
}
