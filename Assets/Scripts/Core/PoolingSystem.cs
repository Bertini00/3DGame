using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolManagerBinding
{
    //Model
    public string SceneName;
    public List<PoolManager> PoolManagerList;
}

public class PoolingSystem : Singleton<PoolingSystem>, ISystem
{

    [SerializeField]
    private int _Priority;

    [SerializeField]
    private PoolingSystemData _PoolingSystemData;

    public int Priority { get => _Priority; }

    // Dictionary<"sceneName", List<PoolManager>>
    private Dictionary<string, List<PoolManager>> _poolManagersDictionary;

    private Dictionary<string, PoolManager> _currentManagersDictionary;

    public void Setup()
    {
        // TODO POOLING SYSTEM SETUP
        _poolManagersDictionary = new Dictionary<string, List<PoolManager>>();
        _currentManagersDictionary = new Dictionary<string, PoolManager>();

        foreach (PoolManagerBinding binding in _PoolingSystemData.PoolManagerBindings)
        {
            // Duplicate the list (new) otherwise the scriptable object changes permanently
            _poolManagersDictionary.Add(binding.SceneName, new List<PoolManager>(binding.PoolManagerList));
        }

        SystemCoordinator.Instance.FinishSystemSetup(this);
    }

    public void SetupSceneManagers(string sceneName)
    {
        if (!_poolManagersDictionary.ContainsKey(sceneName)) return;

        // If there are some Poolmanagers, destroy them first, then setup
        if (_currentManagersDictionary.Count > 0) DestroyAllManagers();
        List<PoolManager> list = _poolManagersDictionary[sceneName];
        foreach (PoolManager manager in list)
        {
            PoolManager currentManager = Instantiate(manager, gameObject.transform);
            currentManager.Setup();
            _currentManagersDictionary.Add(currentManager.ID, currentManager);
        }
    }

    public void DestroyAllManagers()
    {
        foreach (string id in _currentManagersDictionary.Keys)
        {
            Destroy(_currentManagersDictionary[id].gameObject);
        }
        _currentManagersDictionary.Clear();
    }

    public PoolManager GetPoolManagerInstance(IdContainer idContainer)
    {

        if (!_currentManagersDictionary.ContainsKey(idContainer.Id))
        {
            Debug.Log("Error: Pooling manager not found with id " + idContainer.Id);
            return null;
        }

        return _currentManagersDictionary[idContainer.Id];
    }
}
