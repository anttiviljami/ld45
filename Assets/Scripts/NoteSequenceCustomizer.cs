using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoteSequenceCustomizer : MonoBehaviour
{
    void Start()
    {
        var parentEntity = GetComponentInParent<Entity>();
        Customize(parentEntity.NoteSequence);
    }

    protected abstract void Customize(NoteSequence notes);
}
