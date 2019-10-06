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

    private List<GameObject> noteBlocks = new List<GameObject>();

    void Awake()
    {
        SequenceDetector.NoteDetected += NoteDetected;
    }

    void OnDestroy()
    {
        SequenceDetector.NoteDetected -= NoteDetected;
    }

    void Update()
    {
        // noteBlocks()
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

        // set to parent
        block.GetComponent<RectTransform>().SetParent(transform);

        var tf = GetComponent<RectTransform>();
        block.transform.localPosition = new Vector2(75, -tf.rect.height / 2);
        block.SetActive(true);

        // keep track of block
        noteBlocks.Insert(0, block);

        // move all other blocks to the left
        for (int i = 1; i < noteBlocks.Count; i++)
        {
            var noteBlock = noteBlocks[i];
            noteBlock.transform.Translate(new Vector2(-150, 0));
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
}
