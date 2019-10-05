using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Note : IEqualityComparer<Note>
{
    public enum Name
    {
        Undefined, Animals, Plants, Earth, Weather, Building
    }

    [SerializeField]
    private Name name;
    public Name NoteName => name;

    public Note(Name name)
    {
        this.name = name;
    }

    public bool Equals(Note note1, Note note2)
    {
        return note1.NoteName == note2.NoteName;
    }

    public int GetHashCode(Note note)
    {
        int hCode = (int)note.NoteName;
        return hCode.GetHashCode();
    }
}
