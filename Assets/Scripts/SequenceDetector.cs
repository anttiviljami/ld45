﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceDetector
{
    public static event System.Action<float> RawVolumeEvent;

    public const int BPM = 80; // beats per minute
    public const float BEAT_INTERVAL = 60f / (float)BPM;
    public const int SEQUENCE_LENGTH = 3;

    public const float INITIAL_SENSITIVITY = .80f;
    public const float VOLUME_THRESHOLD_SAMPLES = 4;
    public const float MIN_NOTE_COUNT = 2;

    public float sensitivityValue = INITIAL_SENSITIVITY;
    public float volumeThreshold
    {
        get
        {
            return 1f - sensitivityValue;
        }
    }
    public bool IsOverVolumeThreshold = false;

    public static event Action ThresholdStart;
    public static event Action ThresholdEnd;
    public static event Action<Note> NoteDetected;
    public static event Action<NoteSequence> NoteSequenceDetected;

    protected List<float> volumeSequence = new List<float>();

    protected List<Note> currentSequence = new List<Note>();

    private List<MicrophoneFeed.MicrophoneOutput> outputs = new List<MicrophoneFeed.MicrophoneOutput>();

    private static SequenceDetector instance;
    public static SequenceDetector Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SequenceDetector();
            }
            return instance;
        }
    }

    public SequenceDetector()
    {
        MicrophoneFeed.OutputAnalyzed += OnOutputAnalyzed;
    }

    ~SequenceDetector()
    {
        MicrophoneFeed.OutputAnalyzed -= OnOutputAnalyzed;
    }

    void OnOutputAnalyzed(MicrophoneFeed.MicrophoneOutput output)
    {
        // track volume
        volumeSequence.Add(output.volume);
        if (volumeSequence.Count() > VOLUME_THRESHOLD_SAMPLES)
        {
            volumeSequence.RemoveAt(0);
        }
        // count average volume

        RawVolumeEvent?.Invoke(volumeSequence.Average());

        if (IsOverVolumeThreshold && volumeSequence.Average() < volumeThreshold)
        {
            // flip if volume sequence goes under
            IsOverVolumeThreshold = false;
            ThresholdEnd?.Invoke();
        }

        if (!IsOverVolumeThreshold && volumeSequence.Average() > volumeThreshold)
        {
            // flip if volume sequence goes under
            IsOverVolumeThreshold = true;
            ThresholdStart?.Invoke();
        }

        var detectedNote = mapOutputsToNote(outputs);

        // track output cycle if we are over volume threshold
        if (IsOverVolumeThreshold)
            outputs.Add(output);
        /*
        Debug.Log(new
        {
            noteName = detectedNote.NoteName,
            musical = pitchToMusicalNote(output.pitch),
            volume = output.volume,
            IsOverVolumeThreshold,
        });*/
    }

    public void Beat()
    {
        // do nothing if no outputs are recorded yet
        Note detectedNote;
        if (outputs.Count() != 0)
            detectedNote = mapOutputsToNote(outputs);
        else
            detectedNote = new Note(Note.Name.Undefined);

        // push event
        NoteDetected?.Invoke(detectedNote);

        // add to sequence
        pushAndMatchSequence(detectedNote);

        // clear outputs for this beat
        outputs.Clear();
    }

    private void pushAndMatchSequence(Note note)
    {
        // update sequence
        currentSequence.Add(note);
        while (currentSequence.Count() > SEQUENCE_LENGTH)
            currentSequence.RemoveAt(0);

        if (currentSequence.Count() < SEQUENCE_LENGTH)
            return; // do nothing if sequence is not long enough

        foreach (var n in currentSequence)
        {
            if (n.NoteName == Note.Name.Undefined)
                return; // do nothing if one of the notes is undefined
        }

        // sequence was matched
        var seq = new NoteSequence(currentSequence[0], currentSequence[1], currentSequence[2]);
        NoteSequenceDetected?.Invoke(seq);
        Debug.Log(seq);

        // clear the sequence
        currentSequence.Clear();
    }

    private string pitchToMusicalNote(float pitch)
    {
        if (pitch == 0) return "unknown"; // no pitch detected
        var notes = new[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        var noteNum = 12 * (Mathf.Log(pitch / 440) / Mathf.Log(2));
        var index = (Mathf.RoundToInt(noteNum) + 69) % 12;
        while (index < 0)
        {
            index += 12;
        }
        return notes[index];
    }

    private Note mapOutputsToNote(List<MicrophoneFeed.MicrophoneOutput> outputs, bool debug = false)
    {
        if (outputs.Count() == 0)
            return new Note(Note.Name.Undefined);

        // label each output with a note
        var mostCommonNote = outputs
            .Select((o) => pitchToMusicalNote(o.pitch))
            .GroupBy(s => s)
            .Where(g => g.Count() > MIN_NOTE_COUNT)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault();

        // map note to our special Note object
        switch (mostCommonNote)
        {
            case "C":
                return new Note(Note.Name.Animals);
            case "C#":
                return new Note(Note.Name.Plants);
            case "D":
                return new Note(Note.Name.Plants);
            case "D#":
                return new Note(Note.Name.Plants);
            case "E":
                return new Note(Note.Name.Plants);
            case "F":
                return new Note(Note.Name.Earth);
            case "F#":
                return new Note(Note.Name.Earth);
            case "G":
                return new Note(Note.Name.Earth);
            case "G#":
                return new Note(Note.Name.Weather);
            case "A":
                return new Note(Note.Name.Weather);
            case "A#":
                return new Note(Note.Name.Animals);
            case "B":
                return new Note(Note.Name.Animals);
            default:
                return new Note(Note.Name.Undefined);
        }
    }
}
