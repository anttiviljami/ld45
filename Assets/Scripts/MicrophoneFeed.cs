using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MicrophoneFeed : MonoBehaviour
{
  RawImage image;
  AudioClip microphoneInput;
  float _timer = 0;
  float _alpha = .5f * 255;

  void Awake()
  {
    image = gameObject.GetComponent<RawImage>();

    // init microphone
    if (Microphone.devices.Length > 0)
      microphoneInput = Microphone.Start(Microphone.devices[0], true, 999, 44100);
  }

  void Update()
  {
    // increment timer
    _timer += Time.deltaTime;

    // cycle orb colour
    var colors = new List<Color>();

    colors.Add(new Color(255, 0, 0));
    colors.Add(new Color(255, 255, 0));
    colors.Add(new Color(0, 255, 0));
    colors.Add(new Color(0, 255, 255));
    colors.Add(new Color(0, 0, 255));
    colors.Add(new Color(255, 0, 255));

    float cycleSpeed = 2;

    int curr = (int)Math.Floor(cycleSpeed * _timer) % colors.Count;
    int next = curr + 1 < colors.Count ? curr + 1 : 0;
    float phase = cycleSpeed * _timer - Mathf.Floor(cycleSpeed * _timer);

    Debug.Log(new { curr, next, phase });

    image.color = Color.Lerp(colors[curr], colors[next], phase);

    // resize the orb according to mic level
    float level = GetMicrophoneLevel();
    float size = .33f + level * .66f;
    transform.localScale = new Vector3(size, size, size);
  }

  /*
   * Gets mic level ranging 0-1
   *
   * Source: https://www.reddit.com/r/Unity3D/comments/49wuld/best_way_to_implement_microphone_input/
   */
  private float GetMicrophoneLevel()
  {
    // get mic volume
    int dec = 128;
    float[] waveData = new float[dec];
    int micPosition = Microphone.GetPosition(null) - (dec + 1); // null means the first microphone
    microphoneInput.GetData(waveData, micPosition);

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
