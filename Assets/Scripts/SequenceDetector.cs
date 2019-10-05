using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceDetector
{
    public const int BPM = 80; // beats per minute
    public const float BEAT_INTERVAL = 60f / (float)BPM;
    public const int SEQUENCE_LENGTH = 3;

    public static event Action<Note> NoteDetected;
    public static event Action<NoteSequence> NoteSequenceDetected;
    public string[] notes;

    protected List<Note> currentSequence = new List<Note>();

    private List<MicrophoneFeed.MicrophoneOutput> outputs = new List<MicrophoneFeed.MicrophoneOutput>();

    public SequenceDetector()
    {
        notes = new[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

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
        outputs.Add(output);
    }

    public void Beat()
    {
        // do nothing if no outputs are recorded yet
        if (outputs.Count() == 0)
            return;

        // label each output with a note
        var mostCommonNote = outputs.Select((o) =>
            {
                var musicalNote = pitchToMusicalNote(o.pitch);
                var note = musicalNote != null ? notes[(int)musicalNote % 12] : "unknown";
                return note;
            })
            .GroupBy(s => s)
            .Where(g => g.Count() > 1)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .First();

        // map note to our special Note object
        Note detectedNote;
        switch (mostCommonNote)
        {
            case "C":
                detectedNote = new Note(Note.Name.Animals);
                break;
            case "C#":
                detectedNote = new Note(Note.Name.Animals);
                break;
            case "D":
                detectedNote = new Note(Note.Name.Plants);
                break;
            case "D#":
                detectedNote = new Note(Note.Name.Plants);
                break;
            case "E":
                detectedNote = new Note(Note.Name.Earth);
                break;
            case "F":
                detectedNote = new Note(Note.Name.Earth);
                break;
            case "F#":
                detectedNote = new Note(Note.Name.Earth);
                break;
            case "G":
                detectedNote = new Note(Note.Name.Weather);
                break;
            case "G#":
                detectedNote = new Note(Note.Name.Weather);
                break;
            case "A":
                detectedNote = new Note(Note.Name.Weather);
                break;
            case "A#":
                detectedNote = new Note(Note.Name.Weather);
                break;
            case "B":
                detectedNote = new Note(Note.Name.Animals);
                break;
            default:
                detectedNote = new Note(Note.Name.Undefined);
                break;
        }

        NoteDetected?.Invoke(detectedNote);
        // Debug.Log(new { mostCommonNote, detectedNote.NoteName, length = outputs.Count() });

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

        var seq = new NoteSequence(currentSequence[0], currentSequence[1], currentSequence[2]);
        NoteSequenceDetected?.Invoke(seq);
        Debug.Log(seq);
    }

    private Nullable<int> pitchToMusicalNote(float pitch)
    {
        if (pitch == 0) // no pitch was detected
            return null;

        var noteNum = 12 * (Mathf.Log(pitch / 440) / Mathf.Log(2));
        return Mathf.RoundToInt(noteNum) + 69;
    }
}
