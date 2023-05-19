using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : Singleton<GameEventSystem>, ISystem
{

    [SerializeField]
    private int _Priority;
    public int Priority { get => _Priority; }

    private HashSet<GameEvent> _hotEvents;
    public void Setup()
    {
        _hotEvents = new HashSet<GameEvent>();
        SystemCoordinator.Instance.FinishSystemSetup(this);
    }

    public void AddEventToList(GameEvent evt)
    {
        _hotEvents.Add(evt);
    }

    public void RemoveEventFromList(GameEvent evt)
    {
        _hotEvents.Remove(evt);
    }
}
