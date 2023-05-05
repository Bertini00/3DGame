using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SystemCoordinator : Singleton<SystemCoordinator>
{
    [SerializeField]
    private string _OnAllSystemReadyFSMName;


    private List<ISystem> _systems;
    private Dictionary<ISystem, bool> _dictionarySystem;
    private int _countSystemReady;


    private void Start()
    {
        StartSystemSetup();
    }
    public void StartSystemSetup()
    {

        _systems = new List<ISystem>();

        List<GameObject> check = FindObjectsOfType<GameObject>().ToList();
        foreach (GameObject go in check)
        {
            ISystem i = go.GetComponent<ISystem>();
            if (i != null)
            {
                _systems.Add(i);
                
            }
        }
        _dictionarySystem = new Dictionary<ISystem, bool>();
        _systems = _systems.OrderByDescending(P => P.Priority).ToList();
        foreach (ISystem sys in _systems)
        {
            _dictionarySystem.Add(sys, false);
            sys.Setup();
            
        }
    }

    public void FinishSystemSetup(ISystem sys)
    {
        if (!_dictionarySystem.ContainsKey(sys))
        {
            Debug.LogError("FUCK");
            return;
        }
        _dictionarySystem[sys] = true;
        ++_countSystemReady;

        CheckAllSystemReady();
    }
    private void CheckAllSystemReady()
    {
        if (_countSystemReady == _systems.Count())
        {
            FlowSystem.Instance.TriggerFSMEvent(_OnAllSystemReadyFSMName);
        }
    }

}
