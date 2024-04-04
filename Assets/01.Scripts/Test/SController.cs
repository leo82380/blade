using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Transform _trailPrefab;
    [SerializeField] private Transform _impactPrefab;
    [SerializeField] private Transform _firePos;
    
    private CharacterController _characterController;
    private SInput _input;
    private Vector3 _movementInput;

    private Quaternion _quaterRotation;
    private void Awake()
    {
        _input = GetComponent<SInput>();
        _characterController = GetComponent<CharacterController>();
        _quaterRotation = Quaternion.Euler(0f, -45f, 0f);
        
        _input.OnMovementEvent += HandleMovementEvent;
        _input.OnFireEvent += HandleFireEvent;
    }

    private void HandleFireEvent()
    {
        StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        Vector3 startPos = _firePos.position;
        float maxRange = 15f;
        bool isHit = Physics.Raycast(
            startPos, _firePos.forward, out RaycastHit hit, maxRange);

        Transform trail = Instantiate(_trailPrefab, startPos, Quaternion.identity);
        yield return null;
        
        trail.position = isHit ? hit.point : startPos + _firePos.forward * maxRange;
        
        if (isHit)
        {
            Transform impact = Instantiate(_impactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            
            hit.rigidbody?.AddForce(_firePos.forward * 10f, ForceMode.Impulse);
        }

    }

    private void OnDestroy()
    {
        _input.OnMovementEvent -= HandleMovementEvent;
        _input.OnFireEvent -= HandleFireEvent;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = _quaterRotation * _movementInput;
        _characterController.Move(movement * (_speed * Time.fixedDeltaTime));
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        _movementInput = movement;
    }

    
    
    
}
