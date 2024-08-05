using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SController : MonoBehaviour
{
    private SInput _input;
    private CharacterController _cc;
    private Vector3 _movementInput;

    [SerializeField] private Transform _trailPrefab;
    [SerializeField] private Transform _impactPrefab;
    [SerializeField] private Transform _firePos;

    private Quaternion _quaterRotation;
    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _input = GetComponent<SInput>();
        _input.OnMovementEvent += HandleMovementEvent;
        _input.OnFireEvent += HandleFireEvent;

        _quaterRotation = Quaternion.Euler(0, -45f, 0);
    }

    private void HandleFireEvent()
    {
        StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        //·¹ÀÌ¸¦ ½÷¼­
        Vector3 startPosition = _firePos.position;
        float maxRange = 15f;
        bool isHit = Physics.Raycast(
            startPosition, _firePos.forward, out RaycastHit hit, 15f);

        Transform trail = Instantiate(
            _trailPrefab, startPosition, Quaternion.identity);

        yield return null;

        trail.position = isHit ? hit.point : startPosition + _firePos.forward * maxRange;

        if(isHit)
        {
            Transform impact = Instantiate(
            _impactPrefab, hit.point, Quaternion.LookRotation(hit.normal));

            hit.rigidbody?.AddForce(hit.normal * -10f, ForceMode.Impulse);
        }
    }


    private void HandleMovementEvent(Vector3 movement)
    {
        _movementInput = movement;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = _quaterRotation * _movementInput;
        float moveSpeed = 8f;
        _cc.Move(movement * moveSpeed * Time.fixedDeltaTime);
    }
}
