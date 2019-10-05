using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class NoteSequenceColorCustomizer : NoteSequenceCustomizer
{
    private SpriteRenderer targetSprite;

    [SerializeField]
    private Color animal = Color.white;
    [SerializeField]
    private Color earth = Color.white;
    [SerializeField]
    private Color weather = Color.white;
    [SerializeField]
    private Color building = Color.white;
    [SerializeField]
    private Color plant = Color.white;

    protected override void Customize(NoteSequence notes)
    {
        if (!targetSprite)
        {
            targetSprite = GetComponent<SpriteRenderer>();
        }
        
        switch (notes.note3.NoteName)
        {
            case Note.Name.Animals:
                targetSprite.color = animal;
                break;
            case Note.Name.Earth:
                targetSprite.color = earth;
                break;
            case Note.Name.Weather:
                targetSprite.color = weather;
                break;
            case Note.Name.Building:
                targetSprite.color = building;
                break;
            case Note.Name.Plants:
                targetSprite.color = plant;
                break;
        }
    }
}
