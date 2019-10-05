using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NoteSequence : IEqualityComparer<NoteSequence>
{
    public Note note1;
    public Note note2;
    public Note note3;

    public NoteSequence(Note _note1, Note _note2, Note _note3)
    {
        note1 = _note1;
        note2 = _note2;
        note3 = _note3;
    }

    public bool Equals(NoteSequence sequence1, NoteSequence sequence2)
    {
        return sequence1.note1.NoteName == sequence2.note1.NoteName
            && sequence1.note2.NoteName == sequence2.note2.NoteName
            && sequence1.note3.NoteName == sequence2.note3.NoteName;
    }

    public int GetHashCode(NoteSequence sequence)
    {
        int hCode = (int)sequence.note1.NoteName ^ (int)sequence.note2.NoteName ^ (int)sequence.note3.NoteName;
        return hCode.GetHashCode();
    }

    public override string ToString()
    {
        return "NoteSequence: " + note1.NoteName + ", " + note2.NoteName + ", " + note3.NoteName;
    }
}
