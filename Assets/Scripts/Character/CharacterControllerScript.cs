using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{

    private const float MOVEMENT_VALUE = 0.01f;
    private const float ROTATIONFACTOR = 10f;

    [SerializeField]
    private IdContainer _IdProvider;


    private GameplayInputProvider _gameplayInputProvider;

    private float _movementRL;
    private float _movementUD;

    private bool _isRunning = false;

    private Animator animator;

    private CharacterController _characterController;

    private Vector3 _currentMovement;

    private void Awake()
    {
        _gameplayInputProvider = PlayerController.Instance.GetInput<GameplayInputProvider>(_IdProvider.Id);
        _characterController = GetComponent<CharacterController>();

        _currentMovement = Vector3.zero;

        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }

    private void OnEnable()
    {
        _gameplayInputProvider.OnMoveRL += MoveCharacterRL;
        _gameplayInputProvider.OnMoveUD += MoveCharacterUD;
        _gameplayInputProvider.OnJump += JumpCharacter;
        _gameplayInputProvider.OnRun += RunCharacterOn;
        _gameplayInputProvider.OnRunCancelled += RunCharacterOff;
    }
    private void OnDisable()
    {
        _gameplayInputProvider.OnMoveRL -= MoveCharacterRL;
        _gameplayInputProvider.OnMoveUD -= MoveCharacterUD;
        _gameplayInputProvider.OnJump -= JumpCharacter;
        _gameplayInputProvider.OnRun -= RunCharacterOn;
        _gameplayInputProvider.OnRunCancelled -= RunCharacterOff;
    }


    private void Update()
    {
        HandleAnimation();
        HandleRotation();

        
        _characterController.Move(_currentMovement * Time.deltaTime * (_isRunning ? 2:1));

    }

    private void HandleAnimation()
    {
        //Move as long as the key is pressed
        if (_currentMovement != Vector3.zero)
        {
            animator.SetBool("IsWalking", true);
            //Debug.Log("Walking set to true");
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }
    }

    private void HandleRotation()
    {
        //New point our character should look at
        Vector3 positionToLookAt;
        positionToLookAt.x = _currentMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = _currentMovement.z;


        Quaternion currentRotation = transform.rotation;

        if (_currentMovement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, ROTATIONFACTOR * Time.deltaTime);
        }
    }

    private void JumpCharacter()
    {
        Debug.Log("JUMP");

    }

    private void MoveCharacterRL(float value)
    {
        //Debug.LogFormat("RL Value: {0}", value);

        _currentMovement.x = value;
        //_movementRL = value;
    }

    private void MoveCharacterUD(float value)
    {
        //Debug.LogFormat("UD Value: {0}", value);

        _currentMovement.z = value;
        //_movementUD = value;
    }

    private void RunCharacterOn()
    {
        animator.SetBool("IsRunning", true);
        _isRunning = true;
        //Debug.Log("Run!");
        //_currentMovement *= 2;
    }

    private void RunCharacterOff()
    {
        _isRunning = false;
        animator.SetBool("IsRunning", false);
    }

}
