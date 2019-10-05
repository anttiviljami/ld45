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

    void OnEnable()
    {
        SequenceDetector.NoteSequenceDetected += OnSequenceDetected;
    }

    void OnDisable()
    {
        SequenceDetector.NoteSequenceDetected -= OnSequenceDetected;
    }

    void Update()
    {
        // Test spawning
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnSequenceDetected(null);
        }
    }

    private void OnSequenceDetected(Note[] noteSequence)
    {
        // TODO match sequence with prefab

        var entity = Instantiate(defaultPrefab, cursor.Cursor.position + defaultPrefab.Jitter(), Quaternion.identity);
        entity.transform.parent = entitiesRoot;
    }
}
