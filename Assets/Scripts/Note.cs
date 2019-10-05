using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Note
{
    public enum Name
    {
        Animals, Plants, Earth, Weather, Building
    }

    public readonly Name name;

    public Note(Name name)
    {
        this.name = name;
    }
}
