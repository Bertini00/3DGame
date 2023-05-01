using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuViewController : MonoBehaviour
{

    [SerializeField]
    private OptionViewController _OptionsViewPrefab;

    private OptionViewController _optionsViewController;
    public void ChangeScene(string sceneName)
    {
        TravelSystem.Instance.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        if (_optionsViewController)
        {
            return;
        }
        _optionsViewController = Instantiate(_OptionsViewPrefab);
    }
}
