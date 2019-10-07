using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private static EntityManager instance;
    public static EntityManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EntityManager>();
            }
            if (instance == null)
            {
                var go = new GameObject("EntityManager");
                instance = go.AddComponent<EntityManager>();
            }

            return instance;
        }
    }

    private List<Entity> entities = new List<Entity>();

    void OnEnable()
    {
        Entity.EntitySpawned += EntitySpawner_EntitySpawned;
        Entity.EntityRemoved += EntitySpawner_EntityRemoved;
    }

    void OnDisable()
    {
        Entity.EntitySpawned -= EntitySpawner_EntitySpawned;
        Entity.EntityRemoved -= EntitySpawner_EntityRemoved;
    }

    void EntitySpawner_EntitySpawned(Entity newEntity)
    {
        entities.Add(newEntity);
    }

    void EntitySpawner_EntityRemoved(Entity removedEntity)
    {
        entities.Remove(removedEntity);
    }

    public Entity GetClosestInRange(Vector3 position, float range, NoteSequence self)
    {
        var rangeSquared = range * range;
        float closestDistance = float.MaxValue;
        Entity closest = null;

        for (int i = 0; i < entities.Count; i++)
        {
            if (!entities[i].recipe.Equals(self))
            {
                var dist = Vector3.SqrMagnitude(position - entities[i].Position);
                if (dist <= rangeSquared && dist < closestDistance)
                {
                    closest = entities[i];
                    closestDistance = dist;
                }
            }
        }

        return closest;
    }

    public Entity GetClosestInWithNotes(Vector3 position, float range, Note.Name note1, Note.Name note2, NoteSequence self)
    {
        var rangeSquared = range * range;
        float closestDistance = float.MaxValue;
        Entity closest = null;

        for (int i = 0; i < entities.Count; i++)
        {
            if (!entities[i].recipe.Equals(self)
                && ((entities[i].recipe.note1.NoteName & note1) > 0)
                && ((entities[i].recipe.note2.NoteName & note2) > 0))
            {
                var dist = Vector3.SqrMagnitude(position - entities[i].Position);
                if (dist <= rangeSquared && dist < closestDistance)
                {
                    closest = entities[i];
                    closestDistance = dist;
                }
            }
        }

        return closest;
    }

}
