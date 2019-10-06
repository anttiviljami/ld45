using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceView : MonoBehaviour
{
    [SerializeField]
    private Sprite animalIcon;

    [SerializeField]
    private Sprite earthIcon;

    [SerializeField]
    private Sprite weatherIcon;

    [SerializeField]
    private Sprite plantIcon;

    [SerializeField]
    private Sprite emptyIcon;

    [SerializeField]
    private Color mainColor;

    [SerializeField]
    private Color successColor;

    private List<GameObject> noteBlocks = new List<GameObject>();

    void Awake()
    {
        SequenceDetector.NoteDetected += NoteDetected;
        SequenceDetector.NoteSequenceDetected += NoteSequenceDetected;
    }

    void OnDestroy()
    {
        SequenceDetector.NoteDetected -= NoteDetected;
        SequenceDetector.NoteSequenceDetected -= NoteSequenceDetected;
    }

    void NoteDetected(Note note)
    {
        // create a note block
        GameObject block = new GameObject("NoteBlock");
        Image image = block.AddComponent<Image>(); //Add the Image Component script

        switch (note.NoteName)
        {
            case Note.Name.Animals:
                image.sprite = animalIcon;
                break;
            case Note.Name.Earth:
                image.sprite = earthIcon;
                break;
            case Note.Name.Plants:
                image.sprite = plantIcon;
                break;
            case Note.Name.Weather:
                image.sprite = weatherIcon;
                break;
            default:
                image.sprite = emptyIcon;
                break;
        }

        var cs = GetComponentInParent<Canvas>();

        // set to parent
        var btf = block.GetComponent<RectTransform>();
        btf.SetParent(transform);

        var tf = GetComponent<RectTransform>();
        block.transform.localPosition = new Vector2(150, -100);
        block.transform.localScale = Vector3.one;
        block.GetComponent<Image>().color = mainColor;
        block.SetActive(true);

        // keep track of block
        noteBlocks.Insert(0, block);

        // move all other blocks to the left
        for (int i = 1; i < noteBlocks.Count; i++)
        {
            var noteBlock = noteBlocks[i];
            noteBlock.transform.Translate(new Vector2(cs.scaleFactor * -150, 0));
            var img = noteBlock.GetComponent<Image>();
            var color = img.color;

            float[] blockAlphaValues = new float[] {
                1f,
                1f,
                1f,
                .5f,
                .25f,
                .066f,
            };

            if (i < blockAlphaValues.Length)
            {
                color.a = blockAlphaValues[i];
                img.color = color;
            }
            else
            {
                // destroy noteblocks
                noteBlocks.RemoveAt(i);
                Destroy(noteBlock);
            }
        }
    }

    void NoteSequenceDetected(NoteSequence _)
    {
        var noteImage1 = noteBlocks[0].GetComponent<Image>();
        var noteImage2 = noteBlocks[1].GetComponent<Image>();
        var noteImage3 = noteBlocks[2].GetComponent<Image>();
        var sequenceBlocks = noteBlocks.GetRange(0, 3);

        var scaleTween = LeanTween.value(gameObject, 1, 1, 0.45f);
        scaleTween.setEasePunch();
        scaleTween.setOnUpdate(value =>
        {
            noteImage1.transform.localScale = Vector3.one * value;
            noteImage2.transform.localScale = Vector3.one * value;
            noteImage3.transform.localScale = Vector3.one * value;
        });

        successColor.a = 1f;
        noteImage1.color = successColor;
        noteImage2.color = successColor;
        noteImage3.color = successColor;

        //var colorTween = LeanTween.value(gameObject, 0, 1, 0.25f);
        //colorTween.setEaseOutElastic();

        //colorTween.setOnUpdate(value =>
        //{
        //    noteImage1.color = Color.Lerp(mainColor, successColor, value);
        //    noteImage2.color = Color.Lerp(mainColor, successColor, value);
        //    noteImage3.color = Color.Lerp(mainColor, successColor, value);
        //});
    }
}
