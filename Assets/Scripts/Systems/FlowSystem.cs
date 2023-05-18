using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlowSystem : Singleton<FlowSystem>, ISystem
{

    [SerializeField]
    private int _Priority;

    [SerializeField]
    private string _OnStartGameplayEventName;

    public int Priority { get => _Priority; }


    [SerializeField]
    private StateMachine _FSM;

    public void Setup()
    {
        TravelSystem.Instance.TravelComplete += OnTravelComplete;
        SystemCoordinator.Instance.FinishSystemSetup(this);
    }

    public void TriggerFSMEvent(string eventName)
    {
        _FSM.TriggerUnityEvent(eventName);
    }

    public void SetFSMVariable(string variableName, object value)
    {
        Variables.Application.Set(variableName, value);
    }

    public T GetFSMVariable<T>(string variableName)
    {
        return Variables.Application.Get<T>(variableName);
    }

    private void OnTravelComplete()
    {
        if (GetFSMVariable<string>("SCENE_TO_LOAD") != "MainMenu")
        {
            TriggerFSMEvent(_OnStartGameplayEventName);
        }
    }

}
