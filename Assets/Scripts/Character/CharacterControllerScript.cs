using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerScript : MonoBehaviour
{
    [Header("Constant")]
    [SerializeField]
    private float MOVEMENT_VALUE = 0.01f;
    [SerializeField]
    private float ROTATIONFACTOR = 10f;
    [SerializeField]
    private float GRAVITY = 9.8f;
    [SerializeField]
    private float ABILITY_COOLDOWN = 5f;

    [Header("Id Provider")]

    [SerializeField]
    private IdContainer _IdProvider;

    [Header("Ability")]

    [SerializeField]
    private GameObject _ObjectToSpawn;

    [SerializeField]
    private GameObject _LocationToSpawn;


    private GameplayInputProvider _gameplayInputProvider;

    private float _movementRL;
    private float _movementUD;

    private bool _isRunning = false;

    private Animator animator;

    private CharacterController _characterController;

    private Vector3 _currentMovement;
    private Vector3 _currentJumping;

    private bool _canUseAbility;

    private void Awake()
    {
        _gameplayInputProvider = PlayerController.Instance.GetInput<GameplayInputProvider>(_IdProvider.Id);
        _characterController = GetComponent<CharacterController>();

        _currentMovement = Vector3.zero;
        _currentJumping = Vector3.zero;
        

        animator = GetComponent<Animator>();

        _canUseAbility = true;
    }

    private void OnEnable()
    {
        _gameplayInputProvider.OnMoveRL += MoveCharacterRL;
        _gameplayInputProvider.OnMoveUD += MoveCharacterUD;
        _gameplayInputProvider.OnJump += JumpCharacter;
        _gameplayInputProvider.OnRun += RunCharacterOn;
        _gameplayInputProvider.OnRunCancelled += RunCharacterOff;
        _gameplayInputProvider.OnAbility += UseAbility;
    }
    private void OnDisable()
    {
        _gameplayInputProvider.OnMoveRL -= MoveCharacterRL;
        _gameplayInputProvider.OnMoveUD -= MoveCharacterUD;
        _gameplayInputProvider.OnJump -= JumpCharacter;
        _gameplayInputProvider.OnRun -= RunCharacterOn;
        _gameplayInputProvider.OnRunCancelled -= RunCharacterOff;
        _gameplayInputProvider.OnAbility -= UseAbility;
    }


    private void Update()
    {
        HandleAnimation();
        HandleRotation();

        if (!_characterController.isGrounded)
        {
            _currentJumping.y -= GRAVITY * Time.deltaTime;
            
        }

        _characterController.Move(_currentMovement * Time.deltaTime * (_isRunning ? 2:1));

        _characterController.Move(_currentJumping * Time.deltaTime);

    }

    private void HandleAnimation()
    {
        //Move as long as the key is pressed
        if (_currentMovement.x != 0f || _currentMovement.z != 0f)
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
        if (_characterController.isGrounded)
        {
            _currentJumping.y = 5f;

            Debug.Log("JUMP");
        }

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

    private void UseAbility()
    {
        if (_canUseAbility)
        {
            _canUseAbility = false;
            Debug.Log("Ability used");
            SpawnObject();
            StartCoroutine(AbilityCooldownTime());
        }

        return;
    }

    private IEnumerator AbilityCooldownTime()
    {
        yield return new WaitForSeconds(ABILITY_COOLDOWN);
        _canUseAbility = true;
    }

    private void SpawnObject()
    {
        GameObject box = Instantiate(_ObjectToSpawn, _LocationToSpawn.transform);
        box.transform.parent = null;
    }
}
