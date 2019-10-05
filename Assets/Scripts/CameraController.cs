using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera targetCamera;

    [SerializeField]
    private GameObject drapPanel;

    void Start()
    {
        targetCamera.LookAt = WorldCursor.Instance.Cursor;
        targetCamera.Follow = WorldCursor.Instance.Cursor;

        var trigger = drapPanel.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;
        entry.callback.AddListener(OnDragBegin);
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener(OnDrag);
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener(OnDragEnd);
        trigger.triggers.Add(entry);
    }

    Vector2 dragStartPosition;

    void OnDragBegin(BaseEventData data)
    {
        dragStartPosition = ((PointerEventData)data).position;
    }

    void OnDrag(BaseEventData data)
    {
        var amount = ((PointerEventData)data).position - dragStartPosition;
        amount /= Screen.height;
        WorldCursor.Instance.Move(amount);
    }

    void OnDragEnd(BaseEventData data)
    {

    }
}
