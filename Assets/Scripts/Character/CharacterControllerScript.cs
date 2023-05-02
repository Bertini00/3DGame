using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{

    private const float MOVEMENT_VALUE = 0.01f;

    [SerializeField]
    private IdContainer _IdProvider;


    private GameplayInputProvider _gameplayInputProvider;

    private float _movementRL;
    private float _movementUD;

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
        _gameplayInputProvider.OnRun += RunCharacter;
    }
    private void OnDisable()
    {
        _gameplayInputProvider.OnMoveRL -= MoveCharacterRL;
        _gameplayInputProvider.OnMoveUD -= MoveCharacterUD;
        _gameplayInputProvider.OnJump -= JumpCharacter;
        _gameplayInputProvider.OnRun -= RunCharacter;
    }


    private void Update()
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

        _characterController.Move(_currentMovement * Time.deltaTime);

    }

    private void JumpCharacter()
    {
        Debug.Log("JUMP");
    }

    private void MoveCharacterRL(float value)
    {
        Debug.LogFormat("RL Value: {0}", value);

        _currentMovement.x = value;
        //_movementRL = value;
    }

    private void MoveCharacterUD(float value)
    {
        Debug.LogFormat("UD Value: {0}", value);

        _currentMovement.z = value;
        //_movementUD = value;
    }

    private void RunCharacter()
    {
        animator.SetBool("IsRunning", true);
        _currentMovement *= 2;
    }

}
