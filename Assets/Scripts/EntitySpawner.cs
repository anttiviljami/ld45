using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField]
    private Entity defaultPrefab;

    [SerializeField]
    private Transform entitiesRoot;

    [SerializeField]
    private WorldCursor cursor;

    private List<Entity> prefabLibrary = new List<Entity>();

    void Awake()
    {
        LoadPrefabs();
    }

    void OnEnable()
    {
        SequenceDetector.NoteSequenceDetected += OnSequenceDetected;
    }

    void OnDisable()
    {
        SequenceDetector.NoteSequenceDetected -= OnSequenceDetected;
    }

    void LoadPrefabs()
    {
        prefabLibrary.AddRange(Resources.LoadAll<Entity>("EntityPrefabs"));
    }

    void Update()
    {
        // Test spawning
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (Random.value < 0.5f)
            {
                OnSequenceDetected(new NoteSequence(Note.Name.Animals, Note.Name.Plants, Note.Name.Weather));
            }
            else
            {
                OnSequenceDetected(new NoteSequence(Note.Name.Animals, Note.Name.Plants, Note.Name.Earth));
            }
        }
    }

    private void OnSequenceDetected(NoteSequence noteSequence)
    {
        Entity prefab = null;

        for (int i = 0; i < prefabLibrary.Count; i++)
        {
            if (prefabLibrary[i].recipe.Equals(noteSequence))
            {
                prefab = prefabLibrary[i];
            }
        }

        if (prefab != null)
        {
            var entity = Instantiate(prefab.gameObject, cursor.Cursor.position + prefab.Jitter(), Quaternion.identity);
            entity.transform.parent = entitiesRoot;
        }
        else
        {
            var entity = Instantiate(defaultPrefab, cursor.Cursor.position + defaultPrefab.Jitter(), Quaternion.identity);
            entity.transform.parent = entitiesRoot;
        }
    }
}
