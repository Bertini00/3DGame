using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayInputProvider : InputProvider
{
    #region Delegate
    public OnFloatDelegate OnMoveRL;
    public OnFloatDelegate OnMoveUD;
    public OnVoidDelegate OnJump;
    public OnVoidDelegate OnRun;
    #endregion

    [Header("Gameplay")]
    [SerializeField]
    private InputActionReference _MoveRL;
    [SerializeField]
    private InputActionReference _MoveUD;

    [SerializeField]
    private InputActionReference _Jump;

    [SerializeField]
    private InputActionReference _Run;

    private void OnEnable()
    {
        _MoveRL.action.Enable();
        _MoveUD.action.Enable();
        _Jump.action.Enable();
        _Run.action.Enable();

        _MoveRL.action.performed += MovePerformedRL;
        _MoveUD.action.performed += MovePerformedUD;
        _Jump.action.performed += JumpPerformed;
        _Run.action.performed += RunPerformed;
    }

    private void OnDisable()
    {
        _MoveRL.action.Disable();
        _MoveUD.action.Disable();
        _Jump.action.Disable();
        _Run.action.Disable();

        _MoveRL.action.performed -= MovePerformedRL;
        _MoveUD.action.performed -= MovePerformedUD;
        _Jump.action.performed -= JumpPerformed;
        _Run.action.performed -= RunPerformed;
    }

    private void MovePerformedRL(InputAction.CallbackContext obj)
    {
        float value = obj.action.ReadValue<float>();
        OnMoveRL?.Invoke(value);
    }
    private void MovePerformedUD(InputAction.CallbackContext obj)
    {
        float value = obj.action.ReadValue<float>();
        OnMoveUD?.Invoke(value);
    }

    private void JumpPerformed(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke();
    }

    private void RunPerformed(InputAction.CallbackContext obj)
    {
        OnRun?.Invoke();
    }
}
