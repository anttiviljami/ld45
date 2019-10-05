using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceDetector
{
    public static event Action<Note> NoteDetected;
    public static event Action<NoteSequence> NoteSequenceDetected;

    public SequenceDetector()
    {
        MicrophoneFeed.ThresholdStart += OnThresholdStart;
        MicrophoneFeed.ThresholdEnd += OnThresholdEnd;
        MicrophoneFeed.OutputAnalyzed += OnOutputAnalyzed;
    }

    ~SequenceDetector()
    {
        MicrophoneFeed.ThresholdStart -= OnThresholdStart;
        MicrophoneFeed.ThresholdEnd -= OnThresholdEnd;
        MicrophoneFeed.OutputAnalyzed -= OnOutputAnalyzed;
    }

    void OnThresholdStart()
    {
        Debug.Log("SequenceDetector: OnThresholdStart");
    }

    void OnThresholdEnd()
    {
        Debug.Log("SequenceDetector: OnThresholdEnd");
    }

    void OnOutputAnalyzed(MicrophoneFeed.MicrophoneOutput output)
    {
        Debug.Log(new { pitch = output.pitch, volume = output.volume });
    }
}
