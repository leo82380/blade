using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbTrm : MonoBehaviour
{
    private Transform _playerAddOnTrm;

    private bool _isFollowing;
    private bool _rotate;
    private float _rotateSpeed = 1f;

    private void Start()
    {
        _playerAddOnTrm = PlayerManager.Instance.PlayerTrm.Find("AddOnTrm");
    }

    public void SetFollow(bool value)
    {
        _isFollowing = value;
    }

    public void SetRotate(bool value, float speed)
    {
        _rotate = value;
        _rotateSpeed = speed;
    }

    private void LateUpdate()
    {
        if (_isFollowing)
            transform.position = _playerAddOnTrm.position;

        if(_rotate)
        {
            float speed = _rotateSpeed * Time.deltaTime;
            transform.Rotate(new Vector3(0, speed, 0), Space.Self);
        }
    }
}
