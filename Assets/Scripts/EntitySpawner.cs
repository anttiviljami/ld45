using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField]
    private Entity defaultPrefab = default;

    [SerializeField]
    private Transform entitiesRoot = default;

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
            var prefab = prefabLibrary[Random.Range(0, prefabLibrary.Count)];
            var entity = Instantiate(prefab.gameObject, prefab.JitteredPosition(WorldCursor.Instance.Cursor.position), Quaternion.identity);
            entity.transform.parent = entitiesRoot;
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
            var entity = Instantiate(prefab.gameObject, prefab.JitteredPosition(WorldCursor.Instance.Cursor.position), Quaternion.identity);
            entity.transform.parent = entitiesRoot;
        }
        else
        {
            var entity = Instantiate(defaultPrefab, defaultPrefab.JitteredPosition(WorldCursor.Instance.Cursor.position), Quaternion.identity);
            entity.transform.parent = entitiesRoot;
        }
    }
}
