using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NoteSequence : IEqualityComparer<NoteSequence>
{
    public Note note1;
    public Note note2;
    public Note note3;

    public NoteSequence(Note.Name noteName1, Note.Name noteName2, Note.Name noteName3)
    {
        note1 = new Note(noteName1);
        note2 = new Note(noteName2);
        note3 = new Note(noteName3);
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
}
