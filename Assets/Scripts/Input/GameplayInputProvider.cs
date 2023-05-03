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
    public OnVoidDelegate OnRunCancelled;
    public OnVoidDelegate OnAbility;
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

    [SerializeField]
    private InputActionReference _Ability;
    private void OnEnable()
    {
        _MoveRL.action.Enable();
        _MoveUD.action.Enable();
        _Jump.action.Enable();
        _Run.action.Enable();
        _Ability.action.Enable();

        _MoveRL.action.performed += MovePerformedRL;
        _MoveUD.action.performed += MovePerformedUD;
        _Jump.action.performed += JumpPerformed;
        _Run.action.performed += RunPerformed;
        _Run.action.canceled += RunCancelled;

        _Ability.action.performed += AbilityPerformed;
    }

    private void OnDisable()
    {
        _MoveRL.action.Disable();
        _MoveUD.action.Disable();
        _Jump.action.Disable();
        _Run.action.Disable();
        _Ability.action.Disable();

        _MoveRL.action.performed -= MovePerformedRL;
        _MoveUD.action.performed -= MovePerformedUD;
        _Jump.action.performed -= JumpPerformed;
        _Run.action.performed -= RunPerformed;
        _Run.action.canceled -= RunCancelled;

        _Ability.action.performed -= AbilityPerformed;
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

    private void RunCancelled(InputAction.CallbackContext obj)
    {
        OnRunCancelled?.Invoke();
    }

    private void AbilityPerformed(InputAction.CallbackContext obj)
    {
        OnAbility?.Invoke();
    }
}
