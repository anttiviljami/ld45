using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSequenceScaleByNoteNameCustomizer : NoteSequenceCustomizer
{
    [SerializeField]
    private float animal = 1;
    [SerializeField]
    private float earth = 1;
    [SerializeField]
    private float weather = 1;
    [SerializeField]
    private float building = 1;
    [SerializeField]
    private float plant = 1;

    protected override void Customize(NoteSequence notes)
    {
        switch (notes.note3.NoteName)
        {
            case Note.Name.Animals:
                transform.localScale = Vector3.one * animal;
                break;
            case Note.Name.Earth:
                transform.localScale = Vector3.one * earth;
                break;
            case Note.Name.Weather:
                transform.localScale = Vector3.one * weather;
                break;
            case Note.Name.Building:
                transform.localScale = Vector3.one * building;
                break;
            case Note.Name.Plants:
                transform.localScale = Vector3.one * plant;
                break;
        }
    }
}
