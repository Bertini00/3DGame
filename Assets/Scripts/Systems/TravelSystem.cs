using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelSystem : Singleton<TravelSystem>
{

    public delegate void OnTravelCompleteDelegate();
    public OnTravelCompleteDelegate TravelComplete;

    [SerializeField]
    private string _InitialScene;
    [SerializeField]
    private string _LoadingScene;

    private string _currentScene;
    private string _targetScene;

    private void Start()
    {
        _currentScene = SceneManager.GetActiveScene().name;
        LoadScene(_InitialScene);
    }

    public void LoadScene(string path)
    {
        StartCoroutine(Load(path)); 
    }

    private IEnumerator Load(string path)
    {
        _targetScene = path;

        AsyncOperation op_loading = SceneManager.LoadSceneAsync(_LoadingScene, LoadSceneMode.Additive);
        yield return new WaitUntil(() => { return op_loading.isDone; });

         AsyncOperation op_current = SceneManager.UnloadSceneAsync(_currentScene);
        yield return new WaitUntil(() => { return op_current.isDone; });
        
        AsyncOperation op_target = SceneManager.LoadSceneAsync(_targetScene, LoadSceneMode.Additive);
        yield return new WaitUntil(() => { return op_target.isDone; });

        _currentScene = _targetScene;

        _targetScene = string.Empty;

        op_loading = SceneManager.UnloadSceneAsync(_LoadingScene);
        yield return new WaitUntil(() => { return op_current.isDone; });

        TravelComplete?.Invoke();
    }
}
