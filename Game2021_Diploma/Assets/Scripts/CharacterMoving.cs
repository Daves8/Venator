using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoving : MonoBehaviour
{
    internal static bool IsReadyToMove = true;
    internal static bool IsReadyToRun = true;
    //internal static bool Fishing = false;

    [SerializeField] private Animator _animator;

    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _camera;

    [SerializeField] private float _speed = 2.0f;
    [SerializeField] private float _turnSmoothTime = 0.1f;
    [SerializeField] private float _gravity = -19.62f;

    static public Vector3 moveDirection;

    private float _turnSmoothVelocity;
    private Vector3 _velocity;

    private float _speedWalk;
    private float _speedRun;

    private bool _isCrouch = false;

    static public bool rotateCharacter = true;

    private void Start()
    {
        //Application.targetFrameRate = 300;
        Cursor.lockState = CursorLockMode.Locked;

        _speedRun = _speed * 4;
        _speedWalk = _speed;
    }

    private void Update()
    {
        // движение
        CharacterMove();
        // падение
        CharacterFalling();
    }

    private void CharacterMove()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (moveDirection.magnitude >= 0.1f && _controller.isGrounded && IsReadyToMove)
        {
            if (!_isCrouch) // если стоим и движемся
            {
                AnimationsStandings();
            }
            else // если сидим и движемся
            {
                _speed = _speedWalk;
                _animator.SetBool("CrouchIdleToCrouchedWalking", true);
            }
            CharacterMove_Func(moveDirection);
        }
        else
        {
            if (Input.GetButtonDown("Crouch"))
            {
                _isCrouch = !_isCrouch;
            }

            if (!_isCrouch) // если стоим
            {
                _animator.SetBool("IdleToCrouch", false);
                _animator.SetBool("IdleToWalking", false);
                _animator.SetBool("WalkingToRun", false);
            }
            else // если сидим
            {
                _animator.SetBool("IdleToCrouch", true);
                _animator.SetBool("CrouchIdleToCrouchedWalking", false);
            }
        }
    }
    private void CharacterMove_Func(Vector3 moveDirection)
    {
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);

        if (rotateCharacter)
        {
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        _controller.Move(moveDir.normalized * _speed * Time.deltaTime);
    }
    private void CharacterFalling()
    {
        if (_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
    private void AnimationsStandings()
    {
        if (Input.GetButton("Change Speeds") && IsReadyToRun)
        {
            _speed = _speedRun;
            _animator.SetBool("WalkingToRun", true);
        }
        else
        {
            _speed = _speedWalk;
            _animator.SetBool("WalkingToRun", false);
        }

        _animator.SetBool("IdleToWalking", true);
    }
}
