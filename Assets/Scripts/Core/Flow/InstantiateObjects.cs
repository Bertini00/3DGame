using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InstantiateObjects : Unit
{

    [DoNotSerialize]
    public ControlInput InputTrigger;

    [DoNotSerialize]
    public ControlOutput OutputTrigger;

    [DoNotSerialize]
    public ValueInput SceneName;

    protected override void Definition()
    {
        InputTrigger = ControlInput("", InternalBoot);
        OutputTrigger = ControlOutput("");
        SceneName = ValueInput<string>("Scene Name", "");
    }

    private ControlOutput InternalBoot(Flow arg)
    {
        Debug.Log("SceneName to instantiate: " + arg.GetValue<string>(SceneName));
        PoolingSystem.Instance.SetupSceneManagers(arg.GetValue<string>(SceneName));
        return OutputTrigger;
    }
}
