using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField]
    private Entity defaultPrefab = default;

    [SerializeField]
    private Transform entitiesRoot = default;

    [SerializeField]
    private float impulseStrength = 1;

    private CinemachineImpulseSource impulseSource;

    private List<Entity> prefabLibrary = new List<Entity>();

    void Awake()
    {
        var initManager = EntityManager.Instance;
        impulseSource = GetComponent<CinemachineImpulseSource>();
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

#if UNITY_EDITOR
    void Update()
    {
        // Test spawning
        if (Input.GetKeyUp(KeyCode.Space))
        {
            var note1 = (Note.Name)(Random.Range(1, 6));
            var note2 = (Note.Name)(Random.Range(1, 6));
            var note3 = (Note.Name)(Random.Range(1, 6));

            OnSequenceDetected(new NoteSequence(note1, note2, note3));
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            var note1 = Note.Name.Earth;
            var note2 = Note.Name.Earth;
            var note3 = Note.Name.Earth;

            OnSequenceDetected(new NoteSequence(note1, note2, note3));
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            var note1 = Note.Name.Weather;
            var note2 = Note.Name.Earth;
            var note3 = Note.Name.Animals;

            OnSequenceDetected(new NoteSequence(note1, note2, note3));
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            var note1 = Note.Name.Weather;
            var note2 = Note.Name.Earth;
            var note3 = Note.Name.Earth;

            OnSequenceDetected(new NoteSequence(note1, note2, note3));
        }
    }
#endif

    private void ChangeSeason(Note.Name forNote)
    {
        switch (forNote)
        {
            case Note.Name.Animals:
                SeasonManager.Instance.SetSeason(Season.Summer);
                break;
            case Note.Name.Earth:
                SeasonManager.Instance.SetSeason(Season.Autumn);
                break;
            case Note.Name.Plants:
                SeasonManager.Instance.SetSeason(Season.Spring);
                break;
            case Note.Name.Weather:
                SeasonManager.Instance.SetSeason(Season.Winter);
                break;
        }

    }

    private void OnSequenceDetected(NoteSequence noteSequence)
    {
        if (noteSequence.note1.NoteName == Note.Name.Weather && noteSequence.note2.NoteName == Note.Name.Earth)
        {
            ChangeSeason(noteSequence.note3.NoteName);
            return;
        }

        List<Entity> prefabs = new List<Entity>();

        for (int i = 0; i < prefabLibrary.Count; i++)
        {
            if (prefabLibrary[i].recipe.Equals(noteSequence))
            {
                prefabs.Add(prefabLibrary[i]);
            }
        }

        if (prefabs.Count == 0)
        {
            // Loose matching to entities with last note of undefined
            for (int i = 0; i < prefabLibrary.Count; i++)
            {
                if (prefabLibrary[i].recipe.note3.NoteName == Note.Name.Undefined
                    && prefabLibrary[i].recipe.note1.NoteName == noteSequence.note1.NoteName
                    && prefabLibrary[i].recipe.note2.NoteName == noteSequence.note2.NoteName)
                {
                    prefabs.Add(prefabLibrary[i]);
                }
            }
        }

        if (prefabs.Count > 0)
        {
            var prefab = prefabs[Random.Range(0, prefabs.Count)];
            var entity = Instantiate(prefabs[0], prefab.JitteredPosition(WorldCursor.Instance.Cursor.position), Quaternion.identity);
            entity.transform.parent = entitiesRoot;
            entity.InitializeWithNoteSequence(noteSequence);

            impulseSource.GenerateImpulse(Vector3.one * impulseStrength);
            WorldCursor.Instance.Burst();
        }
        else
        {
            var entity = Instantiate(defaultPrefab, defaultPrefab.JitteredPosition(WorldCursor.Instance.Cursor.position), Quaternion.identity);
            entity.transform.parent = entitiesRoot;
            entity.InitializeWithNoteSequence(noteSequence);
        }
    }

    public void GetEntitiesInRange(NoteSequence sequence, float position, float range)
    {

    }
}
