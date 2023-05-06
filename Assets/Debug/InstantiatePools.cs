using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePools : MonoBehaviour
{
    [SerializeField]
    private string _SceneName;
    [ContextMenu("Pooling")]

    public void DebugInstantiatePools()
    {
        PoolingSystem.Instance.SetupSceneManagers(_SceneName);
    }

}
