using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEventInvoker : MonoBehaviour
{

    [SerializeField]
    private IdContainerGameEvent _DebugEvent;

    [SerializeField]
    private IdContainer _DebugIdContainer;

    private void Awake()
    {
        _DebugIdContainer.Id = "Cosa diversa";
    }

    [ContextMenu("Invoke")]
    public void InvokeEvent()
    {
        _DebugEvent.IdContainer = _DebugIdContainer;
        _DebugEvent.Invoke();
    }
}
