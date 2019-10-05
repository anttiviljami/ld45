﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;

public class MicrophoneFeed : MonoBehaviour
{
    private const int SAMPLE_FREQUENCY = 44100;
    private const int PROCESS_SAMPLE_SIZE = 8192; // this is the maximum sample size supported for GetData
    private const float PROCESS_INTERVAL = 1f / 60f; // how often samples are processed

    private static MicrophoneFeed instance;
    public static MicrophoneFeed Instance
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

    private AudioSource microphoneSource;
    private AudioClip microphoneInput;

    void Awake()
    {
        // init microphone (use first microphone available)
        if (Microphone.devices.Length > 0)
            microphoneInput = Microphone.Start(Microphone.devices[0], true, 30, SAMPLE_FREQUENCY);

        microphoneSource = gameObject.AddComponent<AudioSource>();
        InvokeRepeating("ProcessMicrophoneData", PROCESS_INTERVAL, PROCESS_INTERVAL);
    }

    void ProcessMicrophoneData()
    {
        // gather samples from microphone
        float[] samples = new float[PROCESS_SAMPLE_SIZE];
        int micPosition = Microphone.GetPosition(null) - (PROCESS_SAMPLE_SIZE + 1);
        microphoneInput.GetData(samples, micPosition);

        // create a sample clip from the sample data
        AudioClip sample = AudioClip.Create("sample", samples.Count(), 1, SAMPLE_FREQUENCY, false);
        sample.SetData(samples, 0);
        sample.LoadAudioData();
        microphoneSource.clip = sample;
        microphoneSource.Play();

        // get average level from samples
        float sum = 0;
        for (int i = 0; i < PROCESS_SAMPLE_SIZE; i++)
            sum += samples[i] * samples[i]; // sum squared values

        float rms = Mathf.Sqrt(sum / PROCESS_SAMPLE_SIZE); // rms = square root of average
        float volume = Mathf.Sqrt(rms); // get volume between 0-1

        if (volume == 0)
        {
            // empty sample, do nothing
            return;
        }

        // calculate pitch
        float[] spectrum = new float[PROCESS_SAMPLE_SIZE];
        microphoneSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        float maxV = 0; // max amplitude
        int maxN = 0; // index with max volume
        for (int i = 0; i < PROCESS_SAMPLE_SIZE; i++)
        {
            if (spectrum[i] > maxV)
            {
                maxV = spectrum[i];
                maxN = i; // maxN is the index of max
            }
        }

        float freqN = (float)maxN;
        if (maxN > 0 && maxN < PROCESS_SAMPLE_SIZE - 1)
        {
            // interpolate index using neighbours
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += .5f * (dR * dR - dL * dL);
        }
        float pitch = freqN * (SAMPLE_FREQUENCY / 2) / PROCESS_SAMPLE_SIZE;

        // publish event with volume and pitch
        var output = new MicrophoneOutput();
        output.volume = volume;
        output.pitch = pitch;
        OutputAnalyzed(output);
    }
}
